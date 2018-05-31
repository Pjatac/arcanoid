using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace arcanoid
{
	public class GameObjectHelper
	{
		public static Image ReadImageFromFile(string fileName, int width, int height)
		{
			BitmapImage bitmap = new BitmapImage()
			{
				UriSource = new Uri(fileName, UriKind.RelativeOrAbsolute)
			};
			return new Image { Width = width, Height = height, Stretch = Stretch.Fill, Source = bitmap };
		}
		public static void AddToGameBoard(Image Image, int TopLeftPosistionY, int TopLeftPosistionX, Canvas canvas)
		{
			Canvas.SetTop(Image, TopLeftPosistionY);
			Canvas.SetLeft(Image, TopLeftPosistionX);
			canvas.Children.Add(Image);
		}

		public static async void InsertRecord(int score, string playerName, Canvas myCanvas)
		{
			int[] table = new int[10];

			StorageFolder folder = ApplicationData.Current.LocalFolder;
			string fileName = folder.Path +"\\hiscores.txt";
			if (!File.Exists(fileName))
			  await folder.CreateFileAsync("hiscores.txt");
			var _File = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appdata:///local/hiscores.txt"));
			IList<string> text = await FileIO.ReadLinesAsync(_File);
			if (text.Count != 0)
				for (int i = 0; i < text.Count; i++)
					table[i] = GetScore(text[i]);
			bool ins = false;
			if (text.Count == 0)
			{
				text.Add(playerName + "\t" + Convert.ToString(score));
				ins = true;
			}
			else if (text.Count < 10 && score < table[text.Count - 1])
			{
				text.Add(playerName + "\t" + Convert.ToString(score));
				ins = true;
			}
			else
			{
				for (int i = 0; i < text.Count && !ins; i++)
					if (score >= table[i] && text.Count == 10)
					{
						text.Insert(i, playerName + "\t" + Convert.ToString(score));
						text.RemoveAt(text.Count - 1);
						ins = true;
					}
					else if (score >= table[i] && !ins)
					{
						text.Insert(i, playerName + "\t" + Convert.ToString(score));
						ins = true;
					}
			}
			await FileIO.WriteLinesAsync(_File, text);
			

			TextBlock textHi = new TextBlock()
			{
				Visibility = Visibility.Visible,
				Foreground = new SolidColorBrush(Colors.YellowGreen),
				Height = 250,
				Width = 220,
				FontSize = 15,
				Text = "HISCORES\n",
				TextAlignment = TextAlignment.Center
			};
			Canvas.SetLeft(textHi, 280);
			Canvas.SetTop(textHi, 210);
			for (int i = 0; i < text.Count; i++)
			{
				textHi.Text += text[i] + "\n";
			}
			myCanvas.Children.Add(textHi);
		}

		public static int GetScore(string str)
		{
			int currentscore = 0;
			for (int i = str.Length - 1, j = 1; str[i] != '\t'; i--)
			{
				currentscore += (int)Char.GetNumericValue(str[i]) * j;
				j *= 10;
			}
			return currentscore;
		}
		public static void InitGameBoard(Canvas myCanvas, TextBlock Level, TextBlock TextBlockTime, TextBlock Score, TextBlock TextBlockLives)
		{
			TextBlock ScoreTxt = new TextBlock()
			{
				Visibility = Visibility.Visible,
				Foreground = new SolidColorBrush(Colors.White),
				Height = 20,
				Width = 75,
				TextAlignment = TextAlignment.Center,
				Text = "SCORES:"
			};
			Canvas.SetTop(ScoreTxt, 0);
			Canvas.SetLeft(ScoreTxt, 200);
			myCanvas.Children.Add(ScoreTxt);

			TextBlock LevelTxt = new TextBlock()
			{
				Visibility = Visibility.Visible,
				Foreground = new SolidColorBrush(Colors.White),
				Height = 20,
				Width = 75,
				TextAlignment = TextAlignment.Center,
				Text = "LEVEL:"
			};
			Canvas.SetTop(LevelTxt, 0);
			Canvas.SetLeft(LevelTxt, 100);
			myCanvas.Children.Add(LevelTxt);

			TextBlock LivesTxt = new TextBlock()
			{
				Visibility = Visibility.Visible,
				Foreground = new SolidColorBrush(Colors.White),
				Height = 20,
				Width = 75,
				TextAlignment = TextAlignment.Center,
				Text = "LIVES:"
			};
			Canvas.SetTop(LivesTxt, 0);
			Canvas.SetLeft(LivesTxt, 300);
			myCanvas.Children.Add(LivesTxt);

			TextBlock Help = new TextBlock()
			{
				Visibility = Visibility.Visible,
				Foreground = new SolidColorBrush(Colors.White),
				Height = 20,
				Width = 400,
				TextAlignment = TextAlignment.Center,
				Text = "Press 'Q' for exit / Press 'Enter' if you want to take a rest"
			};
			Canvas.SetTop(Help, 0);
			Canvas.SetLeft(Help, 400);
			myCanvas.Children.Add(Help);

			Level.Visibility = Visibility.Visible;
			Level.Foreground = new SolidColorBrush(Colors.White);
			Level.Text = "1";
			Level.Height = 20;
			Level.Width = 75;
			Canvas.SetTop(Level, 0);
			Canvas.SetLeft(Level, 175);
			myCanvas.Children.Add(Level);

			TextBlockLives.Visibility = Visibility.Visible;
			TextBlockLives.Foreground = new SolidColorBrush(Colors.White);
			TextBlockLives.Text = "3";
			TextBlockLives.Height = 20;
			TextBlockLives.Width = 75;
			Canvas.SetTop(TextBlockLives, 0);
			Canvas.SetLeft(TextBlockLives, 375);
			myCanvas.Children.Add(TextBlockLives);

			TextBlockTime.Visibility = Visibility.Visible;
			TextBlockTime.Foreground = new SolidColorBrush(Colors.White);
			TextBlockTime.Text = "TIME";
			TextBlockTime.Height = 20;
			TextBlockTime.Width = 75;
			Canvas.SetTop(TextBlockTime, 0);
			Canvas.SetLeft(TextBlockTime, 0);
			myCanvas.Children.Add(TextBlockTime);

			Score.Visibility = Visibility.Visible;
			Score.Text = "0";
			Score.Foreground = new SolidColorBrush(Colors.White);
			Score.Height = 20;
			Score.Width = 75;
			Canvas.SetTop(Score, 0);
			Canvas.SetLeft(Score, 275);
			myCanvas.Children.Add(Score);
		}

		public static void GameOver(string playerName, int score, Canvas myCanvas)
		{
			StorageFolder folder = ApplicationData.Current.LocalFolder;
			string fileName = folder.Path + "/" + playerName + ".xml";
			if (File.Exists(fileName))
				File.Delete(fileName);
			TextBlock text = new TextBlock()
			{
				Foreground = new SolidColorBrush(Colors.LightCoral),
				Height = 100,
				Width = 200,
				FontSize = 35,
				Text = "GAME OVER"
			};
			Canvas.SetLeft(text, 290);
			Canvas.SetTop(text, 150);
			myCanvas.Children.Add(text);
			InsertRecord(score, playerName, myCanvas);
		}
	}
}

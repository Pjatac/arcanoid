using System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace arcanoid
{
	public sealed partial class Pause : Page
	{
		public string playerName;
		public Canvas myCanvas = new Canvas();
		byte[] neighbor = new byte[8];
		byte[,] life = new byte[30, 30];
		byte[,] move = new byte[30, 30];
		Random r = new Random();
		Color randomColor1 = Color.FromArgb(byte.MaxValue, 0, 0, 0);
		Color randomColor2 = Color.FromArgb(byte.MaxValue, 255, 255, 255);
		Brush br1;
		Brush br2;
		DispatcherTimer FirstTimer = new DispatcherTimer();
		public Pause()
		{
			this.InitializeComponent();
			CreateCanvas();
		}
		private void CreateCanvas()
		{
			myCanvas.Height = 340;
			myCanvas.Width = 300;
			Button Return = new Button();
			Return.Click += delegate
			{
				Frame.Navigate(typeof(MainPage), "continue");
			};
			Return.Content = "RETURN";
			Canvas.SetLeft(Return, 120);
			Canvas.SetTop(Return, 310);
			myCanvas.Children.Add(Return);
			br1 = new SolidColorBrush(randomColor1);
			br2 = new SolidColorBrush(randomColor2);


			life = RandomFull(30, 30);

			for (int i = 0; i < 30; i++)
				for (int j = 0; j < 30; j++)
				{
					Rectangle rec = new Rectangle
					{
						Height = 10,
						Width = 10
					};
					Canvas.SetLeft(rec, i * 10);
					Canvas.SetTop(rec, j * 10);
					if (life[i, j] == 0) rec.Fill = br1; else rec.Fill = br2;
					myCanvas.Children.Add(rec);
				}
			FirstTimer.Interval = new TimeSpan(0, 0, 0, 1);
			FirstTimer.Tick += Timer_Tick;
			FirstTimer.Start();
			this.Content = myCanvas;
		}
		private byte[,] RandomFull(int str, int col)
		{
			byte[,] life = new byte[str, col];
			for (int i = 1; i < str - 1; i++)
				for (int j = 1; j < col - 1; j++)
					life[i, j] = (byte)r.Next(2);
			return life;
		}
		private void Timer_Tick(object sender, object e)
		{
			int endOfmoves = 0;
			for (int i = 0; i < 30; i++)
				for (int j = 0; j < 30; j++)

					if (i > 1 && j > 1 && i < 29 & j < 29)
					{
						neighbor[0] = life[i - 1, j - 1];
						neighbor[1] = life[i - 1, j];
						neighbor[2] = life[i - 1, j + 1];
						neighbor[3] = life[i, j - 1];
						neighbor[4] = life[i, j + 1];
						neighbor[5] = life[i + 1, j - 1];
						neighbor[6] = life[i + 1, j];
						neighbor[7] = life[i + 1, j + 1];
						int sum = 0;
						for (int k = 0; k < 8; k++)
						{
							sum += neighbor[k];
						}
						if (life[i, j] == 0 && sum == 3)
						{
							move[i, j] = 1;
							endOfmoves = 1;
						}
						if (life[i, j] == 1 && sum < 2)
						{
							move[i, j] = 0;
							endOfmoves = 1;
						}
						else if (life[i, j] == 1 && sum > 3)
						{
							move[i, j] = 0;
							endOfmoves = 1;
						}
					}
			for (int i = 0; i < 30; i++)
				for (int j = 0; j < 30; j++)
					life[i, j] = move[i, j];

			int number1 = 0, number2 = 0;
			foreach (UIElement ui in myCanvas.Children)
			{
				if (ui is Rectangle)
				{
					if (life[number1, number2] == 0)
					{
						(ui as Rectangle).Fill = br1;
					}
					else
					{
						randomColor1 = Color.FromArgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255));
						br2 = new SolidColorBrush(randomColor1);
						(ui as Rectangle).Fill = br2;
					}
						number2++;
					if (number2 == 30)
					{
						number1++;
						number2 = 0;
					}
				}
			}
			// Restart if moves end
			if (endOfmoves == 0) 
			{
				life = RandomFull(30, 30);
				number1 = 0;
				number2 = 0;
				foreach (UIElement ui in myCanvas.Children)
				{
					if (ui is Rectangle)
					{
						if (life[number1, number2] == 0)
						{
							(ui as Rectangle).Fill = br1;
						}
						else
						{
							randomColor1 = Color.FromArgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255));
							br2 = new SolidColorBrush(randomColor1);
							(ui as Rectangle).Fill = br2;
						}
						number2++;
						if (number2 == 30)
						{
							number1++;
							number2 = 0;
						}
					}
				}
			}
		}
	}
}

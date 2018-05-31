using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.System;
using Windows.Storage;
using Windows.UI.Popups;
using System.IO;
using Windows.Media.Playback;

namespace arcanoid
{
	public sealed partial class MainPage : Page
	{
		DispatcherTimer _dispatcherTimer = new DispatcherTimer();
		TextBlock Level = new TextBlock();
		TextBlock TextBlockTime = new TextBlock();
		TextBlock Score = new TextBlock();
		TextBlock TextBlockLives = new TextBlock();
		GameBoard Game;
		String playerName;
		Canvas myCanvas = new Canvas();
		public MediaPlayer _mpBgr = new MediaPlayer();

		public MainPage()
		{
			this.InitializeComponent();
			NavigationCacheMode = NavigationCacheMode.Enabled;
			CreateCanvas();
			Game = new GameBoard(myCanvas);
			GameObjectHelper.InitGameBoard(myCanvas, Level, TextBlockTime, Score, TextBlockLives);
			_mpBgr.Source = Windows.Media.Core.MediaSource.CreateFromUri(new Uri(@"ms-appx:///Assets/sound.mp3", UriKind.RelativeOrAbsolute));
			_mpBgr.Play();
			StartGameTimer();
		}
		private void StartGameTimer()
		{
			_dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 1);
			_dispatcherTimer.Tick += DispatcherTimer_Tick;
			_dispatcherTimer.Start();
		}
		private void DispatcherTimer_Tick(object sender, object e)
		{
			TextBlockTime.Text = DateTime.Now.Hour.ToString() + " : " + DateTime.Now.Minute.ToString() + " : " + DateTime.Now.Second.ToString();
			Score.Text = Convert.ToString(Game._score);
			TextBlockLives.Text = Convert.ToString(Game._lives);
			Level.Text = Convert.ToString(Game._level);
			if (!Game.Recalculate(Window.Current.CoreWindow.GetKeyState(VirtualKey.Left).HasFlag(CoreVirtualKeyStates.Down), Window.Current.CoreWindow.GetKeyState(VirtualKey.Right).HasFlag(CoreVirtualKeyStates.Down), Window.Current.CoreWindow.GetKeyState(VirtualKey.Space).HasFlag(CoreVirtualKeyStates.Down)))
			{
				Score.Text = Convert.ToString(Game._score);
				TextBlockLives.Text = Convert.ToString(Game._lives);
				GameOver();
			}
			if (Window.Current.CoreWindow.GetKeyState(VirtualKey.Enter).HasFlag(CoreVirtualKeyStates.Down))
			{
				_dispatcherTimer.Stop();
				_mpBgr.Pause();
				Frame.Navigate(typeof(Pause), playerName);
			}
			if (Window.Current.CoreWindow.GetKeyState(VirtualKey.Q).HasFlag(CoreVirtualKeyStates.Down))
			{
				_dispatcherTimer.Stop();
				Quite();
			}
			if (Game._iteration % 1000 == 0)
				Game.LevelUp();
		}
		private async void Quite()
		{
			_mpBgr.Pause();
			MessageDialog Quite = new MessageDialog("Do you realy want to quite? (Your game will be save)");
			Quite.Commands.Clear();
			Quite.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.Save)));
			Quite.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.Return)));
			await Quite.ShowAsync();
		}
		private void Return(IUICommand command)
		{
			_dispatcherTimer.Start();
			_mpBgr.Play();
		}
		private async void Save(IUICommand command)
		{
			await XMLIO.SaveObjectToXml(XMLIO.Trans(Game, Game._gameObjects), playerName);
			Application.Current.Exit();
		}
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			playerName = e.Parameter as string;
			if (playerName != "continue")
			{
				StorageFolder folder = ApplicationData.Current.LocalFolder;
				string fileName = folder.Path + "/" + playerName + ".xml";
				if (File.Exists(fileName))
					XMLIO.Load(playerName, Game, myCanvas);
			}
			_mpBgr.Play();
			_dispatcherTimer.Start();
		}
		private void GameOver()
		{
			_dispatcherTimer.Stop();
			_dispatcherTimer.Tick -= DispatcherTimer_Tick;
			GameObjectHelper.GameOver(playerName, Game._score, myCanvas);
			Restart();
		}
		public void CreateCanvas()
		{
			myCanvas.Height = 450;
			myCanvas.Width = 800;
			this.Content = myCanvas;
		}
		public async void Restart()
		{
			MessageDialog Quite = new MessageDialog("Do you want to restart? ");
			Quite.Commands.Clear();
			Quite.Commands.Add(new UICommand("Yes", new UICommandInvokedHandler(this.Yes)));
			Quite.Commands.Add(new UICommand("No", new UICommandInvokedHandler(this.No)));
			await Quite.ShowAsync();
		}
		private void No(IUICommand command)
		{
			Application.Current.Exit();
		}
		private void Yes(IUICommand command)
		{
			myCanvas.Children.Clear();
			Game = new GameBoard(myCanvas);
			GameObjectHelper.InitGameBoard(myCanvas, Level, TextBlockTime, Score, TextBlockLives);
			_mpBgr.Play();
			StartGameTimer();
		}
	}
}

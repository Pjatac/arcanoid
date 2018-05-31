using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;


namespace arcanoid
{
	public sealed partial class Start : Page
	{
		public Start()
		{
			this.InitializeComponent();
		}

		private async void BtnStart_Click(object sender, RoutedEventArgs e)
		{
			if (NameBox.Text == "continue")
			{
				MessageDialog Ouups = new MessageDialog("This name impssible");
				await Ouups.ShowAsync();
			}
			else Frame.Navigate(typeof(MainPage), NameBox.Text);
		}
	}
}

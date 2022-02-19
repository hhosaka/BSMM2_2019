using BSMM2.Models;
using BSMM2.Resource;
using BSMM2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayerPage : ContentPage {
		private IEnumerable<string> _excludes = new[] { AppResources.TextPlayerName, AppResources.TextDropped };

		public PlayerPage(BSMMApp app, Player player) {
			InitializeComponent();
			BindingContext = new PlayerViewModel(app, player);

			int i = 2;
			foreach (var param in player.Export(app.Game, new ExportData()).Where(param => !_excludes.Any(key => key == param.Key))) {
				CreateLabel(i, 0, param.Key);
				CreateLabel(i++, 1, param.Value);
				grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
			}

			void CreateLabel(int row, int col, object text) {
				var label = new Label() { Text = text.ToString(), BackgroundColor = row % 2 == 1 ? Color.LightGray : Color.White };
				grid.Children.Add(label);
				Grid.SetColumn(label, col);
				Grid.SetRow(label, row);
			}
		}

		private async void Back(object sender, EventArgs e)
			=> await Navigation.PopModalAsync();

		private void OnClosing(object sender, EventArgs e)
			=> MessagingCenter.Send<object>(this, Messages.REFRESH);
	}
}
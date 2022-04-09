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
		private const int ADDTIONAL_ROW_POSITION = 3;// Under the ByeMatchCount
		private IEnumerable<string> _excludes = new[] { AppResources.TextPlayerName, AppResources.TextDropped };

		public PlayerPage(BSMMApp app, Player player) {
			InitializeComponent();

			int i = ADDTIONAL_ROW_POSITION;
			var data = player.Point.Export(new ExportSource())
				.Union(player.OpponentPoint.Export(new ExportSource(),"opponent_"));
			foreach (var param in data) {
				grid.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
				CreateLabel(i, 0, param.Key);
				CreateLabel(i++, 1, param.Value);
			}

			BindingContext = new PlayerViewModel(app, player);

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
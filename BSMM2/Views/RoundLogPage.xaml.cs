using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundLogPage : ContentPage {

		public RoundLogPage(Game game, int index) {
			InitializeComponent();

			BindingContext = new RoundLogViewModel(game.Rounds.ElementAt(index), index, showMatch);

			async void showMatch(Match match) {
				await Navigation.PushModalAsync(new NavigationPage(game.CreateMatchPage(match)));
				RoundListView.SelectedItem = null;
			}
		}

		private async void Back(object sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}
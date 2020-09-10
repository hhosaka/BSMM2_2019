using BSMM2.Models;
using BSMM2.Resource;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundPage : ContentPage {
		private RoundViewModel viewModel;

		public RoundPage(BSMMApp app) {
			InitializeComponent();

			BindingContext = viewModel = new RoundViewModel(app, showRoundsLog, FailToMakeMatch);

			void showRoundsLog()
				=> Navigation.PushModalAsync(new RoundsLogPage(app));

			async void FailToMakeMatch(string message) {
				if (await DisplayAlert(AppResources.TextAlert, message, AppResources.TextGoToRuleSetting, AppResources.TextGiveUp)) {
					await Navigation.PushModalAsync(new NavigationPage(app.Game.CreateRulePage()));
				}
			}
		}

		private void OpenHelpPage(object sender, EventArgs e) => Navigation.PushModalAsync(new WebPage("https://sites.google.com/site/hhosaka183/bs-match-maker-2"));

		private async void OnMatchTapped(object sender, ItemTappedEventArgs args) {
			if (args.Item is Match match && viewModel.Game.ActiveRound.IsPlaying && !match.IsByeMatch)
				await Navigation.PushModalAsync(new NavigationPage(viewModel.Game.CreateMatchPage(match)));

			RoundListView.SelectedItem = null;
		}
	}
}
using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayersPage : ContentPage {
		private PlayersViewModel viewModel;
		private BSMMApp _app;

		public PlayersPage(BSMMApp app) {
			_app = app;
			InitializeComponent();

			BindingContext = viewModel = new PlayersViewModel(app, NewGame, SelectGame, DeleteGame, AddPlayer);

			void NewGame()
				=> Navigation.PushModalAsync(new NavigationPage(new NewGamePage(_app)));
			void SelectGame()
				=> Navigation.PushModalAsync(
					new NavigationPage(new GamesPage(_app, "Select Item", selectGame)));
			void DeleteGame()
				=> Navigation.PushModalAsync(
					new NavigationPage(new GamesPage(_app, "Delete Item", deleteGame)));
			void AddPlayer()
				=> Navigation.PushModalAsync(new NavigationPage(new AddPlayerPage(_app)));

			async void selectGame(Game game) {
				Debug.Assert(game != null);
				app.Game = game;
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				await Navigation.PopModalAsync();
			}

			async void deleteGame(Game game) {
				Debug.Assert(game != null);
				if (app.Remove(game)) {
					MessagingCenter.Send<object>(this, Messages.REFRESH);
					await Navigation.PopModalAsync();
				}
			}
		}

		private void Log(object sender, EventArgs e) {
			DisplayAlert("log", new Storage().Log(), "Finish");
		}

		private void OpenRuleSettingPage(object sender, EventArgs e)
			=> Navigation.PushModalAsync(new NavigationPage(_app.Game.CreateRulePage()));

		private void OpenSettingsPage(object sender, EventArgs e)
			=> Navigation.PushModalAsync(new NavigationPage(new SettingsPage(_app)));

		private void OpenHelpPage(object sender, EventArgs e)
			=> Navigation.PushModalAsync(new WebPage("https://sites.google.com/site/hhosaka183/bs-match-maker-2"));

		private async void OnPlayerTapped(object sender, ItemTappedEventArgs args) {
			if (args.Item is OrderedPlayer player)
				await Navigation.PushModalAsync(new NavigationPage(new PlayerPage(_app, player.Player)));
			PlayersListView.SelectedItem = null;
		}
	}
}
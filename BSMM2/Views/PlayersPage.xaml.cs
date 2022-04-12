using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlayersPage : ContentPage, IDisposable {
		private BSMMApp _app;
		private PlayersViewModel _viewModel;
		public PlayersPage(BSMMApp app) {
			_app = app;
			InitializeComponent();

			_viewModel = new PlayersViewModel(app);
			_viewModel.OnNewGame+= NewGame;
			_viewModel.OnSelectGame += SelectGame;
			_viewModel.OnDeleteGame += DeleteGame;
			_viewModel.OnAddPlayer += AddPlayer;
			_viewModel.OnShowQRCode += ShowQRCode;
			BindingContext = _viewModel;
		}
		private void NewGame()
			=> Navigation.PushModalAsync(new NavigationPage(new NewGamePage(_app)));
		void SelectGame() {
			Navigation.PushModalAsync(
				new NavigationPage(new GamesPage(_app, "Select Item", selectGame)));

			async void selectGame(Game game) {
				Debug.Assert(game != null);
				_app.Game = game;
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				await Navigation.PopModalAsync();
			}
		}

		void DeleteGame() {
			Navigation.PushModalAsync(
				new NavigationPage(new GamesPage(_app, "Delete Item", deleteGame)));

			async void deleteGame(Game game) {
				Debug.Assert(game != null);
				if (_app.Remove(game)) {
					MessagingCenter.Send<object>(this, Messages.REFRESH);
					await Navigation.PopModalAsync();
				}
			}
		}
		void AddPlayer()
			=> Navigation.PushModalAsync(new NavigationPage(new AddPlayerPage(_app)));

		void ShowQRCode()
			=> Navigation.PushModalAsync(new NavigationPage(new WebServicePage(_app, "games/players/")));

		private void Log(object sender, EventArgs e) {
			DisplayAlert("log", new Storage().Log(), "Finish");
		}

		private void OpenRuleSettingPage(object sender, EventArgs e)
			=> Navigation.PushModalAsync(new NavigationPage(_app.Game.CreateRulePage()));

		private void OpenSettingsPage(object sender, EventArgs e)
			=> Navigation.PushModalAsync(new NavigationPage(new SettingsPage(_app)));

		private void OpenHelpPage(object sender, EventArgs e)
			=> Navigation.PushModalAsync(new NavigationPage(new WebPage("https://sites.google.com/site/hhosaka183/bs-match-maker-2")));
		
		private async void OnPlayerTapped(object sender, ItemTappedEventArgs args) {
			if (args.Item is OrderedPlayer player)
				await Navigation.PushModalAsync(new NavigationPage(new PlayerPage(_app, player.Player)));
			PlayersListView.SelectedItem = null;
		}

		public void Dispose() {
			_viewModel.OnNewGame -= NewGame;
		}
	}
}
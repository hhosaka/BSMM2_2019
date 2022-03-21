using BSMM2.Models;
using BSMM2.Resource;
using BSMM2.ViewModels;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundPage : ContentPage, UI {
		private RoundViewModel viewModel;

		public RoundPage(BSMMApp app) {
			InitializeComponent();

			BindingContext = viewModel = new RoundViewModel(app, showRoundsLog,  this);

			void showRoundsLog()
				=> Navigation.PushModalAsync(new RoundsLogPage(app));
		}

		private void OpenHelpPage(object sender, EventArgs e) => Navigation.PushModalAsync(new WebPage("https://sites.google.com/site/hhosaka183/bs-match-maker-2"));

		private void OpenQRCode2PlayersPage(object sender, EventArgs e) {
			string urlEnc = System.Web.HttpUtility.UrlEncode(BSMMApp.WebURL+"matches");
			Navigation.PushModalAsync(new WebPage($"https://chart.googleapis.com/chart?cht=qr&chl={urlEnc}&chs=300x300&chld=H|1"));
		}

		private async void OnMatchTapped(object sender, ItemTappedEventArgs args) {
			if (args.Item is Match match && viewModel.Game.ActiveRound.IsPlaying && !match.IsByeMatch)
				await Navigation.PushModalAsync(new NavigationPage(viewModel.Game.CreateMatchPage(match)));

			RoundListView.SelectedItem = null;
		}

		public void PushPage(Page page)
			=>Navigation.PushModalAsync(new NavigationPage(page));
	}
}
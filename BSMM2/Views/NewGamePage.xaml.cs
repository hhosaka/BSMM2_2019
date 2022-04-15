using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewGamePage : ContentPage {

		public NewGamePage(BSMMApp app) {
			InitializeComponent();
			var viewModel = new NewGameViewModel(app, async () => await Navigation.PopModalAsync());
			viewModel.OnSettingPoint +=
				() => Navigation.PushModalAsync(new NavigationPage(new PointSettingPage(app.Rule.PointRule)));
			BindingContext = viewModel;
		}

		private async void Back(object sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}
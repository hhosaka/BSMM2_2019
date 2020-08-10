using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GamesPage : ContentPage {
		private GamesViewModel _viewModel;

		public GamesPage(BSMMApp app, string title, Action<Game> action) {
			InitializeComponent();
			_viewModel = new GamesViewModel(app, title, action);
			BindingContext = _viewModel;
		}

		private async void Back(object sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}
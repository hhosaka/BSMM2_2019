using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddPlayerPage : ContentPage {

		public AddPlayerPage(BSMMApp app) {
			InitializeComponent();
			BindingContext = new AddPlayerViewModel(app, () => Navigation.PopModalAsync());
		}

		private async void Back(object sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}
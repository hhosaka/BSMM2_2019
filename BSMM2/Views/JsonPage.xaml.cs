using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class JsonPage : ContentPage {

		public JsonPage(BSMMApp app) {
			InitializeComponent();
			BindingContext = new JsonViewModel(app, () => Navigation.PopModalAsync());
		}

		private async void Back(object sender, EventArgs args)
			=> await Navigation.PopModalAsync();
	}
}
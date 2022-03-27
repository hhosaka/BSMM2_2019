using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WebServicePage : ContentPage {

		public WebServicePage(BSMMApp app, string serviceId) {
			InitializeComponent();
			BindingContext = new WebServiceViewModel(app, serviceId);
		}

		private async void Back(object sender, EventArgs args)
			=> await Navigation.PopModalAsync();
	}
}
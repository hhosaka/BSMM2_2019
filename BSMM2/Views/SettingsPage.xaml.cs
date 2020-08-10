using BSMM2.Models;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage {

		public SettingsPage(BSMMApp app) {
			InitializeComponent();
			BindingContext = app;
		}

		private async void Back(object sender, EventArgs args)
			=> await Navigation.PopModalAsync();

		private void OnClosing(object sender, EventArgs e)
			=> MessagingCenter.Send<object>(this, Messages.REFRESH);
	}
}
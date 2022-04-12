using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class WebPage : ContentPage
	{
		public WebPage(string url) {
			InitializeComponent();
			BindingContext = new WebViewModel(url);
		}

		private async void Back(object sender, EventArgs args)
			=> await Navigation.PopModalAsync();
	}
}
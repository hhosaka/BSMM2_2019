using BSMM2.Models;
using BSMM2.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PointSettingPage : ContentPage
	{
		public PointSettingPage(PointRule pointRule) {
			InitializeComponent();
			BindingContext = pointRule;
		}

		private async void Back(object sender, EventArgs args)
			=> await Navigation.PopModalAsync();
		private async void Default(object sender, EventArgs args) {
			((PointRule)BindingContext).Reset();
			await Navigation.PopModalAsync();
		}
	}
}
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Models.Matches.MultiMatch.FiveGameMatch {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FiveGameMatchPage : ContentPage {

		public FiveGameMatchPage(FiveGameMatchRule rule, Match match) {
			InitializeComponent();
			BindingContext = new FiveGameMatchViewModel(rule, (MultiMatch)match, Back);
		}

		private void Back(object sender, EventArgs e)
			=> Back();

		private async void Back()
			=> await Navigation.PopModalAsync();
	}
}
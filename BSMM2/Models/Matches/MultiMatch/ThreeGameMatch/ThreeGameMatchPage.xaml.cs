using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Models.Matches.MultiMatch.ThreeGameMatch {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThreeGameMatchPage : ContentPage {

		public ThreeGameMatchPage(ThreeGameMatchRule rule, Match match) {
			InitializeComponent();
			BindingContext = new ThreeGameMatchViewModel(rule, (MultiMatch)match, Back);
		}

		private void Back(object sender, EventArgs e)
			=> Back();

		private async void Back()
			=> await Navigation.PopModalAsync();
	}
}
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Models.Matches.MultiMatch.NthGameMatch {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NthGameMatchPage : ContentPage {

		public NthGameMatchPage(NthGameMatchRule rule, Match match) {
			InitializeComponent();
			BindingContext = new NthGameMatchViewModel(rule, (MultiMatch)match, Back);
		}

		private void Back(object sender, EventArgs e)
			=> Back();

		private async void Back()
			=> await Navigation.PopModalAsync();
	}
}
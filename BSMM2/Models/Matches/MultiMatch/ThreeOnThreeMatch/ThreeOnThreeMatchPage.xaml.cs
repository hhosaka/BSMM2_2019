using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ThreeOnThreeMatchPage : ContentPage {
		private ThreeOnThreeMatchViewModel _viewModel;

		public ThreeOnThreeMatchPage(ThreeOnThreeMatchRule rule, Match match) {
			InitializeComponent();
			Title = String.Format("{0} Result", rule.Name);
			BindingContext = _viewModel = new ThreeOnThreeMatchViewModel(rule, (MultiMatch)match, Back);
		}

		private void Back(object sender, EventArgs e)
			=> Back();

		private async void Back()
			=> await Navigation.PopModalAsync();
	}
}
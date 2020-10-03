using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Models.Matches.SingleMatch {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingleMatchSimplePage : ContentPage {

		public SingleMatchSimplePage(SingleMatchRule rule, SingleMatch match) {
			InitializeComponent();
			Title = rule.Name;
			BindingContext = new SingleMatchSimpleViewModel(rule, match, () => Back(null, null));
		}

		private async void Back(object sender, EventArgs e)
			=> await Navigation.PopModalAsync();
	}
}
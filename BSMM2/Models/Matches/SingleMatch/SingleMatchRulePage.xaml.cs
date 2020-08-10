using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Models.Matches.SingleMatch {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingleMatchRulePage : ContentPage {

		public SingleMatchRulePage(Game game) {
			InitializeComponent();
			BindingContext = new SingleMatchRuleViewModel(game);
		}

		private async void Back(object sender, EventArgs args)
			=> await Navigation.PopModalAsync();

		private void OnClosing(object sender, EventArgs e)
			=> MessagingCenter.Send<object>(this, Messages.REFRESH);
	}
}
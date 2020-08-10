using Xamarin.Forms;

namespace BSMM2.Views {

	internal class WebPage : ContentPage {

		public WebPage(string url) {
			Content = new WebView { Source = url };
		}
	}
}
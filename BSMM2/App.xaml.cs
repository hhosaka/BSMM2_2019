using BSMM2.Models;
using BSMM2.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace BSMM2 {

	public partial class App : Application {
		private const string APPDATAFILE = "bsmmapp.json";

		public App() {
			InitializeComponent();
			MainPage = new MainPage(BSMMApp.Create(APPDATAFILE, false));// use true in case save data is broken.
		}

		protected override void OnStart() {
			// Handle when your app starts
		}

		protected override void OnSleep() {
			// Handle when your app sleeps
		}

		protected override void OnResume() {
			// Handle when your app resumes
		}
	}
}
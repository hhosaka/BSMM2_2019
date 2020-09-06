using BSMM2.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BSMM2.Views {

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoundsLogPage : TabbedPage {

		public RoundsLogPage(BSMMApp app) {
			InitializeComponent();
			for (int index = 0; index < app.Game.Rounds.Count(); ++index) {
				Children.Add(CreatePage(
					new RoundLogPage(app.Game, index),
					"Round" + (index + 1)));
			}

			Page CreatePage(Page page, string title)
				=> new NavigationPage(page) { Title = title , BarBackgroundColor=Color.ForestGreen};
		}
	}
}
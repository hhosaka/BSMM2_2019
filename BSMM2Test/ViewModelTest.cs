using BSMM2.Models;
using BSMM2.Models.Matches.SingleMatch;
using BSMM2.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BSMM2Test {

	[TestClass]
	public class ViewModelTest {
		private const string TESTFILE = "testfile.json";

		[TestMethod]
		public async Task ControlGameTest() {
			var app = BSMMApp.Create(TESTFILE, true);
			Assert.IsNotNull(app.Game);
			var viewModel = new PlayersViewModel(app, null);
			await viewModel.ExecuteRefresh();
			app.Add(new FakeGame(new SingleMatchRule(), 8), false);
			var id = app.Game.Id;
			await viewModel.ExecuteRefresh();

			MessagingCenter.Send<object>(app, Messages.REFRESH);

			var app2 = BSMMApp.Create(TESTFILE, false);
			Assert.AreEqual(id, app2.Game.Id);

			Assert.IsTrue(app2.Remove(app2.Game));
			await viewModel.ExecuteRefresh();

			MessagingCenter.Send<object>(app2, Messages.REFRESH);

			//			Assert.AreEqual(Guid.Empty, BSMMApp.Create().Game.Id);
			Assert.AreEqual(app2.Games.Last().Id, app2.Game.Id);
			Assert.IsFalse(app2.Remove(app2.Game));
		}
	}
}
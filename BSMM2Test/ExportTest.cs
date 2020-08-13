using BSMM2.Models;
using BSMM2.Models.Matches.SingleMatch;
using BSMM2.Resource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace BSMM2Test {

	[TestClass]
	public class ExportTest {

		[TestMethod]
		public void ExportPlayerTest() {
			var data = new Player(new SingleMatchRule(), "test").Export(new ExportData());
			Assert.AreEqual("test", data[AppResources.TextPlayerName]);
			Assert.AreEqual("False", data[AppResources.TextDropped].ToString());
			Assert.AreEqual(0, data[AppResources.TextMatchPoint]);
			Assert.AreEqual(0.0, data[AppResources.TextWinPoint]);
			Assert.AreEqual(0, data[AppResources.TextOpponentMatchPoint]);
			Assert.AreEqual(0.0, data[AppResources.TextOpponentWinPoint]);
		}

		[TestMethod]
		public void ExportPlayersTest1() {
			var game = new FakeGame(new SingleMatchRule(), 2);
			var buf = Util.Export(game.Players);
			Assert.AreEqual(
				AppResources.TextPlayerName + ", " +
				AppResources.TextDropped + ", " +
				AppResources.TextMatchPoint + ", " +
				AppResources.TextWinPoint + ", " +
				AppResources.TextOpponentMatchPoint + ", " +
				AppResources.TextOpponentWinPoint + ", " +
				AppResources.TextByeMatchCount + ", \r\n" +
				"\"Player001\", False, 0, 0, 0, 0, 0, \r\n" +
				"\"Player002\", False, 0, 0, 0, 0, 0, \r\n",
				buf);
		}

		[TestMethod]
		public void ExportPlayersTest2() {
			var title = AppResources.TextPlayerName + ", " +
				AppResources.TextDropped + ", " +
				AppResources.TextMatchPoint + ", " +
				AppResources.TextWinPoint + ", " +
				AppResources.TextOpponentMatchPoint + ", " +
				AppResources.TextOpponentWinPoint + ", " +
				AppResources.TextByeMatchCount + ", \r\n";

			var game = new FakeGame(new SingleMatchRule(), 2);

			game.StepToPlaying();
			Util.SetResult(game, 0, RESULT_T.Win);

			Assert.AreEqual(
				title +
				"\"Player001\", False, 3, 1, 0, 0, 0, \r\n" +
				"\"Player002\", False, 0, 0, 3, 1, 0, \r\n",
				Util.Export(game.Players));

			game.Players.Source.ElementAt(0).Dropped = true;

			Assert.AreEqual(
				title +
				"\"Player001\", True, 3, 1, 0, 0, 0, \r\n" +
				"\"Player002\", False, 0, 0, 3, 1, 0, \r\n",
				Util.Export(game.Players));

			game.Players.Source.ElementAt(0).Dropped = false;
			Util.SetResult(game, 0, RESULT_T.Draw);
			Assert.AreEqual(
				title +
				"\"Player001\", False, 1, 0.5, 1, 0.5, 0, \r\n" +
				"\"Player002\", False, 1, 0.5, 1, 0.5, 0, \r\n",
				Util.Export(game.Players));
		}

		[TestMethod]
		public void ExportPlayersTest3() {
			var game = new FakeGame(new SingleMatchRule(true), 2);
			var buf = Util.Export(game.Players);
			Assert.AreEqual(
				AppResources.TextPlayerName + ", " +
				AppResources.TextDropped + ", " +
				AppResources.TextMatchPoint + ", " +
				AppResources.TextWinPoint + ", " +
				AppResources.TextLifePoint + ", " +
				AppResources.TextOpponentMatchPoint + ", " +
				AppResources.TextOpponentWinPoint + ", " +
				AppResources.TextOpponentLifePoint + ", " +
				AppResources.TextByeMatchCount + ", \r\n" +
			"\"Player001\", False, 0, 0, 0, 0, 0, 0, 0, \r\n" +
				"\"Player002\", False, 0, 0, 0, 0, 0, 0, 0, \r\n",
				buf);
		}

		[TestMethod]
		public void DescriptionPlayerTest() {
			var game = new FakeGame(new SingleMatchRule(true), 4);

			game.Shuffle();

			Assert.AreEqual(
				AppResources.TextMatchPoint + " = 0/ " +
				AppResources.TextWinPoint + " = 0.00/ " +
				AppResources.TextLifePoint + " = 0",
				game.Players.GetSortedSource().ElementAt(0).Description);
		}

		[TestMethod]
		public void DescriptionPlayerTest2() {
			var game = new FakeGame(new SingleMatchRule(), 4);

			game.Shuffle();

			Assert.AreEqual(
				AppResources.TextMatchPoint + " = 0/ " +
				AppResources.TextWinPoint + " = 0.00",
				game.Players.GetSortedSource().ElementAt(0).Description);
		}
	}
}
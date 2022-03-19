using BSMM2.Models;
using BSMM2.Models.Matches.SingleMatch;
using BSMM2.Resource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Linq;
using System.Text;
using static BSMM2.Models.RESULT_T;

namespace BSMM2Test {

	[TestClass]
	public class ExportTest {

		[TestMethod]
		public void ExportPlayerTest() {
			var game = new FakeGame(new SingleMatchRule(), 2);
			var data = game.Players.Source.First().Export(new ExportData());
			Assert.AreEqual("Player001", data[AppResources.TextPlayerName]);
			Assert.AreEqual("False", data[AppResources.TextDropped].ToString());
			Assert.AreEqual(0, data[AppResources.TextMatchPoint]);
			Assert.AreEqual(0.0, data[AppResources.TextWinPoint]);
			Assert.AreEqual(0, data[AppResources.TextOpponentMatchPoint]);
			Assert.AreEqual(0.0, data[AppResources.TextOpponentWinPoint]);
		}

		[TestMethod]
		public void ExportPlayersTest1() {
			var game = new FakeGame(new SingleMatchRule(), 2);
			var buf = Util.Export(game.Rule, game.Players);
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
				Util.Export(game.Rule, game.Players));

			game.Players.Source.ElementAt(0).Dropped = true;

			Assert.AreEqual(
				title +
				"\"Player001\", True, 3, 1, 0, 0, 0, \r\n" +
				"\"Player002\", False, 0, 0, 3, 1, 0, \r\n",
				Util.Export(game.Rule, game.Players));

			game.Players.Source.ElementAt(0).Dropped = false;
			Util.SetResult(game, 0, RESULT_T.Draw);
			Assert.AreEqual(
				title +
				"\"Player001\", False, 1, 0.5, 1, 0.5, 0, \r\n" +
				"\"Player002\", False, 1, 0.5, 1, 0.5, 0, \r\n",
				Util.Export(game.Rule, game.Players));
		}

		[TestMethod]
		public void ExportPlayersTest3() {
			var game = new FakeGame(new SingleMatchRule(true), 2);
			var buf = Util.Export(game.Rule, game.Players);
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

			var point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.AreEqual(0, point.LifePoint);
		}

		[TestMethod]
		public void DescriptionPlayerTest2() {
			var game = new FakeGame(new SingleMatchRule(), 4);

			var point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.IsNull(point.LifePoint);
		}

		[TestMethod]
		public void DescriptionPlayerTest3() {
			var game = new FakeGame(new SingleMatchRule(), 4);

			game.StepToPlaying();

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);

			var point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(3, point.MatchPoint);
			Assert.AreEqual(1.0, point.WinPoint);
			Assert.IsNull(point.LifePoint);

			point = game.GetSortedSource().ElementAt(2).Point;

			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.IsNull(point.LifePoint);

		}

		[TestMethod]
		public void ExportPlayerTest1() {
			var game = new FakeGame(new SingleMatchRule(), 4);


			var players = game.Rule.GetExporter().ExportPlayers(game);

			var buf = new StringBuilder();
			players.ExportTitle(new StringWriter(buf));
			Assert.AreEqual(
				OrderedPlayer.TITLE_ORDER + "," +
				Player.TITLE_NAME + "," + Player.TITLE_DROPPED + "," +
				SingleMatchResult.TITLE_MATCH_POINT + "," + SingleMatchResult.TITLE_WIN_POINT + "," +
				Player.TITLE_ORIGIN_OPPONENT + SingleMatchResult.TITLE_MATCH_POINT + "," + Player.TITLE_ORIGIN_OPPONENT + SingleMatchResult.TITLE_WIN_POINT + "," +
				Player.TITLE_BYE_MATCH_COUNT + ",",
				buf.ToString());
			buf.Clear();
			players.ExportData(new StringWriter(buf));
			Assert.AreEqual("1,\"Player001\",False,0,0,0,0,0,\r\n"+
							"1,\"Player002\",False,0,0,0,0,0,\r\n" +
							"1,\"Player003\",False,0,0,0,0,0,\r\n" +
							"1,\"Player004\",False,0,0,0,0,0,\r\n",
							buf.ToString());



			new Serializer<object>().Serialize(new StringWriter(buf), players);

			var str = buf.ToString();
		}
	}
}
using BSMM2.Models;
using BSMM2.Models.Matches.SingleMatch;
using BSMM2.Resource;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static BSMM2.Models.RESULT_T;

namespace BSMM2Test {

	[TestClass]
	public class ExportTest {

		[TestMethod]
		public void ExportPlayersTest1() {
			var title = OrderedPlayer.TITLE_ORDER + "," +
				Player.TITLE_NAME + "," + Player.TITLE_DROPPED + "," +
				SingleMatchResult.TITLE_MATCH_POINT + "," + SingleMatchResult.TITLE_WIN_POINT + "," +
				Player.TITLE_ORIGIN_OPPONENT + SingleMatchResult.TITLE_MATCH_POINT + "," + Player.TITLE_ORIGIN_OPPONENT + SingleMatchResult.TITLE_WIN_POINT + "," +
				Player.TITLE_BYE_MATCH_COUNT + "," + "\r\n";
			var game = new FakeGame(new SingleMatchRule(), 2);
			var buf = new StringBuilder();
			var players = game.Players;

			Export();
			Assert.AreEqual(title +
				"1,\"Player001\",False,0,0,0,0,0,\r\n" +
				"1,\"Player002\",False,0,0,0,0,0,\r\n",
				buf.ToString());

			Export();
			Assert.AreEqual(title + 
				"1,\"Player001\",False,0,0,0,0,0,\r\n" +
				"1,\"Player002\",False,0,0,0,0,0,\r\n",
				buf.ToString());

			game.StepToPlaying();
			Util.SetResult(game, 0, RESULT_T.Win);

			Export();
			Assert.AreEqual(title +
				"1,\"Player001\",False,3,1,0,0,0,\r\n" +
				"2,\"Player002\",False,0,0,3,1,0,\r\n",
				buf.ToString());

			game.Players.Source.ElementAt(0).Dropped = true;

			Export();
			Assert.AreEqual(title +
				"1,\"Player002\",False,0,0,3,1,0,\r\n" +
				"2,\"Player001\",True,3,1,0,0,0,\r\n" ,
				buf.ToString());

			game.Players.Source.ElementAt(0).Dropped = false;
			Util.SetResult(game, 0, RESULT_T.Draw);

			Export();
			Assert.AreEqual(title +
				"1,\"Player001\",False,1,0.5,1,0.5,0,\r\n" +
				"1,\"Player002\",False,1,0.5,1,0.5,0,\r\n",
				buf.ToString());

			void Export() {
				buf.Clear();
				CSVConverter.Convert(players.Export(new ExportSource()), new StringWriter(buf));
			}
		}

		[TestMethod]
		public void PlayerPointTest() {
			var game = new FakeGame(new SingleMatchRule(PointRule.EnableLP), 4);

			var point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.AreEqual(0, point.LifePoint);

			game.StepToPlaying();
			Util.SetResult(game, 0, RESULT_T.Win);
			point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(3, point.MatchPoint);
			Assert.AreEqual(1, point.WinPoint);
			Assert.AreEqual(5, point.LifePoint);

			point = game.GetSortedSource().ElementAt(1).Point;
			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.AreEqual(5, point.LifePoint);
		}

		[TestMethod]
		public void PlayerPointTest2() {
			var game = new FakeGame(new SingleMatchRule(), 4);

			var point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.IsNull(point.LifePoint);

			game.StepToPlaying();
			Util.SetResult(game, 0, RESULT_T.Win);
			point = game.GetSortedSource().ElementAt(0).Point;

			Assert.AreEqual(3, point.MatchPoint);
			Assert.AreEqual(1, point.WinPoint);
			Assert.IsNull(point.LifePoint);

			point = game.GetSortedSource().ElementAt(1).Point;
			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.IsNull(point.LifePoint);

			point = game.GetSortedSource().ElementAt(2).Point;
			Assert.AreEqual(0, point.MatchPoint);
			Assert.AreEqual(0, point.WinPoint);
			Assert.IsNull(point.LifePoint);
		}
	}
}
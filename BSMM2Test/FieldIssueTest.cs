using System;
using System.Collections.Generic;
using System.Text;
using BSMM2.Models;
using BSMM2.Models.Matches.MultiMatch.NthGameMatch;
using BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch;
using BSMM2.Models.Matches.SingleMatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BSMM2.Models.RESULT_T;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace BSMM2Test
{
	[TestClass]
	public class FieldIssueTest {
		[TestMethod]
		public void DeresukeIssue1Test() {
			var game = new FakeGame(new SingleMatchRule(), 6);
			game.AcceptByeMatchDuplication = true;
			game.Rule.Comparers.First(c => c is ByeMatchComparer).Active = false;
			game.Rule.Comparers.First(c => c is OpponentMatchPointComparer).Active = false;
			game.Rule.Comparers.First(c => c is OpponentWinPointComparer).Active = false;
			game.Rule.Comparers.First(c => c is WinPointComparer).Active = false;

			Util.CreateSingleMatchRound(game, new[] { 4, 6, 5, 2, 3, 1 });
			game.StepToMatching();
			Util.CreateSingleMatchRound(game, new[] { 4, 3, 5, 6, 1, 2 });
			game.RandomizePlayer = Game.DefaultRandomizer;

			Assert.AreEqual(game.Players.Count, 6);
			Assert.IsFalse(game.StepToMatching());
			game.AcceptGapMatchDuplication = true;
			Assert.IsTrue(game.StepToMatching());
		}

		[TestMethod]
		public void DeresukeIssue2Test() {
			var game = new FakeGame(new SingleMatchRule(), 6);
			game.AcceptByeMatchDuplication = true;
			game.Rule.Comparers.First(c => c is ByeMatchComparer).Active = false;
			game.Rule.Comparers.First(c => c is OpponentMatchPointComparer).Active = false;
			game.Rule.Comparers.First(c => c is OpponentWinPointComparer).Active = false;
			game.Rule.Comparers.First(c => c is WinPointComparer).Active = false;

			Util.CreateSingleMatchRound(game, new[] { 4, 6, 5, 2, 3, 1 });
			game.StepToMatching();
			Util.CreateSingleMatchRound(game, new[] { 4, 3, 5, 6, 1, 2 });
			game.RandomizePlayer = Game.DefaultRandomizer;

			Assert.AreEqual(game.Players.Count, 6);
			Assert.IsFalse(game.StepToMatching());
			game.AcceptLosersGapMatchDuplication = true;
			Assert.IsTrue(game.StepToMatching());
		}

		[TestMethod]
		public void DeresukeIssue3Test() {
			var rule = new SingleMatchRule();
			var game = new FakeGame(rule, 14);

			Util.CreateSingleMatchRound(game, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 });
			game.StepToMatching();
			Util.CreateSingleMatchRound(game, new[] { 5, 3, 13, 7, 9, 11, 1, 10, 6, 12, 14, 8, 2, 4 });
			game.StepToMatching();
			Util.CreateSingleMatchRound(game, new[] { 5, 13, 9, 1, 2, 6, 14, 7, 11, 3, 10, 4, 8, 12 });
			game.StepToMatching();
			Util.CreateSingleMatchRound(game, new[] { 9, 5, 13, 1, 11, 14, 10, 6, 2, 3, 12, 7, 8, 4 });
			game.StepToMatching();

			Util.CheckWithOrder(game.Rule, new[] { 9, 5, 13, 11, 2, 1, 14, 10, 8, 3, 6, 7, 12, 4 }, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14 }, game.Players.GetOrderedPlayers());
		}
	}
}

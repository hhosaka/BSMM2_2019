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
	}
}

using BSMM2.Models;
using BSMM2.Models.Matches.MultiMatch;
using BSMM2.Models.Matches.MultiMatch.NthGameMatch;
using BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch;
using BSMM2.Models.Matches.SingleMatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Xamarin.Forms.Internals;
using static BSMM2.Models.RESULT_T;

namespace BSMM2Test {

	[TestClass]
	public class BSMM2Test {

		[TestMethod]
		public void RuleTest() {
			var rule = new SingleMatchRule();
			var players = new Players(rule, 4).GetSortedSource().ToArray();// new[] { new Player("player1"), new Player("player2"), new Player("player3"), new Player("player4") };
			var matches = new[] { new SingleMatch(rule, players[0], players[1]), new SingleMatch(rule, players[2], players[3]) };

			Assert.IsTrue(players.SequenceEqual(players.OrderByDescending(player => player, rule.GetComparer(true))));

			matches.ForEach(match => match.Commit());

			CollectionAssert.Equals(players, players.OrderByDescending(p => p, rule.GetComparer(true)));

			matches[0].SetResult(Win);
			matches[1].SetResult(Win);

			CollectionAssert.Equals(new[] { players[0], players[2], players[1], players[3] },
				players.OrderByDescending(p => p, rule.GetComparer(true)));

			matches[0].SetResult(Lose);
			matches[1].SetResult(Lose);

			CollectionAssert.Equals(new[] { players[1], players[3], players[0], players[2] },
				players.OrderByDescending(p => p, rule.GetComparer(true)));
		}

		[TestMethod]
		public void GameAddPlayerTest() {
			var rule = new SingleMatchRule();
			var game = new FakeGame(rule, 4);

			Util.CheckWithOrder(new[] { 1, 2, 3, 4 }, new[] { 1, 1, 1, 1 }, game.Players.GetSortedSource());

			game.Players.Add("Player006");
			Util.CheckWithOrder(new[] { 1, 2, 3, 4, 6 }, new[] { 1, 1, 1, 1, 1 }, game.Players.GetSortedSource());

			game.Players.Add("Player005");
			Util.CheckWithOrder(new[] { 1, 2, 3, 4, 6, 5 }, new[] { 1, 1, 1, 1, 1, 1 }, game.Players.GetSortedSource());

			game.Players.Remove(1);
			Util.CheckWithOrder(new[] { 1, 3, 4, 6, 5 }, new[] { 1, 1, 1, 1, 1 }, game.Players.GetSortedSource());

			game.Players.Add();
			Util.CheckWithOrder(new[] { 1, 3, 4, 6, 5, 6 }, new[] { 1, 1, 1, 1, 1, 1 }, game.Players.GetSortedSource());
		}

		[TestMethod]
		public void GameInitiateByListTest() {
			var buf = "\r\nPlayer001\r\nPlayer002\r\n\r\nPlayer003\r\nPlayer004";
			var rule = new SingleMatchRule();
			var game = new FakeGame(rule, new StringReader(buf));

			Util.CheckWithOrder(new[] { 1, 2, 3, 4 }, new[] { 1, 1, 1, 1 }, game.Players.GetSortedSource());

			game.Players.Add("Player006");
			Util.CheckWithOrder(new[] { 1, 2, 3, 4, 6 }, new[] { 1, 1, 1, 1, 1 }, game.Players.GetSortedSource());
		}

		[TestMethod]
		public void GameSequence1Test() {
			var rule = new SingleMatchRule();
			var game = new FakeGame(rule, 4);

			game.Shuffle();

			Assert.IsFalse(game.CanExecuteStepToMatching());

			Util.Check(new[] { 1, 2, 3, 4 }, game.ActiveRound);
			Util.CheckWithOrder(new[] { 1, 2, 3, 4 }, new[] { 1, 1, 1, 1 }, game.Players.GetSortedSource());

			game.StepToPlaying();

			Util.Check(new[] { 1, 2, 3, 4 }, game.ActiveRound);
			Util.CheckWithOrder(new[] { 1, 2, 3, 4 }, new[] { 1, 1, 1, 1 }, game.Players.GetSortedSource());

			Util.SetResult(game, 0, Win);

			Util.CheckWithOrder(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 3, 3 }, game.Players.GetSortedSource());

			Util.SetResult(game, 1, Win);
			Util.CheckWithOrder(new[] { 1, 3, 2, 4 }, new[] { 1, 1, 3, 3 }, game.Players.GetSortedSource());

			Util.SetResult(game, 0, Lose);
			Util.SetResult(game, 1, Lose);

			Util.CheckWithOrder(new[] { 2, 4, 1, 3 }, new[] { 1, 1, 3, 3 }, game.Players.GetSortedSource());

			game.StepToMatching();
			game.Players.GetSortedSource().ToArray()[0].Dropped = true;

			Util.CheckWithOrder(new[] { 4, 1, 3, 2 }, new[] { 1, 2, 2, 4 }, game.Players.GetSortedSource());

			game.Shuffle();

			Util.CheckWithOrder(new[] { 4, 1, 3, 2 }, new[] { 1, 2, 2, 4 }, game.Players.GetSortedSource());
			Util.Check(new[] { 4, 1, 3, -1 }, game.ActiveRound);
		}

		[TestMethod]
		public void GameSequence2Test() {
			var rule = new SingleMatchRule();
			var game = new FakeGame(rule, 4);

			// 初期設定確認
			Util.Check(new[] { 1, 2, 3, 4 }, game.ActiveRound);

			game.ActiveRound.Swap(0, 1);
			Util.Check(new[] { 3, 2, 1, 4 }, game.ActiveRound);

			//　シャッフルできる
			Assert.IsTrue(game.CanExecuteShuffle());
			game.Shuffle();
			Util.Check(new[] { 1, 2, 3, 4 }, game.ActiveRound);

			game.ActiveRound.Swap(Util.GetMatch(game, 0), Util.GetMatch(game, 1));
			Util.Check(new[] { 3, 2, 1, 4 }, game.ActiveRound);

			game.ActiveRound.Swap(0, 1);
			Util.Check(new[] { 1, 2, 3, 4 }, game.ActiveRound);

			game.Shuffle();
			Util.Check(new[] { 1, 2, 3, 4 }, game.ActiveRound);

			// 試合中にする
			game.StepToPlaying();
			Thread.Sleep(2);

			Assert.IsFalse(game.CanExecuteStepToMatching());

			Util.SetResult(game, 0, Win);

			Assert.IsFalse(game.CanExecuteStepToMatching());

			Util.SetResult(game, 1, Win);

			Assert.IsTrue(game.CanExecuteStepToMatching());

			game.StepToMatching();
			Util.CheckWithOrder(new[] { 1, 3, 2, 4 }, new[] { 1, 1, 3, 3 }, game.Players.GetSortedSource());
			Util.Check(new[] { 1, 3, 2, 4 }, game.ActiveRound);
			Assert.AreEqual(1, game.Rounds.Count());
			Util.Check(new[] { 1, 2, 3, 4 }, game.Rounds.First());

			game.StepToPlaying();

			Util.SetResult(game, 0, Lose);
			Util.SetResult(game, 1, Lose);

			Util.CheckWithOrder(new[] { 3, 1, 4, 2 }, new[] { 1, 2, 2, 4 }, game.Players.GetSortedSource());

			Assert.IsFalse(game.CanExecuteStepToMatching());
		}

		[TestMethod]
		public void GameSequence3Test() {
			var rule = new SingleMatchRule(true);
			var game = new FakeGame(rule, 4);

			game.Shuffle();
			game.StepToPlaying();

			Util.SetResult(game, 0, Win);
			(Util.GetMatch(game, 1) as SingleMatch).SetSingleMatchResult(Win, -1, -1);
			Assert.IsFalse(game.CanExecuteStepToMatching());

			Util.SetResult(game, 1, Win);

			Assert.IsTrue(game.CanExecuteStepToMatching());

			Util.CheckWithOrder(new[] { 1, 3, 2, 4 }, new[] { 1, 1, 3, 3 }, game.Players.GetSortedSource());

			(Util.GetMatch(game, 1) as SingleMatch).SetSingleMatchResult(Win, 5, 5);

			Assert.IsTrue(game.CanExecuteStepToMatching());

			Util.CheckWithOrder(new[] { 1, 3, 2, 4 }, new[] { 1, 1, 3, 3 }, game.Players.GetSortedSource());

			(Util.GetMatch(game, 0) as SingleMatch).SetSingleMatchResult(Win, 0, 0);

			Assert.IsTrue(game.CanExecuteStepToMatching());

			Util.CheckWithOrder(new[] { 3, 1, 4, 2 }, new[] { 1, 2, 3, 4 }, game.Players.GetSortedSource());
		}

		[TestMethod]
		public void CreateGameTest() {
			var rule = new SingleMatchRule();
			var game = new FakeGame(rule, 4);

			game.Shuffle();
			game.StepToPlaying();

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);

			Util.CheckWithOrder(new[] { 1, 3, 2, 4 }, new[] { 1, 1, 3, 3 }, game.Players.GetSortedSource());

			game.StepToMatching();
			game.StepToPlaying();

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);

			Util.CheckWithOrder(new[] { 1, 2, 3, 4 }, new[] { 1, 2, 2, 4 }, game.Players.GetSortedSource());
			Assert.AreEqual(1, game.Rounds.Count());

			var game2 = new FakeGame(rule, new Players(game.Players));
			Assert.AreEqual(0, game2.Rounds.Count());

			Util.Check(new[] { 1, 2, 3, 4 }, game2.ActiveRound);
		}

		[TestMethod]
		public void PlayerNameTest() {
			const string origin = "debug";
			var rule = new SingleMatchRule();
			rule.Prefix = origin;
			var game = new FakeGame( rule, 2);

			Assert.AreEqual(origin + "001", game.Players.GetSortedSource().ElementAt(0).Name);
			Assert.AreEqual(origin + "002", game.Players.GetSortedSource().ElementAt(1).Name);
		}

		[TestMethod]
		public void OrderTestSingleMatch()
			=> OrderTest(new SingleMatchRule());

		[TestMethod]
		public void OrderTestThreeGameMatch()
			=> OrderTest(new NthGameMatchRule(2));

		[TestMethod]
		public void OrderTestThreeOnThreeMatch()
			=> OrderTest(new NthGameMatchRule(2));

		[TestMethod]
		public void OrderTestSingleMatch3()
			=> Util.CheckWithOrder(new[] { 1, 5, 2, 6, 3, 7, 4, 8 }, new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, CreateGame(new SingleMatchRule(), 8, 3).Players.GetSortedSource());

		[TestMethod]
		public void OrderTestSingleMatch4()
			=> Util.CheckWithOrder(new[] { 1, 5, 2, 6, 3, 7, 4 }, new[] { 1, 2, 3, 4, 5, 6, 7 }, CreateGame(new SingleMatchRule(), 7, 3).Players.GetSortedSource());

		[TestMethod]
		public void ライフポイント検証() {
			var rule = new SingleMatchRule(true);
			var game = CreateGame(rule, 8, 2);
			var matches = game.ActiveRound;

			Util.Check(new[] { 1, 3, 5, 7, 2, 4, 6, 8 }, game.ActiveRound);
			Util.CheckWithOrder(new[] { 1, 5, 2, 3, 6, 7, 4, 8 }, new[] { 1, 1, 3, 3, 3, 3, 7, 7 }, game.Players.GetSortedSource());

			(Util.GetMatch(game, 0) as SingleMatch).SetSingleMatchResult(Win, 4, 5);

			Util.CheckWithOrder(new[] { 5, 1, 6, 7, 2, 3, 4, 8 }, new[] { 1, 2, 3, 3, 5, 5, 7, 7 }, game.Players.GetSortedSource());
		}

		[TestMethod]
		public void 勝利ポイント検証() {
			var rule = new NthGameMatchRule(2);
			var game = CreateGame(rule, 8, 2);
			var matches = game.ActiveRound;

			Util.Check(new[] { 1, 3, 5, 7, 2, 4, 6, 8 }, game.ActiveRound);
			Util.CheckWithOrder(new[] { 1, 5, 2, 3, 6, 7, 4, 8 }, new[] { 1, 1, 3, 3, 3, 3, 7, 7 }, game.Players.GetSortedSource());

			(matches.Matches.ElementAt(0) as MultiMatch).SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Util.CheckWithOrder(new[] { 5, 1, 3, 6, 7, 2, 4, 8 }, new[] { 1, 2, 3, 4, 4, 6, 7, 8 }, game.Players.GetSortedSource());
		}

		[TestMethod]
		public void ThreeOnThreeMatchStatusTest()
		{
			var game = new FakeGame(new ThreeOnThreeMatchRule(), 4);
			var match = game.ActiveRound.Matches.ElementAt(0) as MultiMatch;
			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Progress),
				new MultiMatch.Score(Win) });

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

		}

		//[TestMethod]
		//public void LifePointTest() {
		//	var game = new FakeGame(new SingleMatchRule(), 4);
		//	var match = game.ActiveRound.Matches.ElementAt(0) as SingleMatch;
		//	match.SetSingleMatchResult(Win);

		//	Assert.AreEqual(RESULT_T.Win, match.Record1.Result.RESULT);

		//	match.SetSingleMatchResult(Win,-1,-1);

		//	Assert.AreEqual(RESULT_T.Progress, match.Record1.Result.RESULT);

		//	match.SetSingleMatchResult(Win, -1);

		//	Assert.AreEqual(RESULT_T.Progress, match.Record1.Result.RESULT);

		//	match.SetSingleMatchResult(Win, 1,1);

		//	Assert.AreEqual(RESULT_T.Win, match.Record1.Result.RESULT);
		//}

		[TestMethod]
		public void ThreeGameMatchStatusTest()
		{
			var game = new FakeGame(new NthGameMatchRule(2), 4);
			var match = game.ActiveRound.Matches.ElementAt(0) as MultiMatch;
			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose)});

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Draw, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win)});

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Progress)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Progress)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);
		}

		[TestMethod]
		public void ThreeGameMatchWithLPStatusTest()
 		{
			var game = new FakeGame(new NthGameMatchRule(2, true), 4);
			var match = game.ActiveRound.Matches.ElementAt(0) as MultiMatch;
			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Draw, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Lose, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Progress),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose)});

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Draw, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win)});

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Progress)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Progress)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win,-1,-1)});

			//Assert.IsFalse(match.IsFinished);
			//Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win,0,-1)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

            match.SetMultiMatchResult(new[] {
                new MultiMatch.Score(Progress,1,0)});

            Assert.IsFalse(match.IsFinished);
            Assert.AreEqual(Progress, match.Record1.Result.RESULT);
            Assert.AreEqual(0, match.Record1.Result.LifePoint);

        }

		[TestMethod]
		public void FiveGameMatchWithLPStatusTest() {
			var game = new FakeGame(new NthGameMatchRule(3, true), 4);
			var match = game.ActiveRound.Matches.ElementAt(0) as MultiMatch;
			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose),
				new MultiMatch.Score(Win) });

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Lose)});

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Draw, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win)});

			Assert.IsTrue(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Progress)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win),
				new MultiMatch.Score(Progress)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win,-1,-1)});

			//Assert.IsFalse(match.IsFinished);
			//Assert.AreEqual(Progress, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Win,0,-1)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Win, match.Record1.Result.RESULT);

			match.SetMultiMatchResult(new[] {
				new MultiMatch.Score(Progress,1,0)});

			Assert.IsFalse(match.IsFinished);
			Assert.AreEqual(Progress, match.Record1.Result.RESULT);
			Assert.AreEqual(0, match.Record1.Result.LifePoint);

		}

		private void OrderTest(IRule rule) {
			OrderTest1(rule);
			OrderTest2(rule);
			OrderTestAcceptBye(rule);
		}

		//
		// 8人対戦
		//
		private void OrderTest1(IRule rule) {
			var game = new FakeGame(rule, 8);

			// 初期設定確認
			Util.Check(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, game.ActiveRound);

			game.StepToPlaying();

			// 対戦開始時
			Util.CheckWithOrder(new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, new[] { 1, 1, 1, 1, 1, 1, 1, 1 }, game.Players.GetSortedSource());

			// 3 win 4 lose
			Util.SetResult(game, 1, Win);

			Util.CheckWithOrder(new[] { 3, 4, 1, 2, 5, 6, 7, 8 }, new[] { 1, 2, 3, 3, 3, 3, 3, 3 }, game.Players.GetSortedSource());

			// 1 win 2 lose
			Util.SetResult(game, 0, Win);
			Util.CheckWithOrder(new[] { 1, 3, 2, 4, 5, 6, 7, 8 }, new[] { 1, 1, 3, 3, 5, 5, 5, 5 }, game.Players.GetSortedSource());

			// 5  6 draw
			Util.SetResult(game, 2, Draw);
			Util.CheckWithOrder(new[] { 1, 3, 5, 6, 2, 4, 7, 8 }, new[] { 1, 1, 3, 3, 5, 5, 7, 7 }, game.Players.GetSortedSource());

			// 8 win 7 lose
			Util.SetResult(game, 3, Lose);

			Util.CheckWithOrder(new[] { 1, 3, 8, 5, 6, 2, 4, 7 }, new[] { 1, 1, 1, 4, 4, 6, 6, 6 }, game.Players.GetSortedSource());

			// 7 win 8 lose
			Util.SetResult(game, 3, Win);
			Util.CheckWithOrder(new[] { 1, 3, 7, 5, 6, 2, 4, 8 }, new[] { 1, 1, 1, 4, 4, 6, 6, 6 }, game.Players.GetSortedSource());

			// 5 win 6 lose
			Util.SetResult(game, 2, Win);
			Util.CheckWithOrder(new[] { 1, 3, 5, 7, 2, 4, 6, 8 }, new[] { 1, 1, 1, 1, 5, 5, 5, 5 }, game.Players.GetSortedSource());

			// 2回戦目
			game.StepToMatching();
			game.StepToPlaying();

			// 7 win 8 lose
			Util.SetResult(game, 0, Lose);
			game.StepToMatching();//無効であることを確認
			Util.SetResult(game, 1, Win);
			game.StepToMatching();//無効であることを確認
			Util.SetResult(game, 2, Win);
			game.StepToMatching();//無効であることを確認
			Util.SetResult(game, 3, Win);
			Util.CheckWithOrder(new[] { 5, 3, 1, 6, 7, 2, 4, 8 }, new[] { 1, 2, 3, 4, 4, 6, 7, 8 }, game.Players.GetSortedSource());

			Util.SetResult(game, 0, Win);
			Util.CheckWithOrder(new[] { 1, 5, 2, 3, 6, 7, 4, 8 }, new[] { 1, 1, 3, 3, 3, 3, 7, 7 }, game.Players.GetSortedSource());

			// 3回戦目
			game.StepToMatching();
			game.StepToPlaying();

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);
			Util.SetResult(game, 3, Win);
			Util.CheckWithOrder(new[] { 1, 5, 2, 6, 3, 7, 4, 8 }, new[] { 1, 2, 3, 4, 5, 6, 7, 8 }, game.Players.GetSortedSource());
		}

		//
		// 7人対戦
		//
		private void OrderTest2(IRule rule) {
			var game = new FakeGame(rule, 7);

			// 初期設定確認
			Util.Check(new[] { 1, 2, 3, 4, 5, 6, 7, -1 }, game.ActiveRound);

			game.StepToPlaying();

			// 対戦開始時
			Util.CheckWithOrder(new[] { 7, 1, 2, 3, 4, 5, 6 }, new[] { 1, 2, 2, 2, 2, 2, 2 }, game.Players.GetSortedSource());

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);

			Util.CheckWithOrder(new[] { 1, 3, 5, 7, 2, 4, 6 }, new[] { 1, 1, 1, 4, 5, 5, 5 }, game.Players.GetSortedSource());

			game.StepToMatching();
			game.StepToPlaying();

			Util.Check(new[] { 1, 3, 5, 7, 2, 4, 6, -1 }, game.ActiveRound);

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);

			var buf = Util.Export(game);
			Util.CheckOrder(new[] { 1, 1, 3, 3, 5, 5, 7 },game.Players.GetSortedSource());
			Util.CheckWithOrder(new[] { 1, 5, 2, 3, 6, 7, 4 }, new[] { 1, 1, 3, 3, 5, 5, 7 }, game.Players.GetSortedSource());

			game.StepToMatching();
			game.StepToPlaying();

			Util.Check(new[] { 1, 5, 2, 3, 6, 7, 4, -1 }, game.ActiveRound);

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);

			var points = game.Players.GetSortedSource().Select(p => p.Point);
			var opponentPoints = game.Players.GetSortedSource().Select(p => p.OpponentPoint);

			Util.CheckWithOrder(new[] { 1, 5, 2, 6, 3, 7, 4 }, new[] { 1, 2, 3, 4, 5, 6, 7 }, game.Players.GetSortedSource());
		}

		//
		// 3回戦目に全敗がいなくなるケース
		//
		private void OrderTestAcceptBye(IRule rule) {
			var game = new FakeGame(rule, 11);

			// 初期設定確認
			Util.Check(new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, -1 }, game.ActiveRound);

			game.StepToPlaying();

			// 対戦開始時
			Util.CheckWithOrder(new[] { 11, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 }, new[] { 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 }, game.Players.GetSortedSource());

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);
			Util.SetResult(game, 3, Win);
			Util.SetResult(game, 4, Win);

			Util.CheckWithOrder(new[] { 1, 3, 5, 7, 9, 11, 2, 4, 6, 8, 10 }, new[] { 1, 1, 1, 1, 1, 6, 7, 7, 7, 7, 7 }, game.Players.GetSortedSource());

			game.StepToMatching();
			game.StepToPlaying();

			Util.Check(new[] { 1, 3, 5, 7, 9, 11, 2, 4, 6, 8, 10, -1 }, game.ActiveRound);

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);
			Util.SetResult(game, 3, Win);
			Util.SetResult(game, 4, Win);

			Util.CheckWithOrder(new[] { 1, 5, 9, 2, 3, 6, 7, 10, 11, 4, 8 }, new[] { 1, 1, 1, 4, 4, 4, 4, 8, 8, 10, 10 }, game.Players.GetSortedSource());

			game.StepToMatching();
			game.StepToPlaying();

			Util.Check(new[] { 1, 5, 9, 2, 3, 6, 7, 10, 11, 4, 8, -1 }, game.ActiveRound);

			Util.SetResult(game, 0, Win);
			Util.SetResult(game, 1, Win);
			Util.SetResult(game, 2, Win);
			Util.SetResult(game, 3, Win);
			Util.SetResult(game, 4, Lose);

			Util.CheckWithOrder(new[] { 1, 9, 5, 3, 7, 2, 6, 10, 4, 11, 8 }, new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 }, game.Players.GetSortedSource());

			Assert.IsFalse(game.StepToMatching());
			game.AcceptByeMatchDuplication = true;
			Assert.IsTrue(game.StepToMatching());
			game.StepToPlaying();

			var points = game.Players.GetSortedSource().Select(p => p.Point);
			var opponentPoints = game.Players.GetSortedSource().Select(p => p.OpponentPoint);

			Util.Check(new[] { 1, 9, 5, 3, 7, 6, 2, 10, 4, 8, 11, -1 }, game.ActiveRound);
		}

		private Game CreateGame(IRule rule, int count, int round) {
			var game = new FakeGame(rule, count);

			for (int i = 0; i < round; ++i) {
				game.StepToMatching();
				game.StepToPlaying();
				game.ActiveRound.Matches.ForEach(m => m.SetResult(Win));
			}
			return game;
		}

		[TestMethod]
		public void CloneTest() {
			var rule = new SingleMatchRule();
			var a = rule.Clone() as SingleMatchRule;
			Assert.AreEqual(rule.Name, a.Name);
			Assert.AreEqual(rule.EnableLifePoint, a.EnableLifePoint);

			rule.EnableLifePoint = true;
			a = rule.Clone() as SingleMatchRule;
			Assert.AreEqual(rule.EnableLifePoint, a.EnableLifePoint);
			Assert.IsNotNull(a.Comparers);
		}

	}
}
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

namespace BSMM2Test
{
	[TestClass]
	public class SwitcherTest {

		[TestMethod]
		public void SwitcherTest1() {
			var game = new FakeGame(new SingleMatchRule(),8);
			{
				var switcher = new OmitDetailSwitcher();
				Assert.IsTrue(game.Rule.Comparers.Any(c => c.Level == LEVEL.Option && c.Active));
				Assert.IsTrue(switcher.IsTarget(game));
				switcher.Execute(game);
				Assert.IsFalse(game.Rule.Comparers.Any(c => c.Level == LEVEL.Option && c.Active));
			}

			{
				var switcher = new OmitLoosersGapMatchDuplication();
				Assert.IsFalse(game.AcceptLosersGapMatchDuplication);
				Assert.IsTrue(switcher.IsTarget(game));
				switcher.Execute(game);
				Assert.IsTrue(game.AcceptLosersGapMatchDuplication);
			}

			{
				var switcher = new OmitGapMatchDuplication();
				Assert.IsFalse(game.AcceptGapMatchDuplication);
				Assert.IsTrue(switcher.IsTarget(game));
				switcher.Execute(game);
				Assert.IsTrue(game.AcceptGapMatchDuplication);
			}

			{
				var switcher = new OmitByeMatchDuplication();
				Assert.IsFalse(game.AcceptByeMatchDuplication);
				Assert.IsTrue(switcher.IsTarget(game));
				switcher.Execute(game);
				Assert.IsTrue(game.AcceptByeMatchDuplication);
			}

			{
				var switcher = new ConciderOnlyWinner();
				Assert.IsTrue(game.Rule.Comparers.Any(c => c.Level == LEVEL.Required && c.Active));
				Assert.IsTrue(switcher.IsTarget(game));
				switcher.Execute(game);
				Assert.IsFalse(game.Rule.Comparers.Any(c => c.Level == LEVEL.Required && c.Active));
			}
		}

		private bool Check(Game game, ISwitcher switcher) {
			Assert.IsTrue(switcher.IsTarget(game));
			switcher.Execute(game);
			return game.StepToMatching();
		}

		[TestMethod]
		public void SwitcherTest2() {
			var game = new FakeGame(new SingleMatchRule(), 6);

			Util.CreateSingleMatchRound(game, new[] { 1, 2, 3, 4, 5, 6 });
			Assert.IsTrue(game.StepToMatching());
			Util.CreateSingleMatchRound(game, new[] { 1, 3, 5, 2, 4, 6 });
			game.Players.Swap(1, 5);//この順番でなければマッチングできない。通常は乱数でなんとかなる。

			Assert.IsFalse(game.StepToMatching());

			Assert.IsTrue(new OmitDetailSwitcher().IsTarget(game));
			new OmitDetailSwitcher().Execute(game);

			Assert.IsFalse(game.StepToMatching());

			Assert.IsTrue(new OmitLoosersGapMatchDuplication().IsTarget(game));
			new OmitLoosersGapMatchDuplication().Execute(game);

			Assert.IsTrue(game.StepToMatching());
		}

		[TestMethod]
		public void SwitcherTest3() {
			var game = new FakeGame(new SingleMatchRule(), 6);

			Util.CreateSingleMatchRound(game, new[] { 1, 2, 3, 4, 5, 6 });
			Assert.IsTrue(game.StepToMatching());
			Util.CreateSingleMatchRound(game, new[] { 1, 3, 5, 2, 4, 6 });

			Assert.IsFalse(game.StepToMatching());
			Assert.IsFalse(Check(game, new OmitDetailSwitcher()));
			Assert.IsFalse(Check(game, new OmitLoosersGapMatchDuplication()));
			Assert.IsFalse(Check(game, new OmitGapMatchDuplication()));
			Assert.IsFalse(Check(game, new OmitByeMatchDuplication()));
			Assert.IsFalse(Check(game, new ConciderOnlyWinner()));
		}

		[TestMethod]
		public void SwitcherTest4() {
			var game = new FakeGame(new SingleMatchRule(), 6);

			Util.CreateSingleMatchRound(game, new[] { 1, 2, 3, 4, 5, 6 });
			Assert.IsTrue(game.StepToMatching());
			Util.CreateSingleMatchRound(game, new[] { 1, 3, 5, 2, 4, 6 });

			var switcher = new Switcher();

			Assert.IsFalse(game.StepToMatching());

			var s = switcher.Get(game);
			Assert.IsTrue(s is OmitDetailSwitcher);
			s.Execute(game);

			s = switcher.Get(game);
			Assert.IsTrue(s is OmitLoosersGapMatchDuplication);
			s.Execute(game);

			s = switcher.Get(game);
			Assert.IsTrue(s is OmitGapMatchDuplication);
			s.Execute(game);

			s = switcher.Get(game);
			Assert.IsTrue(s is OmitByeMatchDuplication);
			s.Execute(game);

			s = switcher.Get(game);
			Assert.IsTrue(s is ConciderOnlyWinner);
			s.Execute(game);

			s = switcher.Get(game);
			Assert.IsNull(s);
		}
	}
}

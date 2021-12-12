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
			var data = "{\"$id\":\"1\",\"_rounds\":[{\"$id\":\"2\",\"_matches\":[{\"$id\":\"3\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$id\":\"4\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchRule, BSMM2\",\"_comparers\":{\"$type\":\"BSMM2.Models.IComparer[], BSMM2\",\"$values\":[{\"$id\":\"5\",\"$type\":\"BSMM2.Models.WinnerComparer, BSMM2\",\"Active\":true},{\"$id\":\"6\",\"$type\":\"BSMM2.Models.PointComparer, BSMM2\",\"Active\":true},{\"$id\":\"7\",\"$type\":\"BSMM2.Models.OpponentMatchPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"8\",\"$type\":\"BSMM2.Models.WinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"9\",\"$type\":\"BSMM2.Models.OpponentWinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"10\",\"$type\":\"BSMM2.Models.ByeMatchComparer, BSMM2\",\"Active\":true}]},\"EnableLifePoint\":false,\"Prefix\":\"Player\"},\"_records\":[{\"$id\":\"11\",\"Player\":{\"$id\":\"12\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"3\"},{\"$id\":\"13\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"14\",\"Player\":{\"$ref\":\"12\"},\"Result\":{\"$id\":\"15\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"16\",\"Player\":{\"$id\":\"17\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$id\":\"18\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"19\",\"Player\":{\"$ref\":\"17\"},\"Result\":{\"$id\":\"20\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"21\",\"Player\":{\"$id\":\"22\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"18\"},{\"$id\":\"23\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"24\",\"Player\":{\"$ref\":\"22\"},\"Result\":{\"$id\":\"25\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"26\",\"Player\":{\"$id\":\"27\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$id\":\"28\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"29\",\"Player\":{\"$id\":\"30\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"28\"},{\"$id\":\"31\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"32\",\"Player\":{\"$ref\":\"30\"},\"Result\":{\"$id\":\"33\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"34\",\"Player\":{\"$id\":\"35\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"3\"},{\"$ref\":\"31\"}],\"Name\":\"Player002\",\"Dropped\":false},\"Result\":{\"$id\":\"36\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":true}],\"Name\":\"Player005\",\"Dropped\":false},\"Result\":{\"$id\":\"37\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"38\",\"Player\":{\"$ref\":\"27\"},\"Result\":{\"$id\":\"39\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"23\"}],\"Name\":\"Player006\",\"Dropped\":false},\"Result\":{\"$id\":\"40\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"Player004\",\"Dropped\":false},\"Result\":{\"$id\":\"41\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"13\"}],\"Name\":\"Player003\",\"Dropped\":false},\"Result\":{\"$id\":\"42\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"Player001\",\"Dropped\":false},\"Result\":{\"$id\":\"43\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"44\",\"Player\":{\"$ref\":\"35\"},\"Result\":{\"$id\":\"45\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"18\"},{\"$ref\":\"28\"}],\"IsPlaying\":true}],\"AcceptByeMatchDuplication\":false,\"AcceptGapMatchDuplication\":false,\"AcceptLosersGapMatchDuplication\":false,\"Title\":\"2021/12/05 22:17:17\",\"Id\":\"5bd2c1fa-c6e8-4f98-9691-260fd24b601f\",\"Rule\":{\"$ref\":\"4\"},\"StartTime\":null,\"Players\":{\"$id\":\"46\",\"_rule\":{\"$ref\":\"4\"},\"_prefix\":null,\"_players\":[{\"$ref\":\"12\"},{\"$ref\":\"27\"},{\"$ref\":\"17\"},{\"$ref\":\"22\"},{\"$ref\":\"30\"},{\"$ref\":\"35\"}]},\"ActiveRound\":{\"$id\":\"47\",\"_matches\":[{\"$ref\":\"13\"},{\"$ref\":\"31\"},{\"$ref\":\"23\"}],\"IsPlaying\":true}}";
			var game = new Serializer<Game>().Deserialize(new StringReader(data));
			game.RandomizePlayer = p => p;

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
			var data = "{\"$id\":\"1\",\"_rounds\":[{\"$id\":\"2\",\"_matches\":[{\"$id\":\"3\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$id\":\"4\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchRule, BSMM2\",\"_comparers\":{\"$type\":\"BSMM2.Models.IComparer[], BSMM2\",\"$values\":[{\"$id\":\"5\",\"$type\":\"BSMM2.Models.WinnerComparer, BSMM2\",\"Active\":true},{\"$id\":\"6\",\"$type\":\"BSMM2.Models.PointComparer, BSMM2\",\"Active\":true},{\"$id\":\"7\",\"$type\":\"BSMM2.Models.OpponentMatchPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"8\",\"$type\":\"BSMM2.Models.WinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"9\",\"$type\":\"BSMM2.Models.OpponentWinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"10\",\"$type\":\"BSMM2.Models.ByeMatchComparer, BSMM2\",\"Active\":true}]},\"EnableLifePoint\":false,\"Prefix\":\"Player\"},\"_records\":[{\"$id\":\"11\",\"Player\":{\"$id\":\"12\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"3\"},{\"$id\":\"13\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"14\",\"Player\":{\"$ref\":\"12\"},\"Result\":{\"$id\":\"15\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"16\",\"Player\":{\"$id\":\"17\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$id\":\"18\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"19\",\"Player\":{\"$ref\":\"17\"},\"Result\":{\"$id\":\"20\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"21\",\"Player\":{\"$id\":\"22\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"18\"},{\"$id\":\"23\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"24\",\"Player\":{\"$ref\":\"22\"},\"Result\":{\"$id\":\"25\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"26\",\"Player\":{\"$id\":\"27\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$id\":\"28\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"29\",\"Player\":{\"$id\":\"30\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"28\"},{\"$id\":\"31\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"32\",\"Player\":{\"$ref\":\"30\"},\"Result\":{\"$id\":\"33\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"34\",\"Player\":{\"$id\":\"35\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"3\"},{\"$ref\":\"31\"}],\"Name\":\"Player002\",\"Dropped\":false},\"Result\":{\"$id\":\"36\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":true}],\"Name\":\"Player005\",\"Dropped\":false},\"Result\":{\"$id\":\"37\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"38\",\"Player\":{\"$ref\":\"27\"},\"Result\":{\"$id\":\"39\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"23\"}],\"Name\":\"Player006\",\"Dropped\":false},\"Result\":{\"$id\":\"40\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"Player004\",\"Dropped\":false},\"Result\":{\"$id\":\"41\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"13\"}],\"Name\":\"Player003\",\"Dropped\":false},\"Result\":{\"$id\":\"42\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"Player001\",\"Dropped\":false},\"Result\":{\"$id\":\"43\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"44\",\"Player\":{\"$ref\":\"35\"},\"Result\":{\"$id\":\"45\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"18\"},{\"$ref\":\"28\"}],\"IsPlaying\":true}],\"AcceptByeMatchDuplication\":false,\"AcceptGapMatchDuplication\":false,\"AcceptLosersGapMatchDuplication\":false,\"Title\":\"2021/12/05 22:29:23\",\"Id\":\"7038dd31-73bc-4e6d-b094-3c8f5e00fdf8\",\"Rule\":{\"$ref\":\"4\"},\"StartTime\":null,\"Players\":{\"$id\":\"46\",\"_rule\":{\"$ref\":\"4\"},\"_prefix\":null,\"_players\":[{\"$ref\":\"12\"},{\"$ref\":\"35\"},{\"$ref\":\"17\"},{\"$ref\":\"22\"},{\"$ref\":\"30\"},{\"$ref\":\"27\"}]},\"ActiveRound\":{\"$id\":\"47\",\"_matches\":[{\"$ref\":\"13\"},{\"$ref\":\"31\"},{\"$ref\":\"23\"}],\"IsPlaying\":true}}";
			var game = new Serializer<Game>().Deserialize(new StringReader(data));

			game.RandomizePlayer = p => p;
			Assert.IsFalse(game.StepToMatching());
			Assert.IsFalse(Check(game, new OmitDetailSwitcher()));
			Assert.IsFalse(Check(game, new OmitLoosersGapMatchDuplication()));
			Assert.IsFalse(Check(game, new OmitGapMatchDuplication()));
			Assert.IsFalse(Check(game, new OmitByeMatchDuplication()));
			Assert.IsFalse(Check(game, new ConciderOnlyWinner()));
		}

		[TestMethod]
		public void SwitcherTest4() {
			var data = "{\"$id\":\"1\",\"_rounds\":[{\"$id\":\"2\",\"_matches\":[{\"$id\":\"3\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$id\":\"4\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchRule, BSMM2\",\"_comparers\":{\"$type\":\"BSMM2.Models.IComparer[], BSMM2\",\"$values\":[{\"$id\":\"5\",\"$type\":\"BSMM2.Models.WinnerComparer, BSMM2\",\"Active\":true},{\"$id\":\"6\",\"$type\":\"BSMM2.Models.PointComparer, BSMM2\",\"Active\":true},{\"$id\":\"7\",\"$type\":\"BSMM2.Models.OpponentMatchPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"8\",\"$type\":\"BSMM2.Models.WinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"9\",\"$type\":\"BSMM2.Models.OpponentWinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"10\",\"$type\":\"BSMM2.Models.ByeMatchComparer, BSMM2\",\"Active\":true}]},\"EnableLifePoint\":false,\"Prefix\":\"Player\"},\"_records\":[{\"$id\":\"11\",\"Player\":{\"$id\":\"12\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"3\"},{\"$id\":\"13\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"14\",\"Player\":{\"$ref\":\"12\"},\"Result\":{\"$id\":\"15\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"16\",\"Player\":{\"$id\":\"17\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$id\":\"18\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"19\",\"Player\":{\"$ref\":\"17\"},\"Result\":{\"$id\":\"20\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"21\",\"Player\":{\"$id\":\"22\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"18\"},{\"$id\":\"23\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"24\",\"Player\":{\"$ref\":\"22\"},\"Result\":{\"$id\":\"25\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"26\",\"Player\":{\"$id\":\"27\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$id\":\"28\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"29\",\"Player\":{\"$id\":\"30\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"28\"},{\"$id\":\"31\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"32\",\"Player\":{\"$ref\":\"30\"},\"Result\":{\"$id\":\"33\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"34\",\"Player\":{\"$id\":\"35\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_matches\":[{\"$ref\":\"3\"},{\"$ref\":\"31\"}],\"Name\":\"Player002\",\"Dropped\":false},\"Result\":{\"$id\":\"36\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":true}],\"Name\":\"Player005\",\"Dropped\":false},\"Result\":{\"$id\":\"37\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"38\",\"Player\":{\"$ref\":\"27\"},\"Result\":{\"$id\":\"39\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"23\"}],\"Name\":\"Player006\",\"Dropped\":false},\"Result\":{\"$id\":\"40\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"Player004\",\"Dropped\":false},\"Result\":{\"$id\":\"41\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"13\"}],\"Name\":\"Player003\",\"Dropped\":false},\"Result\":{\"$id\":\"42\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"Player001\",\"Dropped\":false},\"Result\":{\"$id\":\"43\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":0}},{\"$id\":\"44\",\"Player\":{\"$ref\":\"35\"},\"Result\":{\"$id\":\"45\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":5.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"18\"},{\"$ref\":\"28\"}],\"IsPlaying\":true}],\"AcceptByeMatchDuplication\":false,\"AcceptGapMatchDuplication\":false,\"AcceptLosersGapMatchDuplication\":false,\"Title\":\"2021/12/05 22:29:23\",\"Id\":\"7038dd31-73bc-4e6d-b094-3c8f5e00fdf8\",\"Rule\":{\"$ref\":\"4\"},\"StartTime\":null,\"Players\":{\"$id\":\"46\",\"_rule\":{\"$ref\":\"4\"},\"_prefix\":null,\"_players\":[{\"$ref\":\"12\"},{\"$ref\":\"35\"},{\"$ref\":\"17\"},{\"$ref\":\"22\"},{\"$ref\":\"30\"},{\"$ref\":\"27\"}]},\"ActiveRound\":{\"$id\":\"47\",\"_matches\":[{\"$ref\":\"13\"},{\"$ref\":\"31\"},{\"$ref\":\"23\"}],\"IsPlaying\":true}}";
			var game = new Serializer<Game>().Deserialize(new StringReader(data));
			game.RandomizePlayer = p => p;
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

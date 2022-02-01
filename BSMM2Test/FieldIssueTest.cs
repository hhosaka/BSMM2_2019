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
	public class FieldIssueTest {

		static string deresukeIssue1= "{\"$id\":\"1\",\"_rounds\":[{\"$id\":\"2\",\"_matches\":[{\"$id\":\"3\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$id\":\"4\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchRule, BSMM2\",\"_comparers\":{\"$type\":\"BSMM2.Models.IComparer[], BSMM2\",\"$values\":[{\"$id\":\"5\",\"$type\":\"BSMM2.Models.PointComparer, BSMM2\",\"Active\":true},{\"$id\":\"6\",\"$type\":\"BSMM2.Models.OpponentMatchPointComparer, BSMM2\",\"Active\":false},{\"$id\":\"7\",\"$type\":\"BSMM2.Models.WinPointComparer, BSMM2\",\"Active\":false},{\"$id\":\"8\",\"$type\":\"BSMM2.Models.OpponentWinPointComparer, BSMM2\",\"Active\":false},{\"$id\":\"9\",\"$type\":\"BSMM2.Models.ByeMatchComparer, BSMM2\",\"Active\":false}]},\"EnableLifePoint\":false,\"Prefix\":\"Player\"},\"_records\":[{\"$id\":\"10\",\"Player\":{\"$id\":\"11\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$id\":\"12\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchRule, BSMM2\",\"_comparers\":{\"$type\":\"BSMM2.Models.IComparer[], BSMM2\",\"$values\":[{\"$id\":\"13\",\"$type\":\"BSMM2.Models.PointComparer, BSMM2\",\"Active\":true},{\"$id\":\"14\",\"$type\":\"BSMM2.Models.OpponentMatchPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"15\",\"$type\":\"BSMM2.Models.WinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"16\",\"$type\":\"BSMM2.Models.OpponentWinPointComparer, BSMM2\",\"Active\":true},{\"$id\":\"17\",\"$type\":\"BSMM2.Models.ByeMatchComparer, BSMM2\",\"Active\":true}]},\"EnableLifePoint\":false,\"Prefix\":\"Player\"},\"_matches\":[{\"$ref\":\"3\"},{\"$id\":\"18\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"19\",\"Player\":{\"$id\":\"20\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"12\"},\"_matches\":[{\"$id\":\"21\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"22\",\"Player\":{\"$ref\":\"20\"},\"Result\":{\"$id\":\"23\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":0}},{\"$id\":\"24\",\"Player\":{\"$id\":\"25\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"12\"},\"_matches\":[{\"$ref\":\"21\"},{\"$id\":\"26\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"27\",\"Player\":{\"$ref\":\"25\"},\"Result\":{\"$id\":\"28\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":1}},{\"$id\":\"29\",\"Player\":{\"$id\":\"30\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"12\"},\"_matches\":[{\"$id\":\"31\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"32\",\"Player\":{\"$id\":\"33\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"12\"},\"_matches\":[{\"$ref\":\"31\"},{\"$id\":\"34\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatch, BSMM2\",\"_rule\":{\"$ref\":\"4\"},\"_records\":[{\"$id\":\"35\",\"Player\":{\"$ref\":\"33\"},\"Result\":{\"$id\":\"36\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":1}},{\"$id\":\"37\",\"Player\":{\"$id\":\"38\",\"$type\":\"BSMM2.Models.Player, BSMM2\",\"_rule\":{\"$ref\":\"12\"},\"_matches\":[{\"$ref\":\"3\"},{\"$ref\":\"34\"}],\"Name\":\"SHOW\",\"Dropped\":false},\"Result\":{\"$id\":\"39\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":0}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"しろりく\",\"Dropped\":false},\"Result\":{\"$id\":\"40\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":0}},{\"$id\":\"41\",\"Player\":{\"$ref\":\"30\"},\"Result\":{\"$id\":\"42\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"26\"}],\"Name\":\"村上文緒\",\"Dropped\":false},\"Result\":{\"$id\":\"43\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":0}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false}],\"Name\":\"ヨトロ\",\"Dropped\":false},\"Result\":{\"$id\":\"44\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"18\"}],\"Name\":\"ゆうこう\",\"Dropped\":false},\"Result\":{\"$id\":\"45\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":0}},{\"$id\":\"46\",\"Player\":{\"$ref\":\"11\"},\"Result\":{\"$id\":\"47\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":1}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":true}],\"Name\":\"でれゴン\",\"Dropped\":false},\"Result\":{\"$id\":\"48\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":1}},{\"$id\":\"49\",\"Player\":{\"$ref\":\"38\"},\"Result\":{\"$id\":\"50\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchResult, BSMM2\",\"LifePoint\":0.0,\"RESULT\":0}}],\"Rule\":{\"$ref\":\"4\"},\"IsGapMatch\":false},{\"$ref\":\"21\"},{\"$ref\":\"31\"}],\"IsPlaying\":true}],\"AcceptByeMatchDuplication\":true,\"AcceptGapMatchDuplication\":false,\"Title\":\"Game2021/10/24 13:46:49\",\"Id\":\"cd8b4507-99eb-420a-964c-304dc0f1b269\",\"Rule\":{\"$id\":\"4\",\"$type\":\"BSMM2.Models.Matches.SingleMatch.SingleMatchRule, BSMM2\",\"_comparers\":{\"$type\":\"BSMM2.Models.IComparer[], BSMM2\",\"$values\":[{\"$id\":\"5\",\"$type\":\"BSMM2.Models.PointComparer, BSMM2\",\"Active\":true},{\"$id\":\"6\",\"$type\":\"BSMM2.Models.OpponentMatchPointComparer, BSMM2\",\"Active\":false},{\"$id\":\"7\",\"$type\":\"BSMM2.Models.WinPointComparer, BSMM2\",\"Active\":false},{\"$id\":\"8\",\"$type\":\"BSMM2.Models.OpponentWinPointComparer, BSMM2\",\"Active\":false},{\"$id\":\"9\",\"$type\":\"BSMM2.Models.ByeMatchComparer, BSMM2\",\"Active\":false}]},\"EnableLifePoint\":false,\"Prefix\":\"Player\"},\"StartTime\":\"2021-10-24T14:51:15.509673+09:00\",\"Players\":{\"$id\":\"51\",\"_rule\":{\"$ref\":\"12\"},\"_prefix\":null,\"_players\":[{\"$ref\":\"30\"},{\"$ref\":\"25\"},{\"$ref\":\"33\"},{\"$ref\":\"38\"},{\"$ref\":\"20\"},{\"$ref\":\"11\"}]},\"ActiveRound\":{\"$id\":\"52\",\"_matches\":[{\"$ref\":\"34\"},{\"$ref\":\"18\"},{\"$ref\":\"26\"}],\"IsPlaying\":true}}";
		[TestMethod]
		public void DeresukeIssue1Test() {
			var game = new Serializer<Game>().Deserialize(new StringReader(deresukeIssue1));

			Assert.AreEqual(game.Players.Count, 6);
			Assert.IsFalse(game.StepToMatching());
			game.AcceptGapMatchDuplication = true;
			Assert.IsTrue(game.StepToMatching());
		}

		[TestMethod]
		public void DeresukeIssue2Test() {
			var game = new Serializer<Game>().Deserialize(new StringReader(deresukeIssue1));

			Assert.AreEqual(game.Players.Count, 6);
			Assert.IsFalse(game.StepToMatching());
			game.AcceptLosersGapMatchDuplication = true;
			Assert.IsTrue(game.StepToMatching());
		}
	}
}

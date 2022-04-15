using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace BSMM2.Models.Matches.MultiMatch {

	[JsonObject]
	public abstract class MultiMatch : SingleMatch.SingleMatch {

		public class Score:IScore {
			public RESULT_T RESULT { get; }
			public int LifePoint1 { get; }
			public int LifePoint2 { get; }

			public Score(RESULT_T result, int lp1 = 0, int lp2 = 0) {
				RESULT = result;
				LifePoint1 = lp1;
				LifePoint2 = lp2;
			}
		}

		[JsonIgnore]
		protected abstract int MatchCount { get; }

		public MultiMatch() {
		}

		public MultiMatch(Player player1, Player player2)
			: base(player1, player2) {
		}

		public override void SetResult(IRule rule, RESULT_T result) {
			var scores = new List<Score>();
			for (int i = 0; i < MatchCount; ++i) {
				scores.Add(new Score(result));
			}
			SetMultiMatchResult(rule, scores);
		}

		protected abstract MultiMatchResult CreateResult(PointRule pointRule);

		public void SetMultiMatchResult(IRule rule, IEnumerable<IScore> scores) {
			var result1 = CreateResult(rule.PointRule);
			var result2 = CreateResult(rule.PointRule);
			foreach (var score in scores) {
				result1.Add(rule.PointRule, new SingleMatchResult(rule.PointRule, score.RESULT, score.LifePoint1));
				result2.Add(rule.PointRule, new SingleMatchResult(rule.PointRule, RESULTUtil.ToOpponents(score.RESULT), score.LifePoint2));
			}
			SetResults(rule, result1, result2);
		}
	}
}
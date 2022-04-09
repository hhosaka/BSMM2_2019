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

		public override void SetResult(RESULT_T result) {
			var scores = new List<Score>();
			for (int i = 0; i < MatchCount; ++i) {
				scores.Add(new Score(result));
			}
			SetMultiMatchResult(scores);
		}

		protected abstract MultiMatchResult CreateResult();

		public void SetMultiMatchResult(IEnumerable<IScore> scores) {
			var result1 = CreateResult();
			var result2 = CreateResult();
			foreach (var score in scores) {
				result1.Add(new SingleMatchResult(score.RESULT, score.LifePoint1));
				result2.Add(new SingleMatchResult(RESULTUtil.ToOpponents(score.RESULT), score.LifePoint2));
			}
			SetResults(result1, result2);
		}
	}
}
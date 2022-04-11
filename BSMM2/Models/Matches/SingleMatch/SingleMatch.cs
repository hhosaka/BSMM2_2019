using Newtonsoft.Json;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatch : Match {

		public SingleMatch() {
		}

		public SingleMatch(Player player1, Player player2)
			: base(player1, player2) {
		}

		public SingleMatch(Record record1, Record record2)
			: base(record1, record2) {

		}

		public override void SetResult(IRule rule, RESULT_T result)
			=> SetSingleMatchResult(rule, result, RESULTUtil.DEFAULT_LIFE_POINT, RESULTUtil.DEFAULT_LIFE_POINT);

		public void SetSingleMatchResult(IRule rule, RESULT_T result, int?lp1, int?lp2) {
			SetResults(
				rule,
				new SingleMatchResult(rule.PointRule, result, lp1),
				new SingleMatchResult(rule.PointRule, RESULTUtil.ToOpponents(result), lp2));
		}
	}
}
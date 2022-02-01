using Newtonsoft.Json;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatch : Match {

		public SingleMatch() {
		}

		public SingleMatch(int id, IPlayer player1, IPlayer player2)
			: base(id, player1, player2) {
		}

		public override void SetResult(RESULT_T result)
			=> SetSingleMatchResult(result, RESULTUtil.DEFAULT_LIFE_POINT, RESULTUtil.DEFAULT_LIFE_POINT);

		public void SetSingleMatchResult(RESULT_T result, int lp1, int lp2) {
			SetResults(
				new SingleMatchResult(result, lp1),
				new SingleMatchResult(RESULTUtil.ToOpponents(result), lp2));
		}
	}
}
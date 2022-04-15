using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static BSMM2.Models.RESULT_T;

namespace BSMM2.Models.Matches.MultiMatch {

	[JsonObject]
	public abstract class MultiMatchResult : IResult,IPoint {
		[JsonProperty]
		protected List<IResult> _results;

		[JsonProperty]
		public int MatchPoint { get; private set; }

		[JsonProperty]
		public double WinPoint { get; private set; }

		[JsonProperty]
		public double? LifePoint { get; private set; }

		[JsonProperty]
		public RESULT_T RESULT { get; private set; }

		private double?GetLifePoint()
			=> _results.Where(r=>r.IsFinished).DefaultIfEmpty().Average(p => p?.Point.LifePoint ??0);

		private double GetWinPoint(PointRule pointRule)
			=> _results.Sum(p => p.Point.WinPoint) / _results.Count();

		private int GetMatchPoint(PointRule pointRule) {
			return RESULT == Win ? pointRule.MatchPoint_Win : RESULT == Lose ? pointRule.MatchPoint_Lose : pointRule.MatchPoint_Draw;
		}

		public abstract bool IsFinished { get; }

		[JsonIgnore]
		internal IEnumerable<SingleMatchResult> Results => _results.Cast<SingleMatchResult>();

		public IPoint Point => this;

		public void Clear()
			=> _results.Clear();

		public void Add(PointRule pointRule, IResult result) {
			_results.Add(result);
			RESULT = GetResult();
			MatchPoint = GetMatchPoint(pointRule);
			WinPoint = GetWinPoint(pointRule);
			LifePoint = GetLifePoint();
		}

		private RESULT_T GetResult() {
			if (_results.Any()) {
				int result = 0;
				foreach (var r in _results) {
					switch (r.RESULT) {
						case Win:
							++result;
							break;
						case Lose:
							--result;
							break;
						case Progress:
							return RESULT_T.Progress;
					}
				}
				return result == 0 ? Draw : result > 0 ? Win : Lose;
			}
			return RESULT_T.Progress;
		}

		public MultiMatchResult() {
			_results = new List<IResult>();
		}
	}
}
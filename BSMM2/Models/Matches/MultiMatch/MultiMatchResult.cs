using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using static BSMM2.Models.RESULT_T;

namespace BSMM2.Models.Matches.MultiMatch {

	[JsonObject]
	public class MultiMatchResult : IResult {

		[JsonProperty]
		private List<IResult> _results;

		[JsonProperty]
		private int _minCount;

		[JsonIgnore]
		private RESULT_T _RESULT;

		[JsonIgnore]
		public double LifePoint
			=> _results.Where(r=>r.IsFinished).DefaultIfEmpty().Average(p => p?.LifePoint??0);

		[JsonIgnore]
		public double WinPoint
			=> _results.Sum(p => p.WinPoint) / _results.Count();

		[JsonIgnore]
		public RESULT_T RESULT
			=> _RESULT = GetResult();

		public int MatchPoint
			=> RESULT == Win ? 3 : RESULT == Lose ? 0 : 1;

		[JsonIgnore]
		public bool IsFinished
			=> _results.Count(result => result.IsFinished) >= _minCount;

		[JsonIgnore]
		internal IEnumerable<SingleMatchResult> Results => _results.Cast<SingleMatchResult>();

		public void Clear()
			=> _results.Clear();

		public void Add(IResult result)
			=> _results.Add(result);

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

		public IExportData Export(IExportData data)
			=> throw new System.NotImplementedException();

		public MultiMatchResult(int minCount) {
			_minCount = minCount;
			_results = new List<IResult>();
		}
	}
}
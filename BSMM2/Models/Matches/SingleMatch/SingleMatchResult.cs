using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatchResult : IResult {

		private class TheResult : IPoint {
			private bool _enableLifePoint;

			public int MatchPoint { get; }

			public double LifePoint { get; }

			public double WinPoint { get; }

			public IExportData Export(IExportData data) {
				data[AppResources.TextMatchPoint] = MatchPoint;
				data[AppResources.TextWinPoint] = WinPoint;
				if (_enableLifePoint) data[AppResources.TextLifePoint] = LifePoint;
				return data;
			}

			public TheResult() {
			}

			public TheResult(bool enableLifePoint, IEnumerable<IPoint> source) {
				_enableLifePoint = enableLifePoint;

				MatchPoint = source.Sum(p => p?.MatchPoint??0);
				WinPoint = source.DefaultIfEmpty().Average(p => p?.WinPoint??0);
				if (enableLifePoint)
					LifePoint = source.DefaultIfEmpty().Average(p => p?.LifePoint ?? 0);
			}
		}

		public static IPoint Total(bool enableLifePoint, IEnumerable<IPoint> points)
			=> new TheResult(enableLifePoint, points);

		public IExportData Export(IExportData data)
			=> throw new System.NotImplementedException();

		[JsonIgnore]
		public int MatchPoint
			=> RESULT == Models.RESULT_T.Win ? 3 : RESULT == Models.RESULT_T.Lose ? 0 : 1;

		[JsonIgnore]
		public double WinPoint
			=> RESULT == Models.RESULT_T.Win ? 1.0 : RESULT == Models.RESULT_T.Lose ? 0.0 : 0.5;

		[JsonProperty]
		public double LifePoint { get; set; }

		[JsonProperty]
		public RESULT_T RESULT { get; set; }

		[JsonIgnore]
		public bool IsFinished => RESULT != RESULT_T.Progress && (LifePoint >= 0) != false;

        public SingleMatchResult()
        {
        }

		public SingleMatchResult(RESULT_T result, int lifePoint) {
			RESULT = result;
			LifePoint = lifePoint;
		}
	}
}
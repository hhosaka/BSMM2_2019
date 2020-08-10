using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BSMM2.Models.Matches.SingleMatch {

	internal class SingleMatchResult : IResult {

		private class TheResult : IPoint {
			private bool _enableLifePoint;

			public int MatchPoint { get; }

			public int LifePoint { get; }

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
				LifePoint = 0;// for enable lifepoint
				foreach (var point in source) {
					if (point != null) {
						MatchPoint += point.MatchPoint;
						if (enableLifePoint) LifePoint += point.LifePoint;
						WinPoint += point.WinPoint;
					}
				}
				WinPoint = source.Any() ? WinPoint / source.Count() : 0.0;
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
		public int LifePoint { get; }

		[JsonProperty]
		public RESULT_T RESULT { get; }

		[JsonIgnore]
		public bool IsFinished => RESULT != RESULT_T.Progress && (LifePoint >= 0) != false;

		public SingleMatchResult(RESULT_T result, int lifePoint) {
			RESULT = result;
			LifePoint = lifePoint;
		}
	}
}
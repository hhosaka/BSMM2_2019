using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatchResult : IResult {

		private class TheResult : IPoint, IExportableObject {
			public const string TITLE_MATCH_POINT = "match_point";
			public const string TITLE_WIN_POINT = "win_point";
			public const string TITLE_LIFE_POINT = "life_point";

			private bool _enableLifePoint;

			public int MatchPoint { get; }

			public double?LifePoint { get; }

			public double WinPoint { get; }

			public ExportData Export(ExportData data) {
				data[AppResources.TextMatchPoint] = MatchPoint;
				data[AppResources.TextWinPoint] = WinPoint;
				if (_enableLifePoint) data[AppResources.TextLifePoint] = LifePoint;
				return data;
			}

			public bool ExportData(TextWriter writer) {
				writer.Write(MatchPoint);
				writer.Write(",");
				writer.Write(WinPoint);
				writer.Write(",");
				if (LifePoint != null) {
					writer.Write(LifePoint);
					writer.Write(",");
				}
				return true;
			}

			public bool ExportTitle(TextWriter writer) {
				writer.Write(TITLE_MATCH_POINT);
				writer.Write(",");
				writer.Write(TITLE_WIN_POINT);
				writer.Write(",");
				if (LifePoint != null) {
					writer.Write(TITLE_LIFE_POINT);
					writer.Write(",");
				}
				return true;
			}

			public TheResult() {
			}

			public TheResult(bool enableLifePoint, IEnumerable<IPoint> source) {
				_enableLifePoint = enableLifePoint;

				MatchPoint = source?.Sum(p => p?.MatchPoint??0)??0;
				WinPoint = source?.DefaultIfEmpty().Average(p => p?.WinPoint??0)??0;
				if (enableLifePoint)
					LifePoint = source.DefaultIfEmpty().Average(p => p?.LifePoint ?? 0);
			}
		}

		public static IPoint Total(bool enableLifePoint, IEnumerable<IPoint> points)
			=> new TheResult(enableLifePoint, points);

		public ExportData Export(ExportData data)
			=> throw new System.NotImplementedException();

		public bool ExportData(TextWriter writer) {
			throw new System.NotImplementedException();
		}

		public bool ExportTitle(TextWriter writer) {
			throw new System.NotImplementedException();
		}

		[JsonIgnore]
		public int MatchPoint
			=> RESULT == Models.RESULT_T.Win ? 3 : RESULT == Models.RESULT_T.Lose ? 0 : 1;

		[JsonIgnore]
		public double WinPoint
			=> RESULT == Models.RESULT_T.Win ? 1.0 : RESULT == Models.RESULT_T.Lose ? 0.0 : 0.5;

		[JsonProperty]
		public double?LifePoint { get; set; }

		[JsonProperty]
		public RESULT_T RESULT { get; set; }

		[JsonIgnore]
		public bool IsFinished => RESULT != RESULT_T.Progress && !(LifePoint < 0);

        public SingleMatchResult()
        {
        }

		public SingleMatchResult(RESULT_T result, int?lifePoint) {
			RESULT = result;
			LifePoint = lifePoint;
		}
	}
}
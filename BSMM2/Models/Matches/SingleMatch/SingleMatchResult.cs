using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatchResult : IResult, IPoint {
		public const string TITLE_MATCH_POINT = "matchpoint";
		public const string TITLE_WIN_POINT = "winpoint";
		public const string TITLE_LIFE_POINT = "lifepoint";

		private class TotalResult : IExportablePoint {

			public int MatchPoint { get; }

			public double?LifePoint { get; }

			public double WinPoint { get; }

			public ExportSource Export(ExportSource data, string origin="") {
				data.Add(origin + TITLE_MATCH_POINT,MatchPoint);
				data.Add(origin + TITLE_WIN_POINT, WinPoint);
				if(LifePoint!=null)data.Add(origin + TITLE_LIFE_POINT, MatchPoint);
				return data;
			}

			public TotalResult() {
			}

			public TotalResult(bool enableLifePoint, IEnumerable<IPoint> source) {
				MatchPoint = source?.Sum(p => p?.MatchPoint??0)??0;
				WinPoint = source?.DefaultIfEmpty().Average(p => p?.WinPoint??0)??0;
				if (enableLifePoint)
					LifePoint = source.DefaultIfEmpty().Average(p => p?.LifePoint ?? 0);
			}
		}

		public static IExportablePoint Total(bool enableLifePoint, IEnumerable<IPoint> points)
			=> new TotalResult(enableLifePoint, points);

		[JsonProperty]
		public int MatchPoint { get; private set; }

		[JsonProperty]
		public double WinPoint { get; private set; }

		[JsonProperty]
		public double?LifePoint { get; private set; }

		[JsonProperty]
		public RESULT_T RESULT { get; private set; }

		[JsonIgnore]
		public bool IsFinished => RESULT != RESULT_T.Progress && !(LifePoint < 0);


		[JsonIgnore]
		public IPoint Point => this;

		public int GetMatchPoint(PointRule pointRule)
			=> RESULT == Models.RESULT_T.Win ? pointRule.MatchPoint_Win
				: RESULT == Models.RESULT_T.Lose ? pointRule.MatchPoint_Lose
				: pointRule.MatchPoint_Draw;

		public double GetWinPoint(PointRule pointRule)
			=> RESULT == Models.RESULT_T.Win ? pointRule.WinPoint_Win
				: RESULT == Models.RESULT_T.Lose ? pointRule.WinPoint_Lose
				: pointRule.WinPoint_Draw;

		public SingleMatchResult()
        {
        }

		public SingleMatchResult(PointRule pointRule, RESULT_T result, int?lifePoint) {
			RESULT = result;
			MatchPoint = GetMatchPoint(pointRule);
			WinPoint = GetWinPoint(pointRule);
			LifePoint = lifePoint;
		}
	}
}
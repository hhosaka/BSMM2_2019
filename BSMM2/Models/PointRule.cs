using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models
{
	[JsonObject]
	public class PointRule
	{
		[JsonProperty]
		int MatchPoint_Win { get; }

		[JsonProperty]
		double WinPoint_Win { get; }

		[JsonProperty]
		int MatchPoint_Draw { get; }

		[JsonProperty]
		double WinPoint_Draw { get; }

		[JsonProperty]
		int MatchPoint_Lose { get; }

		[JsonProperty]
		double WinPoint_Lose { get; }

		[JsonProperty]
		bool EnableLifePoint { get; }

		private PointRule() { }

		public PointRule(
			bool enableLifePoint=false,
			int matchPoint_Win=3, double winPoint_Win=1,
			int matchPoint_Draw = 1, double winPoint_Draw = 1,
			int matchPoint_Lose = 0, double winPoint_Lose = 0
			) {
			EnableLifePoint = enableLifePoint;
			MatchPoint_Win = matchPoint_Win;
			WinPoint_Win = winPoint_Win;
			MatchPoint_Draw = matchPoint_Draw;
			WinPoint_Draw = winPoint_Draw;
			MatchPoint_Lose = matchPoint_Lose;
			WinPoint_Lose = winPoint_Lose;
		}
	}
}

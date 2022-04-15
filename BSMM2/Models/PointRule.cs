using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models
{
	[JsonObject]
	public class PointRule
	{
		public static readonly PointRule Default = new PointRule(false);
		public static readonly PointRule EnableLP = new PointRule(true);
		public static readonly PointRule Pokemon = new PointRule(false, 3, 1, 0, 0, 0, 0);

		[JsonProperty]
		public int MatchPoint_Win { get; set; }

		[JsonProperty]
		public double WinPoint_Win { get; set; }

		[JsonProperty]
		public int MatchPoint_Draw { get; set; }

		[JsonProperty]
		public double WinPoint_Draw { get; set; }

		[JsonProperty]
		public int MatchPoint_Lose { get; set; }

		[JsonProperty]
		public double WinPoint_Lose { get; set; }

		[JsonProperty]
		public bool EnableLifePoint { get; set; }

		private PointRule() { }

		public PointRule(
			bool enableLifePoint,
			int matchPoint_Win=3, double winPoint_Win=1,
			int matchPoint_Draw = 1, double winPoint_Draw = 0.5,
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

		public PointRule(PointRule other = null) : this(
			other?.EnableLifePoint??false,
			other?.MatchPoint_Win??3, other?.WinPoint_Win??1,
			other?.MatchPoint_Draw??1, other?.WinPoint_Draw??0.5,
			other?.MatchPoint_Lose??0, other?.WinPoint_Lose??0) {
		}

		public PointRule Reset() {
			EnableLifePoint = false;
			MatchPoint_Win = 3;
			WinPoint_Win = 1;
			MatchPoint_Draw = 1;
			WinPoint_Draw = 0.5;
			MatchPoint_Lose = 0;
			WinPoint_Lose = 0;
			return this;
		}
	}
}

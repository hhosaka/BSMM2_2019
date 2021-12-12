using BSMM2.Resource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BSMM2.Models {

	internal class CompUtil {

		public static int Comp2Factor(bool param1, bool param2) => param1 ? param2 ? 0 : -1 : param2 ? 1 : 0;
		public static int CompResult(double value) => (int)(value > 0.0 ? Math.Ceiling(value) : Math.Floor(value));
	}

	internal class TheComparer : Comparer<Player> {
		private IEnumerable<IComparer> _compareres;
		private bool _force;

		public TheComparer(IEnumerable<IComparer> compareres, bool force) {
			_compareres = compareres;
			_force = force;
		}

		public override int Compare(Player p1, Player p2) {
			if (p1 != p2) {
				var ret = CompUtil.Comp2Factor(p1.Dropped, p2.Dropped);
				if (ret == 0) {
					foreach (var c in _compareres.Where(c => _force || c.Active)) {
						ret = c.Compare(p1, p2);
						if (ret != 0) return ret;
					}
					return p1.GetResult(p2) ?? 0;
				}
				return ret;
			}
			return 0;
		}
	}

	public class ByeMatchComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelUseByePoint;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Option;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> p2.ByeMatchCount - p1.ByeMatchCount;
	}

	public class PointComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelPointCompare;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Required;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> p1.Point.MatchPoint - p2.Point.MatchPoint;
	}

	public class WinnerComparer : IComparer
	{

		[JsonIgnore]
		public string Label => AppResources.LabelWinnerCompare;

		[JsonIgnore]
		public bool Selectable => false;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Mandatory;

		public bool Active {
			get => true;
			set { }
		}

		public int Compare(Player p1, Player p2)
			=> (p1.IsAllWins?1:0) - (p2.IsAllWins?1:0);
	}

	[JsonObject]
	public class LifePointComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelUseLifePoint;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Option;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> CompUtil.CompResult(p1.Point.LifePoint - p2.Point.LifePoint);
	}

	[JsonObject]
	public class OpponentMatchPointComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelUseOpponentMatchPoint;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Option;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> p1.OpponentPoint.MatchPoint - p2.OpponentPoint.MatchPoint;
	}

	[JsonObject]
	public class OpponentLifePointComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelUseOpponentLifePoint;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Option;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> CompUtil.CompResult(p1.OpponentPoint.LifePoint - p2.OpponentPoint.LifePoint);
	}

	[JsonObject]
	public class WinPointComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelUseWinPoint;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Option;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> CompUtil.CompResult(p1.Point.WinPoint - p2.Point.WinPoint);
	}

	[JsonObject]
	public class OpponentWinPointComparer : IComparer {

		[JsonIgnore]
		public string Label => AppResources.LabelUseOpponentWinPoint;

		[JsonIgnore]
		public bool Selectable => true;

		[JsonIgnore]
		public LEVEL Level => LEVEL.Option;

		[JsonProperty]
		public bool Active { get; set; } = true;

		public int Compare(Player p1, Player p2)
			=> CompUtil.CompResult(p1.OpponentPoint.WinPoint - p2.OpponentPoint.WinPoint);
	}
}
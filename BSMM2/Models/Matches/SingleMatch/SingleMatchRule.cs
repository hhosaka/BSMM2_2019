using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatchRule : IRule {

		[JsonProperty]
		bool _enableLifePoint;
		public bool EnableLifePoint {
			get => _enableLifePoint;
			set {
				if (_comparers==null || _enableLifePoint != value) {
					_enableLifePoint = value;
					if (value) {
						_comparers = new IComparer[] {
							new WinnerComparer(),
							new PointComparer(),
							new LifePointComparer(),
							new OpponentMatchPointComparer(),
							new OpponentLifePointComparer(),
							new WinPointComparer(),
							new OpponentWinPointComparer(),
							new ByeMatchComparer(),
						};
					} else {
						_comparers = new IComparer[] {
							new WinnerComparer(),
							new PointComparer(),
							new OpponentMatchPointComparer(),
							new WinPointComparer(),
							new OpponentWinPointComparer(),
							new ByeMatchComparer(),
						};
					}
				}
			}

		}

		[JsonProperty]
		public string Prefix { get; set; }

		[JsonProperty]
		private IEnumerable<IComparer> _comparers;

		[JsonIgnore]
		public IEnumerable<IComparer> Comparers => _comparers;

		[JsonIgnore]
		public virtual string Name
			=> AppResources.ItemRuleSingleMatch;

		[JsonIgnore]
		public virtual string Description
			=> AppResources.DescriptionSingleMatch;

		public virtual ContentPage CreateMatchPage(Match match)
			=> EnableLifePoint ? (ContentPage)new SingleMatchPage(this, (SingleMatch)match) : (ContentPage)new SingleMatchSimplePage(this, (SingleMatch)match);

		public ContentPage CreateRulePage(Game game)
			=> new SingleMatchRulePage(game);

		public virtual IRule Clone()
			=> new SingleMatchRule(this);

		public virtual Match CreateMatch(int id, IPlayer player1, IPlayer player2)
			=> new SingleMatch(id, player1, player2);

		public IPoint Point(IEnumerable<IPoint> points)
			=> SingleMatchResult.Total(EnableLifePoint, points);

		public virtual Comparer<Player> GetComparer(bool force)
			=> new TheComparer(Comparers, force);

		public virtual string GetDescription(Player player) {
			var buf = new StringBuilder();
			buf.Append(AppResources.TextMatchPoint);
			buf.Append(" = ");
			buf.Append(player.Point.MatchPoint);
			buf.Append("/ ");
			buf.Append(AppResources.TextWinPoint);
			buf.Append(" = ");
			buf.Append(string.Format("{0:F}",player.Point.WinPoint));
			if (EnableLifePoint) {
				buf.Append("/ ");
				buf.Append(AppResources.TextLifePoint);
				buf.Append(" = ");
				buf.Append(player.Point.LifePoint >= 0 ? player.Point.LifePoint.ToString() : "-");
			}
			return buf.ToString();
		}

		private SingleMatchRule() {
		}

		public SingleMatchRule(bool enableLifePoint = false) {
			EnableLifePoint = enableLifePoint;
			Prefix = AppResources.PrefixPlayer;
		}

		protected SingleMatchRule(SingleMatchRule src) : this(src.EnableLifePoint) {
		}
	}
}
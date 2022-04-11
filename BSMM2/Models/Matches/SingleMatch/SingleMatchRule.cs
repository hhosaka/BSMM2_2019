using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.SingleMatch {

	[JsonObject]
	public class SingleMatchRule : IRule {

		//[JsonProperty]
		//private bool _enableLifePoint;

		//[JsonIgnore]
		//private bool EnableLifePoint {
		//	get => _enableLifePoint;
		//	set {
		//		if (_comparers==null || _enableLifePoint != value) {
		//			_enableLifePoint = value;
		//			if (value) {
		//				_comparers = new IComparer[] {
		//					new WinnerComparer(),
		//					new PointComparer(),
		//					new LifePointComparer(),
		//					new OpponentMatchPointComparer(),
		//					new OpponentLifePointComparer(),
		//					new WinPointComparer(),
		//					new OpponentWinPointComparer(),
		//					new ByeMatchComparer(),
		//				};
		//			} else {
		//				_comparers = new IComparer[] {
		//					new WinnerComparer(),
		//					new PointComparer(),
		//					new OpponentMatchPointComparer(),
		//					new WinPointComparer(),
		//					new OpponentWinPointComparer(),
		//					new ByeMatchComparer(),
		//				};
		//			}
		//		}
		//	}

		//}

		[JsonProperty]
		public string Prefix { get; set; }

		[JsonProperty]
		public IEnumerable<IComparer> Comparers { get; private set; }

		[JsonIgnore]
		public virtual string Name
			=> AppResources.ItemRuleSingleMatch;

		[JsonIgnore]
		public virtual string Description
			=> AppResources.DescriptionSingleMatch;

		public PointRule PointRule { get; private set; }

		public virtual ContentPage CreateMatchPage(Match match)
			=> PointRule.EnableLifePoint ? (ContentPage)new SingleMatchPage(this, (SingleMatch)match) : (ContentPage)new SingleMatchSimplePage(this, (SingleMatch)match);

		public ContentPage CreateRulePage(Game game)
			=> new SingleMatchRulePage(game);

		public virtual IRule Clone()
			=> new SingleMatchRule(this);

		public virtual Match CreateMatch(Player player1, Player player2)
			=> new SingleMatch(player1, player2);

		public IPoint Point(IEnumerable<IPoint> points)
			=> SingleMatchResult.Total(PointRule.EnableLifePoint, points);

		public virtual Comparer<Player> GetComparer( bool force)
			=> new TheComparer(Comparers, force);

		private IEnumerable<IComparer> CreateComparers(bool enableLifePoint) {
			if (enableLifePoint) {
				return new IComparer[] {
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
				return new IComparer[] {
							new WinnerComparer(),
							new PointComparer(),
							new OpponentMatchPointComparer(),
							new WinPointComparer(),
							new OpponentWinPointComparer(),
							new ByeMatchComparer(),
						};
			}
		}

		private SingleMatchRule() {
		}

		public SingleMatchRule(PointRule pointRule = null) {
			PointRule = new PointRule(pointRule);
			Comparers = CreateComparers(PointRule.EnableLifePoint);
			Prefix = AppResources.PrefixPlayer;
		}

		protected SingleMatchRule(SingleMatchRule src) : this(src.PointRule) {
		}
	}
}
using BSMM2.Resource;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.MultiMatch.ThreeGameMatch {

	[JsonObject]
	public class ThreeGameMatchRule : MultiMatchRule {

		[JsonIgnore]
		public override int MatchCount => 2;

		[JsonIgnore]
		public override int MinimumMatchCount => 0;

		[JsonIgnore]
		public override string Name => AppResources.ItemRuleThreeGameMatch;

		[JsonIgnore]
		public override string Description => AppResources.DescriptionTreeGameMatch;

		public override ContentPage CreateMatchPage(Match match) => new ThreeGameMatchPage(this, match);

		public override IRule Clone() => new ThreeGameMatchRule(this);

		public override Match CreateMatch(IPlayer player1, IPlayer player2)
			=> new MultiMatch(this, player1, player2);

		public ThreeGameMatchRule() {
		}

		private ThreeGameMatchRule(MultiMatchRule src) : base(src) {
		}
	}
}
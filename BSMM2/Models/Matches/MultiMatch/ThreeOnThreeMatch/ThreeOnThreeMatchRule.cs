using BSMM2.Resource;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch {

	[JsonObject]
	public class ThreeOnThreeMatchRule : MultiMatchRule {

		private class TheMatch : MultiMatch
		{
			[JsonIgnore]
			protected override int MatchCount => 3;

			class TheResult : MultiMatchResult
			{
				public override bool IsFinished => _results.Count(result => result.IsFinished) == 3;

				public TheResult(int drawPoint):base(drawPoint) {
				}
			}
			protected override MultiMatchResult CreateResult(int drawPoint)
				=> new TheResult(drawPoint);

			public TheMatch() {// For Serializer
			}

			public TheMatch(Player player1, Player player2) : base(player1, player2) { }
		}

		[JsonIgnore]
		public override string Name
			=> AppResources.ItemRuleThreeOnThreeMatch;

		[JsonIgnore]
		public override string Description
			=> AppResources.DescriptionThreeOnThreeMatch;

		public override ContentPage CreateMatchPage(Match match)
			=> new ThreeOnThreeMatchPage(this, (MultiMatch)match);

		public override IRule Clone()
			=> new ThreeOnThreeMatchRule(this);

		public override Match CreateMatch(Player player1, Player player2)
			=> new TheMatch(player1, player2);

		private ThreeOnThreeMatchRule() {
		}

		public ThreeOnThreeMatchRule(PointRule pointRule = null):base(pointRule)
		{
		}

		private ThreeOnThreeMatchRule(MultiMatchRule rule) : this(rule.PointRule) {
		}
	}
}
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
			}
			protected override MultiMatchResult CreateResult()
				=> new TheResult();

			public TheMatch(MultiMatchRule rule, IPlayer player1, IPlayer player2) : base(rule, player1, player2) { }
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

		public override Match CreateMatch(IPlayer player1, IPlayer player2)
			=> new TheMatch(this, player1, player2);

		private ThreeOnThreeMatchRule() {
		}

		public ThreeOnThreeMatchRule(bool enableLifePoint=false):base(enableLifePoint)
		{
		}

		private ThreeOnThreeMatchRule(MultiMatchRule rule) : base(rule) {
		}
	}
}
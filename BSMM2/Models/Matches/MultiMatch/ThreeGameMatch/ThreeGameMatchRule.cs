using BSMM2.Resource;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.MultiMatch.ThreeGameMatch {

	[JsonObject]
	public class ThreeGameMatchRule : MultiMatchRule {

		private class TheMatch : MultiMatch
		{
			[JsonIgnore]
			protected override int MatchCount => 1;

			class TheResult : MultiMatchResult
			{
				public override bool IsFinished => _results.Any() && !_results.Any(result => !result.IsFinished);
			}
			protected override MultiMatchResult CreateResult()
				=> new TheResult();

			public TheMatch(MultiMatchRule rule, IPlayer player1, IPlayer player2) : base(rule, player1, player2) { }
		}

		[JsonIgnore]
		public override string Name => AppResources.ItemRuleThreeGameMatch;

		[JsonIgnore]
		public override string Description => AppResources.DescriptionTreeGameMatch;

		public override ContentPage CreateMatchPage(Match match) => new ThreeGameMatchPage(this, match);

		public override IRule Clone() => new ThreeGameMatchRule(this);

		public override Match CreateMatch(IPlayer player1, IPlayer player2)
			=> new TheMatch(this, player1, player2);

		private ThreeGameMatchRule() {
		}

		public ThreeGameMatchRule(bool enableLifePoint = false) : base(enableLifePoint)
        {

        }

		private ThreeGameMatchRule(MultiMatchRule src) : base(src) {
		}
	}
}
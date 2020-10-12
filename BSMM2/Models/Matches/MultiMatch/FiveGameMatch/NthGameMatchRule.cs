using BSMM2.Resource;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.MultiMatch.FiveGameMatch {

	// It should be sub class of NthMatchRule
	[JsonObject]
	public class NthGameMatchRule : MultiMatchRule {

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

			public TheMatch() {// For Serializer
			}

			public TheMatch(MultiMatchRule rule, IPlayer player1, IPlayer player2) : base(rule, player1, player2) { }
		}

		[JsonIgnore]
		public override string Name => AppResources.ItemRuleFiveGameMatch;

		[JsonIgnore]
		public override string Description => AppResources.DescriptionFiveGameMatch;

		//public override ContentPage CreateMatchPage(Match match) => new FiveGameMatchPage(this, match);

		//public override IRule Clone() => new FiveGameMatchRule(this);

		public override Match CreateMatch(IPlayer player1, IPlayer player2)
			=> new TheMatch(this, player1, player2);

		private NthGameMatchRule() {
		}

		public NthGameMatchRule(bool enableLifePoint = false) : base(enableLifePoint)
        {
        }

		private NthGameMatchRule(MultiMatchRule src) : base(src) {
		}
	}
}
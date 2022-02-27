using BSMM2.Resource;
using Newtonsoft.Json;
using System.Linq;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.MultiMatch.NthGameMatch {

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

			public TheMatch(int id, Player player1, Player player2) : base(id, player1, player2) { }
		}

		[JsonProperty]
		public int Count { get; private set; }

		[JsonIgnore]
		public override string Name => AppResources.ItemRuleThreeGameMatch;

		[JsonIgnore]
		public override string Description => AppResources.DescriptionTreeGameMatch;

		public override ContentPage CreateMatchPage(Match match) => new NthGameMatchPage(this, match);

		public override IRule Clone() => new NthGameMatchRule(Count, this);

		public override Match CreateMatch(int id, Player player1, Player player2)
			=> new TheMatch(id, player1, player2);

		private NthGameMatchRule() {
		}

		public NthGameMatchRule(int count, bool enableLifePoint = false) : base(enableLifePoint)
        {
			Count = count;

        }

		private NthGameMatchRule(int count, MultiMatchRule src) : base(src) {
			Count = count;
		}
	}
}
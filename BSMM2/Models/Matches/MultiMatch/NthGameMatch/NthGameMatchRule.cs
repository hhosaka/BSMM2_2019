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
			protected override MultiMatchResult CreateResult(PointRule pointRule)
				=> new TheResult();

			public TheMatch() {// For Serializer
			}

			public TheMatch(Player player1, Player player2) : base(player1, player2) { }
		}

		[JsonProperty]
		public int Count { get; private set; }

		[JsonProperty]
		private string _title;
		[JsonIgnore]
		public override string Name => _title;

		[JsonIgnore]
		public override string Description => AppResources.DescriptionTreeGameMatch;

		public override ContentPage CreateMatchPage(Match match) => new NthGameMatchPage(this, match);

		public override IRule Clone() => new NthGameMatchRule(Count, this);

		public override Match CreateMatch(Player player1, Player player2)
			=> new TheMatch(player1, player2);

		private NthGameMatchRule() {
		}

		public NthGameMatchRule(int count, PointRule pointRule = null, string title="") : base(pointRule)
        {
			_title = title;
			Count = count;
        }

		private NthGameMatchRule(int count, MultiMatchRule src) : this(count, src.PointRule, src.Name) {
		}
	}
}
﻿using BSMM2.Resource;
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

				public TheResult(int drawPoint):base(drawPoint) { }
			}
			protected override MultiMatchResult CreateResult(int drawPoint)
				=> new TheResult(drawPoint);

			public TheMatch() {// For Serializer
			}

			public TheMatch(Player player1, Player player2) : base(player1, player2) { }
		}

		[JsonProperty]
		public int Count { get; private set; }

		[JsonIgnore]
		public override string Name => AppResources.ItemRuleThreeGameMatch;

		[JsonIgnore]
		public override string Description => AppResources.DescriptionTreeGameMatch;

		public override ContentPage CreateMatchPage(Match match) => new NthGameMatchPage(this, match);

		public override IRule Clone() => new NthGameMatchRule(Count, this);

		public override Match CreateMatch(Player player1, Player player2)
			=> new TheMatch(player1, player2);

		private NthGameMatchRule() {
		}

		public NthGameMatchRule(int count, bool enableLifePoint = false, int drawPoint=1) : base(enableLifePoint, drawPoint)
        {
			Count = count;

        }

		private NthGameMatchRule(int count, MultiMatchRule src) : base(src) {
			Count = count;
		}
	}
}
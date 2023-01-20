using BSMM2.Resource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	[JsonObject]
	public class Player:IExportableObject{

		public const string TITLE_NAME = "name";
		public const string TITLE_DROPPED = "dropped";
		public const string TITLE_BYE_MATCH_COUNT = "bymatchcount";
		public const string TITLE_ORIGIN_OPPONENT = "opponent_";

		[JsonProperty]
		public Guid Id { get; set; }

		[JsonProperty]
		public String Name { get; set; }

		[JsonProperty]
		public virtual bool Dropped { get; set; }

		[JsonIgnore]
		public int ByeMatchCount
			=> _matches.Count(match => match.IsByeMatch);

		[JsonProperty]
		private IRule _rule;

		[JsonProperty]
		private List<Match> _matches;

		[JsonIgnore]
		public IExportablePoint Point
			=> _rule?.Point(_matches.Where(match => match.IsFinished).Select(match => match.GetRecord(this).Result.Point));

		[JsonIgnore]
		public IExportablePoint OpponentPoint
			=> _rule?.Point(_matches.Where(match => match.IsFinished).Select(match => (match.GetOpponentRecord(this).Player as Player)?.Point));

		[JsonIgnore]
		public IExportablePoint OpponentOpponentPoint
			=> _rule?.Point(_matches.Where(match => match.IsFinished).Select(match => match.GetOpponentRecord(this).Player?.OpponentPoint));

		public void StepToPlaying(Match match)
			=>_matches.Add(match);

		public bool IsAllWins()
			=> _matches.Count() > 0 && !_matches.Any(match => match.GetRecord(this).Result.RESULT != RESULT_T.Win);

		public bool IsAllLoses()
			=> _matches.Count() > 0 && !_matches.Any(match => match.GetRecord(this).Result.RESULT != RESULT_T.Lose);

		public bool HasGapMatch()
			=> _matches.Any(match => match.IsGapMatch);

		public int? GetResult(Player opponent) {
			var result = _matches.FirstOrDefault(m => m.GetOpponentRecord(this).Player == opponent)?.GetRecord(this).Result.RESULT;
			switch (result) {
				case null:
					return null;

				case RESULT_T.Win:
					return 1;

				case RESULT_T.Lose:
					return -1;

				default:
					return 0;
			}
		}

		private readonly IDictionary<string, string> _dic = new Dictionary<string, string>() {
						{AppResources.TextMatchPoint,AppResources.TextOpponentMatchPoint },
						{AppResources.TextWinPoint,AppResources.TextOpponentWinPoint },
						{AppResources.TextLifePoint,AppResources.TextOpponentLifePoint }};

        public int CompareTo(Player obj)
			=> _rule.GetComparer(true).Compare(this, obj);

		public ExportSource Export(ExportSource src, string origin = "") {
			src.Add(TITLE_NAME, Name);
			src.Add(TITLE_DROPPED, Dropped);
			Point.Export(src, origin);
			OpponentPoint.Export(src, origin+"opponent_");
			src.Add(TITLE_BYE_MATCH_COUNT, ByeMatchCount);
			return src;
		}

		public Player() {// For Serializer
		}

		public Player(IRule rule, string name) {
			Id = Guid.NewGuid();
			_rule = rule;
			Name = name;
			_matches = new List<Match>();
		}
	}
}
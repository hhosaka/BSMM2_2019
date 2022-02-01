using BSMM2.Resource;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	[JsonObject]
	public class Player : IPlayer{

		[JsonProperty]
		public String Name { get; set; }

		[JsonProperty]
		public virtual bool Dropped { get; set; }

		[JsonProperty]
		private IList<Match> _matches;

		[JsonIgnore]
		public IEnumerable<Match> Matches => _matches;

		[JsonIgnore]
		public int ByeMatchCount
			=> _matches.Count(match => match.IsByeMatch);

		[JsonIgnore]
		public bool HasGapMatch
			=> _matches.Any(match => match.IsGapMatch);

		[JsonIgnore]
		public bool IsAllWins
			=> _matches.Count() > 0 && !_matches.Any(match => match.GetRecord(this).Result.RESULT != RESULT_T.Win);

		[JsonIgnore]
		public bool IsAllLoses
			=> _matches.Count() > 0 && !_matches.Any(match => match.GetRecord(this).Result.RESULT != RESULT_T.Lose);

		[JsonIgnore]
		public IPoint Point { get; private set; }

		[JsonIgnore]
		public IPoint OpponentPoint { get; private set; }

		public string Description(IRule rule)
			=> rule.GetDescription(this);

		public void Commit(Match match)
		{
			_matches.Add(match);
            if (match.IsByeMatch)
            {
				match.SetResult(RESULT_T.Win);
            }
		}

		public int? GetResult(Player player) {
			var result = _matches.FirstOrDefault(m => m.GetOpponentRecord(this).Player == player)?.GetRecord(this).Result.RESULT;
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

		internal void CalcPoint(IRule rule)
			=> Point = rule.Point(_matches.Where(match=>match.IsFinished).Select(match => match.GetRecord(this).Result));

		internal void CalcOpponentPoint(IRule rule)
			=> OpponentPoint = rule.Point(_matches.Where(match => match.IsFinished).Select(match => (match.GetOpponentRecord(this).Player as Player)?.Point));

		public Player() {// For Serializer
		}

		private readonly IDictionary<string, string> _dic = new Dictionary<string, string>() {
						{AppResources.TextMatchPoint,AppResources.TextOpponentMatchPoint },
						{AppResources.TextWinPoint,AppResources.TextOpponentWinPoint },
						{AppResources.TextLifePoint,AppResources.TextOpponentLifePoint }};

		public IExportData Export(IExportData data) {
			data[AppResources.TextPlayerName] = Name;
			data[AppResources.TextDropped] = Dropped;
			Point.Export(data);
			OpponentPoint.Export(new ExportData()).ForEach(pair => data[_dic[pair.Key]] = pair.Value);
			data[AppResources.TextByeMatchCount] = ByeMatchCount;
			return data;
		}

        public int CompareTo(IRule rule, Player obj)
			=> rule.GetComparer(true).Compare(this, obj);

        public Player(IRule rule, string name) : this() {
			_matches = new List<Match>();
			Name = name;
			Point = OpponentPoint = rule.Point(Enumerable.Empty<IPoint>());
		}
	}
}
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

		[JsonProperty]
		private IRule _rule;

		[JsonProperty]
		private List<Match> _matches;

		[JsonProperty]
		IPoint _point;

		[JsonIgnore]
		public IPoint Point => _point = CalcPoint();

		[JsonProperty]
		IPoint _opponent_point;

		[JsonIgnore]
		public IPoint OpponentPoint => _opponent_point = CalcOpponentPoint();

		//[JsonIgnore]
		//public string Description
		//	=> _rule.GetDescription(this);

		public void StepToPlaying(Match match)
			=>_matches.Add(match);

		internal IPoint CalcPoint()
			=> _rule?.Point(_matches.Where(match=>match.IsFinished).Select(match => match.GetRecord(this).Result));

		internal IPoint CalcOpponentPoint()
			=> _rule?.Point(_matches.Where(match => match.IsFinished).Select(match => (match.GetOpponentRecord(this).Player as Player)?.Point));

		public bool IsAllWins()
			=> _matches.Count() > 0 && !_matches.Any(match => match.GetRecord(this).Result.RESULT != RESULT_T.Win);

		public bool IsAllLoses()
			=> _matches.Count() > 0 && !_matches.Any(match => match.GetRecord(this).Result.RESULT != RESULT_T.Lose);

		public int ByeMatchCount()
			=> _matches.Count(match => match.IsByeMatch);

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

		public ExportData Export(ExportData data) {
			data[AppResources.TextPlayerName] = Name;
			data[AppResources.TextDropped] = Dropped;
			Point.Export(data);
			OpponentPoint.Export(new ExportData()).ForEach(pair => data[_dic[pair.Key]] = pair.Value);
			data[AppResources.TextByeMatchCount] = ByeMatchCount();
			return data;
		}

        public int CompareTo(Player obj)
			=> _rule.GetComparer(true).Compare(this, obj);

		public bool ExportTitle(TextWriter writer, string origin) {
			writer.Write(origin + TITLE_NAME);
			writer.Write(",");
			writer.Write(origin + TITLE_DROPPED);
			writer.Write(",");
			Point.ExportTitle(writer, origin);
			OpponentPoint.ExportTitle(writer, origin + TITLE_ORIGIN_OPPONENT);
			writer.Write(TITLE_BYE_MATCH_COUNT);
			writer.Write(",");
			return true;
		}

		public bool ExportData(TextWriter writer) {
			writer.Write("\""+Name+ "\"");
			writer.Write(",");
			writer.Write(Dropped);
			writer.Write(",");
			Point.ExportData(writer);
			OpponentPoint.ExportData(writer);
			writer.Write(ByeMatchCount());
			writer.Write(",");
			return true;

			//data[AppResources.TextPlayerName] = Name;
			//data[AppResources.TextDropped] = Dropped;
			//Point.Export(data);
			//OpponentPoint.Export(new ExportData()).ForEach(pair => data[_dic[pair.Key]] = pair.Value);
			//data[AppResources.TextByeMatchCount] = ByeMatchCount();
			//return data;
		}

		public Player() {// For Serializer
			_matches = new List<Match>();
		}

		public Player(IRule rule, string name):this() {
			Id = Guid.NewGuid();
			_rule = rule;
			Name = name;
//			Point = OpponentPoint = rule?.Point(Enumerable.Empty<IPoint>());
		}
	}
}
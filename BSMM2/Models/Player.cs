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

		[JsonIgnore]
		public IPoint Point { get; private set; }

		[JsonIgnore]
		public IPoint OpponentPoint { get; private set; }

		public string Description(IRule rule)
			=> rule.GetDescription(this);

		public void Commit(Match match)
		{
            if (match.IsByeMatch)
            {
				match.SetResult(RESULT_T.Win);
            }
		}

		internal void CalcPoint(Game game, IRule rule)
			=> Point = rule.Point(game.GetMatches(this).Where(match=>match.IsFinished).Select(match => match.GetRecord(this).Result));

		internal void CalcOpponentPoint(Game game, IRule rule)
			=> OpponentPoint = rule.Point(game.GetMatches(this).Where(match => match.IsFinished).Select(match => (match.GetOpponentRecord(this).Player as Player)?.Point));

		public Player() {// For Serializer
		}

		private readonly IDictionary<string, string> _dic = new Dictionary<string, string>() {
						{AppResources.TextMatchPoint,AppResources.TextOpponentMatchPoint },
						{AppResources.TextWinPoint,AppResources.TextOpponentWinPoint },
						{AppResources.TextLifePoint,AppResources.TextOpponentLifePoint }};

		public IExportData Export(Game game, IExportData data) {
			data[AppResources.TextPlayerName] = Name;
			data[AppResources.TextDropped] = Dropped;
			Point.Export(data);
			OpponentPoint.Export(new ExportData()).ForEach(pair => data[_dic[pair.Key]] = pair.Value);
			data[AppResources.TextByeMatchCount] = game.ByeMatchCount(this);
			return data;
		}

        public int CompareTo(Game game, Player obj)
			=> game.GetComparer(true).Compare(this, obj);

        public Player(IRule rule, string name) : this() {
			Name = name;
			Point = OpponentPoint = rule.Point(Enumerable.Empty<IPoint>());
		}
	}
}
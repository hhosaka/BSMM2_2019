using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	[JsonObject]
	public class Game {

		[JsonIgnore]
		private const int TRY_COUNT = 100;

		[JsonIgnore]
		public static string GameTitle
			=> "Game" + DateTime.Now.ToString();

		[JsonProperty]
		public bool AcceptByeMatchDuplication { get; set; }

		[JsonProperty]
		public bool AcceptGapMatchDuplication { get; set; }

		[JsonProperty]
		public bool AcceptLosersGapMatchDuplication { get; set; }

		[JsonProperty]
		public string Title { get; private set; }

		[JsonProperty]
		public Guid Id { get; private set; }

		[JsonProperty]
		public IRule Rule { get; private set; }

		[JsonProperty]
		public DateTime? StartTime { get; private set; }

		[JsonProperty]
		public Players Players { get; private set; }

		[JsonProperty]
		public List<Round> _rounds;

		[JsonIgnore]
		public IEnumerable<Round> Rounds
			=> _rounds;

		[JsonProperty]
		public Round ActiveRound { get; private set; }

		[JsonIgnore]
		public string Headline => "(Round " + (Rounds?.Count() + 1 ?? 0) + ")"　+ Title;

		//public Comparer<Player> GetComparer( bool force)
		//	=>Rule.GetComparer(force);

		public IEnumerable<Match>GetMatches(Player player) {
			foreach(var round in _rounds) {
				yield return round.Matches.First(m => m.HasPlayer(player));
			}
			if (ActiveRound?.IsPlaying ==true ) {
				yield return ActiveRound.Matches.First(m => m.HasPlayer(player));
			}
		}

		public IEnumerable<Player> GetSortedSource()
			=> Players.GetSortedSource();

		public bool CanAddPlayers() => !ActiveRound.IsPlaying && !_rounds.Any();

		public bool AddPlayers(string data) {
			foreach (var name in data.Split(new[] { '\r', '\n' })) {
				if (!string.IsNullOrEmpty(name)) {
					Players.Add(name);
				}
			}
			Shuffle();// TODO : 一回戦の結果が終わるまでは追加を認めたいのだが…
			return true;
		}

		public ContentPage CreateMatchPage(Match match)
			=> Rule.CreateMatchPage(match);

		public void AddRound(IEnumerable<Match> matches) {
			if (CreateMatching(matches)) {
				StepToPlaying();
			}
		}
		public bool CreateMatching(IEnumerable<Match>matches=null) {
			if(matches==null)
				matches = CreateMatches();

			if (matches != null) {
				ActiveRound = new Round(matches);
				return true;
			}
			return false;
		}

		public Player GetPlayer(int index)
			=>index == BYE.Id?BYE.Instance : Players.Source.ElementAt(index);

		public bool CanExecuteShuffle()
			=> !ActiveRound.IsPlaying;

		public bool Shuffle() {
			if (CanExecuteShuffle()) {
				return CreateMatching();
			}
			return false;
		}

		public bool CanExecuteStepToPlaying()
			=> !ActiveRound.IsPlaying;

		public void StepToPlaying() {
			if (CanExecuteStepToPlaying()) {
				Players.StepToPlaying(ActiveRound.StepToPlaying());
			}
		}

		public bool CanExecuteStartTimer()
			=> ActiveRound.IsPlaying && StartTime==null;

		public void StartTimer()
		{
			StartTime = DateTime.Now;
		}

		public bool CanExecuteStepToMatching()
			=> ActiveRound.IsFinished;

		public bool StepToMatching() {
			if (CanExecuteStepToMatching()) {
				StartTime = null;
				var round = ActiveRound;
				if (CreateMatching()) {
					_rounds.Add(round);
					return true;
				}
			}
			return false;
		}

		public bool IsFinished()
			=> Players.Source.Count(player => player.IsAllWins()) <= 1;

		[JsonIgnore]
		public Func<IEnumerable<Player>, IEnumerable<Player>> RandomizePlayer;

		public static IEnumerable<Player> DefaultRandomizer(IEnumerable<Player> players)
			=> players.OrderBy(i => Guid.NewGuid());

		public static IEnumerable<Player> FixedRandomizer(IEnumerable<Player> players)
			=> players;

		private IEnumerable<Match> CreateMatches() {
			Players.Reset();
			for (int i = 0; i < TRY_COUNT; ++i) {
				var matchingList = Create(RandomizePlayer(Players.Source)
					.OrderByDescending(p => p, Rule.GetComparer(false))
					.Where(p => !p.Dropped));

				if (matchingList != null) {
					return matchingList;
				}
			}
			return null;

			IEnumerable<Match> Create(IEnumerable<Player> players) {
				var results = new Queue<Match>();
				var stack = new List<Player>();
				var id = 0;
				foreach (var p1 in players) {
					var p2 = PickOpponent(stack, p1);
					if (p2 != null) {
						stack.Remove(p2);
						results.Enqueue(Rule.CreateMatch(++id, p2, p1));
					} else {
						stack.Add(p1);
					}
				}
				switch (stack.Count) {
					case 0:
						return results;//組み合わせ完了

					case 1:// 1人余り
						{
							var p = stack.First();
							if (isByeAcceptable(p)) {
								results.Enqueue(Rule.CreateMatch(++id, p));
								return results;//1人不戦勝
							}
						}
						break;
				}
				return null;//組み合わせを作れなかった。

				bool isByeAcceptable(Player p) {
					if (p.IsAllWins()) return false;
					return p.IsAllLoses() || (AcceptByeMatchDuplication || p.ByeMatchCount() == 0);
				}

				Player PickOpponent(IEnumerable<Player> opponents, Player player) {
					foreach (var opponent in opponents) {
						if (player.GetResult(opponent) == null) {//対戦履歴なし
							if (CheckGapMatch()) {
								return opponent;//対戦者認定
							}
						}

						bool CheckGapMatch() {// 階段戦の重複は避ける
							if (AcceptGapMatchDuplication) return true;
							if (IsAcceptLosersGapMatchDuplication()) return true;
							if (player.Point.MatchPoint == opponent.Point.MatchPoint) return true;
							return !player.HasGapMatch() && !opponent.HasGapMatch();

							bool IsAcceptLosersGapMatchDuplication(){
								return AcceptLosersGapMatchDuplication && (!player.IsAllWins() && !opponent.IsAllWins());
							}
						}
					}
					return null;//適合者なし
				}
			}
		}

		public static Game Load(Guid id, Storage storage)
			=> storage.Load<Game>(id.ToString() + ".json", () => new Game());

		public void Save(Storage storage)
			=> storage.Save<Game>(this, Id.ToString() + ".json");

		public void Remove(Storage storage, Game game)
			=> storage.Delete(game.Id.ToString() + ".json");

		public Page CreateRulePage()
			=> Rule.CreateRulePage(this);

		public Game() {// For Serializer
			RandomizePlayer = DefaultRandomizer;
		}

		public Game(IRule rule, Players players, string title = null,Func<IEnumerable<Player>, IEnumerable<Player>>randomizePlayer=null):this() {
			if (title == null)
				Title = GameTitle;
			else
				Title = title;

			Id = Guid.NewGuid();
			Players = players;
			Rule = rule;
			_rounds = new List<Round>();
			StartTime = null;
			if(randomizePlayer!= null) RandomizePlayer = randomizePlayer;
			var result = CreateMatching();
			Debug.Assert(result);
		}
	}
}
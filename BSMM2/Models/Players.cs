using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Internals;

namespace BSMM2.Models
{

	public class Players : IExportableObject
	{
		private const string TITLE_PLAYER = "player{0:000}";
		private const string DEFAULT_PREFIX = "Player";
		public const int MAX_COUNT = 32;

		[JsonProperty]
		private IRule _rule;

		[JsonIgnore]
		public IRule Rule => _rule;

		[JsonProperty]
		private String _prefix;

		[JsonProperty]
		private List<Player> _players;

		[JsonIgnore]
		public List<Player> Source => _players;

		[JsonIgnore]
		public int Count => _players.Count;

		private IEnumerable<Player> Generate(int start, string prefix, int count = 1) {
			//Debug.Assert(count > 0);
			Debug.Assert(_rule != null);
			for (int i = 0; i < count; ++i) {
				yield return new Player(_rule, string.Format("{0}{1:000}", prefix, start + i));
			}
		}

		public void StepToPlaying(IEnumerable<Match> matches) {
			foreach (var player in _players.Where(player=>!player.Dropped))
				player.StepToPlaying(matches.First(m => m.HasPlayer(player)));
		}

		public Players() {
		}

		public Players(IRule rule, Players players) {
			Debug.Assert(players.Count <= MAX_COUNT);
			_rule = rule;
			_prefix = players._prefix;
			_players = players.Source.Select(player => new Player(rule, player.Name)).ToList();
		}

		public Players(IRule rule, int count) {
			Debug.Assert(count <= MAX_COUNT);
			_rule = rule;
			_players = Generate(1, rule.Prefix, count).ToList();
		}

		public static IEnumerable<Player> FromStream(IRule rule, TextReader reader) {
			String buf;
			while ((buf = reader.ReadLine()) != null) {
				if (!String.IsNullOrWhiteSpace(buf))
					yield return new Player(rule, buf);
			}
		}

		public Players(IRule rule, IEnumerable<Player>players, String prefix = DEFAULT_PREFIX) {
			_rule = rule;
			_prefix = prefix;
			_players = players.ToList();
			Debug.Assert(_players.Count <= MAX_COUNT);
		}

		public bool Add(int count = 1, String prefix = DEFAULT_PREFIX) {
			if (count + _players.Count <= MAX_COUNT) {
				int start = _players.Count() + 1;
				_players.AddRange(Generate(start, prefix, count));
				return true;
			}
			return false;
		}

		public bool Add(String name) {
			Debug.Assert(_rule != null);
			if (_players.Count < MAX_COUNT) {
				_players.Add(new Player(_rule, name));
				return true;
			}
			return false;
		}

		public void Remove(int index)
			=> _players.RemoveAt(index);

		public void Remove(Player player)
			=> _players.Remove(player);

		// For Debug
		public void Swap(int x, int y) {
			var temp = _players[x];
			_players[x] = _players[y];
			_players[y] = temp;
		}

		public IEnumerable<Player> GetSortedPlayers() {
			Debug.Assert(_rule != null);
			if (_players == null) {
				return Enumerable.Empty<Player>();
			} else {
				return _players.OrderByDescending(p => p, _rule.GetComparer(true));
			}
		}

		public IEnumerable<OrderedPlayer> GetOrderedPlayers() {
			Debug.Assert(_rule != null);
			Player prev = null;
			int order = 0;
			int count = 0;
			foreach (var p in GetSortedPlayers()) {
				if (prev == null || prev.CompareTo(p) != 0) {
					order = count;
					prev = p;
				}
				yield return new OrderedPlayer(_rule, p, order + 1);
				++count;
			}
		}

		public ExportSource Export(ExportSource src, string origin = "") {
			int i = 0;
			foreach (var player in GetOrderedPlayers()) {
				src.Add(string.Format(TITLE_PLAYER, ++i), player.Export(new ExportSource(), origin));
			}
			return src;
		}
	}
}
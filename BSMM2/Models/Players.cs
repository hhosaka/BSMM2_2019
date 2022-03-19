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

	public class Players
	{
		private const String DEFAULT_PREFIX = "Player";

		[JsonProperty]
		private IRule _rule;

		[JsonProperty]
		private String _prefix;

		[JsonProperty]
		private List<Player> _players;

		[JsonIgnore]
		public List<Player> Source => _players;

		[JsonIgnore]
		public int Count => _players.Count;

		private IEnumerable<Player> Generate(int start, string prefix, int count = 1) {
			Debug.Assert(count > 0);
			for (int i = 0; i < count; ++i) {
				yield return new Player(_rule, string.Format("{0}{1:000}", prefix, start + i));
			}
		}

		public void StepToPlaying(IEnumerable<Match> matches) {
			foreach (var player in _players)
				player.StepToPlaying(matches.First(m => m.HasPlayer(player)));
		}

		public Players() {
		}

		public Players(IRule rule, Players players) {
			_rule = rule;
			_prefix = players._prefix;
			_players = players.Source.Select(player => new Player(rule, player.Name)).ToList();
		}

		public Players(IRule rule, int count) {
			_rule = rule;
			_players = Generate(1, rule.Prefix, count).ToList();
		}

		public Players(IRule rule, TextReader r, String prefix = DEFAULT_PREFIX) {
			_rule = rule;
			_prefix = prefix;
			_players = Generate().ToList();

			IEnumerable<Player> Generate() {
				String buf;
				while ((buf = r.ReadLine()) != null) {
					if (!String.IsNullOrWhiteSpace(buf))
						yield return new Player(rule, buf);
				}
			}
		}

		public void Add(int count = 1, String prefix = DEFAULT_PREFIX) {
			int start = _players.Count() + 1;
			_players.AddRange(Generate(start, prefix, count));
		}

		public void Add(String name)
			=> _players.Add(new Player(_rule, name));

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

		public void Export(TextWriter writer) {
			_players.First()?.Export(new ExportData()).Keys.ForEach(key => writer.Write(key + ", "));
			writer.WriteLine();
			foreach (var player in _players) {
				foreach (var param in player.Export(new ExportData())) {
					switch (param.Value) {
						case string str:
							writer.Write(string.Format("\"{0}\", ", str));
							break;

						default:
							writer.Write(param.Value + ", ");
							break;
					}
				}
				writer.WriteLine();
			}
		}
		public IEnumerable<Player> GetSortedPlayers() {
			if (_players == null) {
				return Enumerable.Empty<Player>();
			} else {
				return _players.OrderByDescending(p => p, _rule.GetComparer(true));
			}
		}

		public IEnumerable<OrderedPlayer> GetOrderedPlayers() {
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
	}
}
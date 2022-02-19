using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	public class Players {
		private const String DEFAULT_PREFIX = "Player";

		public static IEnumerable<OrderedPlayer> GetOrderedPlayers(Game game, IRule rule, IEnumerable<Player> players)
		{
			Player prev = null;
			int order = 0;
			int count = 0;
			foreach (var p in players)
			{
				if (prev == null || prev.CompareTo(game, rule, p) != 0)
				{
					order = count;
					prev = p;
				}
				yield return new OrderedPlayer(rule, p, order + 1);
				++count;
			}
		}

		[JsonProperty]
		private String _prefix;

		[JsonProperty]
		private List<Player> _players;

		[JsonIgnore]
		public List<Player> Source => _players;

		[JsonIgnore]
		public int Count => _players.Count;

		private IEnumerable<Player> Generate(IRule rule, int start, string prefix, int count = 1) {
			Debug.Assert(count > 0);
			for (int i = 0; i < count; ++i) {
				yield return new Player(rule, string.Format("{0}{1:000}", prefix, start + i));
			}
		}

		public Players() {
		}

		public Players(IRule rule, Players players) {
			_prefix = players._prefix;
			_players = players.Source.Select(player => new Player(rule, player.Name)).ToList();
		}

		public Players(IRule rule, int count) {
			_players = Generate(rule, 1, rule.Prefix, count).ToList();
		}

		public Players(IRule rule, TextReader r, String prefix = DEFAULT_PREFIX) {
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

		public void Add(IRule rule, int count = 1, String prefix = DEFAULT_PREFIX) {
			int start = _players.Count() + 1;
			_players.AddRange(Generate(rule, start, prefix, count));
		}

		public void Add(IRule rule, String name)
			=> _players.Add(new Player(rule, name));

		//public void Remove(int index)
		//	=> _players.RemoveAt(index);

		//public void Remove(Player player)
		//	=> _players.Remove(player);

		public void Reset(Game game, IRule rule) {
			_players.ForEach(p => p.CalcPoint(game, rule));
			_players.ForEach(p => p.CalcOpponentPoint(game, rule));
		}

		// For Debug
		public void Swap(int x, int y) {
			var temp = _players[x];
			_players[x] = _players[y];
			_players[y]=temp;
		}

		public void Export(Game game, IRule rule, TextWriter writer) {
			_players.First()?.Export(game, new ExportData()).Keys.ForEach(key => writer.Write(key + ", "));
			writer.WriteLine();
			Reset(game, rule);
			foreach (var player in _players) {
				foreach (var param in player.Export(game, new ExportData())) {
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
		public IEnumerable<Player> GetSortedSource(Game game, IRule rule)
		{
			if (_players == null)
			{
				return Enumerable.Empty<Player>();
			}
			else
			{
				Reset(game, rule);
				return _players.OrderByDescending(p => p, rule.GetComparer(game, true));
			}
		}
	}
}
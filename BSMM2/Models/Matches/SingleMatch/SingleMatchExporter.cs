using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models.Matches.SingleMatch
{
	internal class SingleMatchExporter : IExporter {
		[JsonObject]
		class ThePlayer {
			[JsonProperty]
			public int Order { get; }

			[JsonProperty]
			public string Name { get; }

			[JsonProperty]
			public IPoint Result { get; }

			public ThePlayer(OrderedPlayer player) {
				Order = player.Order;
				Name = player.Name;
				Result = player.Player.Point;
			}
		}

		[JsonObject]
		class ThePlayers {
			[JsonProperty]
			public string Title { get; }

			[JsonProperty]
			public string Owner { get; }

			[JsonProperty]
			public DateTime Date { get; }

			[JsonProperty]
			public IEnumerable<OrderedPlayer> Players { get; }

			public ThePlayers(Game game) {
				Title = game.Title;
				Owner = "TBD";
				Date = DateTime.Now;
				Players = game.Players.GetOrderedPlayers();
			}

		}
		public object ExportPlayers(Game game) {
			return new ThePlayers(game);
		}
	}
}

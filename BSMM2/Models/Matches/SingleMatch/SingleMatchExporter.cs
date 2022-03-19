using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BSMM2.Models.Matches.SingleMatch
{
	internal class SingleMatchExporter : IExporter {
		//[JsonObject]
		//class ThePlayer:IExportableObject {
		//	[JsonProperty]
		//	public int Order { get; }

		//	[JsonProperty]
		//	public string Name { get; }

		//	[JsonProperty]
		//	public IPoint Result { get; }

		//	public ThePlayer(OrderedPlayer player) {
		//		Order = player.Order;
		//		Name = player.Name;
		//		Result = player.Player.Point;
		//	}

		//	public bool ExportData(TextWriter writer) {
		//		writer.Write(Order);
		//		writer.Write(",");
		//		writer.Write(Name);
		//		writer.Write(",");
		//		return Result.ExportData(writer);
		//	}

		//	public const string TITLE_ORDER = "order";
		//	public const string TITLE_NAME = "name";
		//	public bool ExportTitle(TextWriter writer) {
		//		writer.Write(TITLE_ORDER);
		//		writer.Write(",");
		//		writer.Write(TITLE_NAME);
		//		writer.Write(",");
		//		return Result.ExportTitle(writer);
		//	}
		//}

		[JsonObject]
		class ThePlayers:IExportableObject {
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

			public bool ExportData(TextWriter writer) {
				//writer.WriteLine(Title);
				//writer.WriteLine(Owner);
				//writer.WriteLine(Date);
				foreach (var player in Players) {
					player.ExportData(writer);
					writer.WriteLine();
				}
				return true;
			}

			public bool ExportTitle(TextWriter writer,  string origin) {
				return Players.FirstOrDefault()?.ExportTitle(writer, origin) ??false;
			}
		}
		public IExportableObject ExportPlayers(Game game) {
			return new ThePlayers(game);
		}
	}
}

using Newtonsoft.Json;
using System;
using System.IO;

namespace BSMM2.Models {

	[JsonObject]
	public class OrderedPlayer:IExportableObject{

		public const string TITLE_ORDER = "order";

		[JsonProperty]
		public int Order { get; }

		[JsonProperty]
		public Player Player { get; }

		public OrderedPlayer(IRule rule, Player player, int order)
		{
			Player = player;
			Order = order;
		}

		public bool ExportData(TextWriter writer) {
			writer.Write(Order);
			writer.Write(",");
			return Player.ExportData(writer);
		}

		public bool ExportTitle(TextWriter writer, string origin) {
			writer.Write(origin + TITLE_ORDER);
			writer.Write(",");
			return Player.ExportTitle(writer, origin);
		}
	}
}
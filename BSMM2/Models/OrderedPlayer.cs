using Newtonsoft.Json;
using System;
using System.IO;

namespace BSMM2.Models {

	[JsonObject]
	public class OrderedPlayer:IExportableObject{

		public const string TITLE_ORDER = "order";
		public const string TITLE_NAME = "name";

		[JsonProperty]
		public int Order { get; }

		[JsonProperty]
		public string Name { get; }

		[JsonProperty]
		public IPoint Point { get; }

		[JsonIgnore]
		public Player Player { get; }

		public OrderedPlayer(IRule rule, Player player, int order)
		{
			Player = player;
			Order = order;
			Name = Player.Name;
			Point = Player.Point;
		}

		public bool ExportData(TextWriter writer) {
			writer.Write(Order);
			writer.Write(",");
			writer.Write(Name);
			writer.Write(",");
			return Point.ExportData(writer);
		}

		public bool ExportTitle(TextWriter writer) {
			writer.Write(TITLE_ORDER);
			writer.Write(",");
			writer.Write(TITLE_NAME);
			writer.Write(",");
			return Point.ExportData(writer);
		}
	}
}
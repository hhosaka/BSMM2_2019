using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace BSMM2.Models {

	[JsonObject]
	public class OrderedPlayer{

		public const string TITLE_ORDER = "order";

		[JsonProperty]
		public int Order { get; }

		[JsonProperty]
		public string Name => Player.Name;

		[JsonProperty]
		public IPoint Point => Player.Point;

		[JsonProperty]
		public IPoint OpponentPoint => Player.OpponentPoint;

		[JsonIgnore]
		public Player Player { get; }

		public OrderedPlayer(IRule rule, Player player, int order)
		{
			Order = order;
			Player = player;
		}

		public ExportSource Export(ExportSource src, string origin="") {
			src.Add(TITLE_ORDER, Order);
			return Player?.Export(src, origin);
		}
	}
}
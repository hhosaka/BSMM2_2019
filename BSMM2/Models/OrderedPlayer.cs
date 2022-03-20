using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

		public ExportSource Export(ExportSource src, string origin="") {
			src.Add(TITLE_ORDER, Order);
			return Player.Export(src, origin);
		}
	}
}
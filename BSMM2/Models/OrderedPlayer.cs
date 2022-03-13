using Newtonsoft.Json;
using System;

namespace BSMM2.Models {

	[JsonObject]
	public class OrderedPlayer
	{
		private Player _player;

		[JsonIgnore]
		public Player Player => _player;

		[JsonProperty]
		public string Name { get; }

		[JsonProperty]
		public IPoint Point { get; }

		[JsonProperty]
		public int Order { get; }

		public OrderedPlayer(IRule rule, Player player, int order)
		{
			_player = player;
			Order = order;
			Name = _player.Name;
			Point = _player.Point;
		}
	}
}
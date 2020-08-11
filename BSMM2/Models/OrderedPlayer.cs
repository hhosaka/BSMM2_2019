using System;

namespace BSMM2.Models {

	public class OrderedPlayer
	{
			private Player _player;

			public Player Player => _player;

			public string Name => _player.Name;

			public string Description => _player.Description;

			public int Order { get; }

			public OrderedPlayer(Player player, int order)
			{
				_player = player;
				Order = order;
			}
	}
}
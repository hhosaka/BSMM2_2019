using System;

namespace BSMM2.Models {

	public class OrderedPlayer
	{
		private Player _player;
		private IRule _rule;

		public Player Player => _player;

		public string Name => _player.Name;

		public string Description => _player.Description(_rule);

		public int Order { get; }

		public OrderedPlayer(IRule rule, Player player, int order)
		{
			_rule = rule;
			_player = player;
			Order = order;
		}
	}
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models.WebAccess
{
	[JsonObject]
	public class GameOutline
	{
		public static GameOutline Create(Game game)
			=> new GameOutline(game);

		[JsonIgnore]
		private Game _game;
		[JsonProperty]
		public string Title => _game.Title;
		[JsonProperty]
		public string Owner => _game.Owner;
		[JsonProperty]
		public DateTime?Started => _game.StartTime;
		[JsonProperty]
		public string RuleName => _game.Rule.Name;
		[JsonProperty]
		public string Id => _game.WebServiceId.ToString();
		[JsonProperty]
		public bool EnableLifePoint => true;//TODO: TBD
		public GameOutline(Game game) {
			_game = game;
		}
	}
}

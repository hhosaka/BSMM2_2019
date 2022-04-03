using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models.WebAccess
{
	[JsonObject]
	public class GameOutline
	{
		public static GameOutline Create(BSMMApp app)
			=> new GameOutline(app);

		[JsonIgnore]
		private BSMMApp _app;
		[JsonIgnore]
		private Game Game => _app.Game;

		[JsonProperty]
		public string Title => Game.Title;
		[JsonProperty]
		public string Owner => _app.Owner;
		[JsonProperty]
		public string ElapseTime => (DateTime.Now - Game.StartTime)?.ToString(@"hh\:mm\:ss");
		[JsonProperty]
		public string RuleName => Game.Rule.Name;
		[JsonProperty]
		public string Id => Game.WebServiceId.ToString();
		[JsonProperty]
		public string MailAddress => _app.MailAddress;
		[JsonProperty]
		public bool EnableLifePoint => Game.Rule.EnableLifePoint;
		public GameOutline(BSMMApp app) {
			_app = app;
		}
	}
}

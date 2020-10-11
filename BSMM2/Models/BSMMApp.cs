using BSMM2.Models.Matches.MultiMatch.FiveGameMatch;
using BSMM2.Models.Matches.MultiMatch.ThreeGameMatch;
using BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch;
using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BSMM2.Models {

	[JsonObject]
	public class BSMMApp {

		public static BSMMApp Create(string path, bool force) {
			var storage = new Storage();

			if (force) {
				return Initiate();
			} else {
				try {
					return storage.Load<BSMMApp>(path, Initiate);
				} catch (Exception) {
					return Initiate();
				}
			}

			BSMMApp Initiate() {
				var app = new BSMMApp(storage,
						path,
						new IRule[] {
					new SingleMatchRule(),
					new ThreeGameMatchRule(),
					new ThreeOnThreeMatchRule(),
					new FiveGameMatchRule(),
						});
				app.Save(true);
				return app;
			}
		}

		[JsonProperty]
		public Guid Id { get; private set; }

		[JsonProperty]
		public bool IsDebugMode { get; set; }

		[JsonProperty]
		private string _path;

		[JsonIgnore]
		private Storage _storage;

		[JsonProperty]
		private List<Game> _games;

		[JsonIgnore]
		public IEnumerable<Game> Games => _games;

		[JsonProperty]
		public IEnumerable<IRule> Rules { get; private set; }

		[JsonProperty]
		public IRule Rule { get; set; }

		[JsonProperty]
		public Game Game { get; set; }

		[JsonProperty]
		public bool AutoSave { get; set; }

		[JsonProperty]
		public string MailAddress { get; set; }

		[JsonProperty]
		public string EntryTemplate { get; set; }

		public bool Add(Game game, bool AsCurrentGame) {
			if (AsCurrentGame) {
				_games.Remove(Game);
			}
			_games.Add(game);
			Game = game;
			return true;
		}

		public bool Remove(Game game) {
			if (_games.Count > 1) {
				_games.Remove(game);
				Game = _games.Last();
				return true;
			}
			return false;
		}

		public void Save(bool force) {
			if (force || AutoSave)
				_storage.Save(this, _path);
		}

		public async Task SendByMail(string subject, string body)
			=> await SendByMail(subject, body, new[] { MailAddress });

		public async Task SendByMail(string subject, string body, IEnumerable<string> recipients) {
			try {
				var message = new EmailMessage {
					Subject = subject,
					Body = body,
					To = recipients.ToList(),
				};
				await Email.ComposeAsync(message);
			} catch (FeatureNotSupportedException) {
				// Email is not supported on this device
			}
		}

		public async void ExportPlayers() {
			var buf = new StringBuilder();
			Game.Players.Export(new StringWriter(buf));
			await SendByMail(Game.Headline, buf.ToString(), new[] { MailAddress });
		}

		private BSMMApp(Storage storage) {
			_storage = storage;
			MessagingCenter.Subscribe<object>(this, Messages.REFRESH, (sender) => Save(false));
		}

		public BSMMApp() : this(new Storage()) {// for Serializer
		}

		private BSMMApp(Storage storage, string path, IRule[] rules) : this(storage) {
			Id = Guid.NewGuid();
			Rules = rules;
			_path = path;
			Rule = Rules.First();
			_games = new List<Game>() { new Game(rules[0], new Players(rules[0], 8)) };
			Game = _games.Last();
			AutoSave = true;
		}
	}
}
using BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch;
using BSMM2.Models.Matches.SingleMatch;
using BSMM2.Models.WebAccess;
using BSMM2.Resource;
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

		public const string WebURL = "http://localhost/bsmm2svr/";//TODO TBD

		private const int VERSION = 3;

		[JsonProperty]
		private readonly int _version;

		[JsonIgnore]
		private static  string _information;

		public static BSMMApp Create(string path, bool force) {// TODO: Separate to Load and Create
			var storage = new Storage();

			if (force) {
				return Initiate();
			} else {
				try {
					var _app =  storage.Load<BSMMApp>(path, Initiate);
					switch (_app?._version) {
						case VERSION:
							return _app;
						default:
							_information = AppResources.TextLegacyVersion;
							return Initiate();
					}
				} catch (Exception) {
					_information = AppResources.TextCorruptedData;
					return Initiate();
				}
			}

			BSMMApp Initiate() {
				var app = new BSMMApp(storage, path,
						new []{
							new SingleMatchRule(),
							new Matches.MultiMatch.NthGameMatch.NthGameMatchRule(2),
							new ThreeOnThreeMatchRule(),
							new Matches.MultiMatch.NthGameMatch.NthGameMatchRule(3)
						});
				return app;
			}
		}

		[JsonProperty]
		public Guid Id { get; }

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
		public List<IRule> Rules { get; private set; }

		[JsonProperty]
		public IRule Rule { get; set; }

		[JsonProperty]
		public Game Game { get; set; }

		[JsonProperty]
		public bool AutoSave { get; set; }

		[JsonProperty]
		public string MailAddress { get; set; }

		[JsonProperty]
		public string Password { get; set; }

		[JsonProperty]
		public string EntryTemplate { get; set; }

		[JsonProperty]
		public string Owner { get; set; }

		[JsonProperty]
		public bool UseUniqueId4WebService { get; set; }

		public void VersionCheck(Action<string>display) {
			if (!string.IsNullOrEmpty(_information)) {
				display?.Invoke(_information);
				_information = null;
			}
		}

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

		public async Task<bool> Save(bool force) {
			if (force || AutoSave) {
				_storage.Save(this, _path);
				return await new WebClient().Upload(WebURL, MailAddress, Password, Game);
			}
			return false;
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
			CSVConverter.Convert(Game.Players.Export(new ExportSource()), new StringWriter(buf));
			await SendByMail(Game.Headline, buf.ToString(), new[] { MailAddress });
		}

		private BSMMApp(Storage storage) {
			_storage = storage;
			MessagingCenter.Subscribe<object>(this, Messages.REFRESH, (sender) => Save(false));
		}

		public BSMMApp() : this(new Storage()) {// for Serializer
		}

		private BSMMApp(Storage storage, string path,IEnumerable<IRule>rules) : this(storage) {
			_version = VERSION;
			Id = Guid.NewGuid();
			_path = path;
			Rules = new List<IRule>(rules);
			Rule = Rules.First();
			_games = new List<Game>() { new Game(new Players(Rule, 8)) };
			Game = _games.Last();
			AutoSave = true;
		}
	}
}
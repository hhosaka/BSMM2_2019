 using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSMM2.Models.WebAccess
{
	public class WebClient
	{
		private class AccountInfo
		{
			public string Key => "BSMM2Svr";
			public string MailAddress { get; }
			public string Password { get; }

			public AccountInfo(string mailAddress, string password) {
				MailAddress = mailAddress;
				Password = password;
			}
		}

		public async Task<bool> Upload(string url, string id, string password, BSMMApp app) 
		{
			Debug.Assert(!string.IsNullOrEmpty(app.MailAddress));
			var game = app.Game;
            var cc = await LoginAsync(url, id, password);
			if (cc != null) {
				if (await SendDataAsync<GameOutline>(cc, url + "games/uploadOutline/" + game.WebServiceId, GameOutline.Create(app))) {
					if (await SendDataAsync<Game>(cc, url + "games/uploadGame/" + game.WebServiceId, game)) {
						if (await SendDataAsync<IEnumerable<OrderedPlayer>>(cc, url + "games/uploadPlayers/" + game.WebServiceId, game.Players.GetOrderedPlayers())) {
							return await SendDataAsync<IEnumerable<MatchOutline>>(cc, url + "games/uploadMatches/" + game.WebServiceId, MatchOutline.Create(game.ActiveRound.Matches));
						}
					}
				}
			}
			return false;
		}

		private async Task<CookieContainer> LoginAsync(string url, string id, string pw) {
            var LOGIN_ADDRESS = url+"users/login";

            using (var handler = new HttpClientHandler()) {
                using (var client = new HttpClient(handler)) {
					//ログイン用のPOSTデータ生成
					var content = new FormUrlEncodedContent(new Dictionary<string, string>
					{
						{ "email", id },
						{ "password", pw }
					});

					await client.PostAsync(LOGIN_ADDRESS, content);
					return handler.CookieContainer;
                }
            }
        }

		private async Task<bool> SendDataAsync<T>(CookieContainer cc, string url, T data) {
			var buf = new StringBuilder();
			new Serializer<T>().Serialize(new StringWriter(buf), data);

			using (var handler = new HttpClientHandler()) {
				using (var client = new HttpClient(handler)) {
					handler.CookieContainer = cc;
					var response = await client.PostAsync(url, new StringContent(buf.ToString(), Encoding.UTF8, "application/json"));
					return response.StatusCode==HttpStatusCode.OK;
				}
			}
		}

	}

}

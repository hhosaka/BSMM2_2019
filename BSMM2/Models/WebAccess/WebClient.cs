 using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSMM2.Models.WebAccess
{
	public class WebClient
	{
		public async Task<bool> Upload(string url, string mailAddress, string password, Game game) 
		{

            var cc = await LoginAsync(url, mailAddress, password);
			if (cc != null) {
				if(await SendDataAsync<GameOutline>(cc, url+"users/uploadGame", GameOutline.Create(game))) {
					if(await SendDataAsync<IEnumerable<OrderedPlayer>>(cc, url + "users/uploadPlayers", game.Players.GetOrderedPlayers())) {
						return await SendDataAsync<IEnumerable<MatchOutline>>(cc, url + "users/uploadMatches", MatchOutline.Create(game.ActiveRound.Matches));
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

 using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace BSMM2.Models.WebAccess
{
	public class Client
	{
		WebClient webclient;
		public async void Upload(Game game) 
		{
			var buf = new StringBuilder();

            new Serializer<Game>().Serialize(new StringWriter(buf), game);
            await LoginAsync(buf.ToString());
		}

        async Task<CookieContainer> LoginAsync(string buf) {
            var LOGIN_ADDRESS = "http://localhost/bsmm2svr/users/login";

            CookieContainer cc;
            using (var handler = new HttpClientHandler()) {
                using (var client = new HttpClient(handler)) {
                    //ログイン用のPOSTデータ生成
                    var content = new FormUrlEncodedContent(new Dictionary<string, string>
                    {
                        { "email", "admin@gmail.com" },
                        { "password", "admin" }
                    });

                    //ログイン
                    await client.PostAsync(LOGIN_ADDRESS, content);

                    //クッキー保存
                    cc = handler.CookieContainer;

                    var response = await client.PostAsync("http://localhost/bsmm2svr/users/upload", new StringContent(buf, Encoding.UTF8,"application/json"));
                }
            }
            CookieCollection cookies = cc.GetCookies(new Uri(LOGIN_ADDRESS));

            return cc;
        }

    }

}

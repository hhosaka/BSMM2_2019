using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BSMM2.Models.Server
{
	public class TheServer
	{
        private TcpListener tcplistener;
        public int Port { get; set; } = 8000;

        public void Start() {
            var hostname = Dns.GetHostName();
            var hosts = Dns.GetHostAddresses(hostname);
            var address = hosts[0].ToString();
            Task.Run(()=>Execute());
            new WebClient();
        }

        private async void Execute() {
            tcplistener = new TcpListener(IPAddress.Any, Port);
            tcplistener.Start();

            while (true) {
                var tcpClient = await tcplistener.AcceptTcpClientAsync();
                await Task.Run(async () => {
                    using (var stream = tcpClient.GetStream())
                    using (var reader = new StreamReader(stream))
                    using (var writer = new StreamWriter(stream)) {
                        var requestHeaders = new List<string>();
                        while (true) {
                            var line = await reader.ReadLineAsync();
                            if (String.IsNullOrWhiteSpace(line)) {
                                break;
                            }
                            requestHeaders.Add(line);
                        }

                        // 一行目(リクエストライン)は [Method] [Path] HTTP/[HTTP Version] となっている
                        var requestLine = requestHeaders.FirstOrDefault();
                        var requestParts = requestLine?.Split(new[] { ' ' }, 3);
                        if (!requestHeaders.Any() || requestParts.Length != 3) {
                            await writer.WriteLineAsync("HTTP/1.0 400 Bad Request");
                            await writer.WriteLineAsync("Content-Type: text/plain; charset=UTF-8");
                            await writer.WriteLineAsync();
                            await writer.WriteLineAsync("Bad Request");
                            return;
                        }

                        var path = requestParts[1];
                        if (path == "/") {
                            await writer.WriteLineAsync("HTTP/1.0 200 OK");
                            await writer.WriteLineAsync("Content-Type: text/plain; charset=UTF-8");
                            await writer.WriteLineAsync();
                            await writer.WriteLineAsync("Hello! Konnichiwa! @ " + DateTime.Now); // 動的感を出す
                        }
                    }
                });
            }
        }
    }
}

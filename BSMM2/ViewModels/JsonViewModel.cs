using BSMM2.Models;
using Prism.Commands;
using System;
using System.IO;
using System.Text;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	public class JsonViewModel : BaseViewModel {
		private string _buf;

		public string Buf {
			get => _buf;
			set { SetProperty(ref _buf, value); }
		}

		public bool AsCurrentGame { get; set; }
		public DelegateCommand ExportCommand { get; }
		public DelegateCommand ImportCommand { get; }
		public DelegateCommand ClearCommand { get; }
		public DelegateCommand SendByMailCommand { get; }

		public JsonViewModel(BSMMApp app, Action Close) {
			AsCurrentGame = true;

			ExportCommand = new DelegateCommand(Export);
			ImportCommand = new DelegateCommand(Import);
			ClearCommand = new DelegateCommand(Clear);
			SendByMailCommand = new DelegateCommand(SendByMail);

			void Export() {
				var buf = new StringBuilder();
				new Serializer<Game>().Serialize(new StringWriter(buf), app.Game);
				Buf = buf.ToString();
			}

			void Import() {
				app.Add(new Serializer<Game>().Deserialize(new StringReader(Buf)), AsCurrentGame);
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				Close?.Invoke();
			}

			void Clear() {
				Buf = "";
			}

			async void SendByMail() {
				await app.SendByMail(app.Game.Headline, Buf);
			}
		}
	}
}
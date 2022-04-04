using BSMM2.Models;
using BSMM2.Models.WebAccess;
using Plugin.Clipboard;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	public class SettingsViewModel : BaseViewModel {

		public BSMMApp App { get; }

		private int _count = 0;

		public bool AutoSave {
			get => App.AutoSave;
			set {
				App.AutoSave = value;
				if (++_count > 10) {
					_count = 0;
					IsDebugMode = !IsDebugMode;
				}
				OnPropertyChanged(nameof(App.AutoSave));
			}
		}

		private void SetAutoSave(bool value) {
		}

		public string MailAddress{
			get=> App.MailAddress;
			set {
				if (App.MailAddress != value) {
					App.MailAddress = value;
					if (string.IsNullOrEmpty(value)) {
						ActiveWebService = false;
						IsEnableActiveWebService = false;
					} else {
						IsEnableActiveWebService = true;
					}
					OnPropertyChanged(nameof(MailAddress));
				}
			}
		}

		private bool _isEnableActiveWebService;
		public bool IsEnableActiveWebService {
			get => _isEnableActiveWebService;
			set {
				_isEnableActiveWebService = value;
				OnPropertyChanged(nameof(IsEnableActiveWebService));
			}
		}

		public bool ActiveWebService {
			get => App.ActiveWebService;
			set {
				if (App.ActiveWebService != value) {
					App.ActiveWebService = value;
					OnPropertyChanged(nameof(ActiveWebService));
				}
			}
		}

		public bool IsDebugMode
        {
			get => App.IsDebugMode;
			set {
				App.IsDebugMode = value;
				OnPropertyChanged(nameof(IsDebugMode));
			}
		}

		private void SetMailAddress(string value) {
			App.MailAddress = value;
			if (string.IsNullOrEmpty(value)) {
				ActiveWebService = false;
				IsEnableActiveWebService = false;
			} else {
				IsEnableActiveWebService = true;
			}
		}

		public ICommand ExportAppCommand { get; }
		public ICommand ImportAppCommand { get; }

		public SettingsViewModel(BSMMApp app, Action<string> displayAlert) {
			App = app;
			IsDebugMode = App.IsDebugMode;
			IsEnableActiveWebService = !string.IsNullOrEmpty(MailAddress);

			ExportAppCommand = new Command(Export);
			ImportAppCommand = new Command(Import);

			void Export()
			{
				try {
					var buf = new StringBuilder();
					new Serializer<Game>().Serialize(new StringWriter(buf), app.Game);
					CrossClipboard.Current.SetText(buf.ToString());
                }
                catch (Exception e)
                {
					displayAlert?.Invoke(e.Message);
                }
			}

			async void Import()
			{
				try
				{
					app.Add(new Serializer<Game>().Deserialize(new StringReader(await CrossClipboard.Current.GetTextAsync())), false);
					MessagingCenter.Send<object>(this, Messages.REFRESH);
				}
				catch (Exception e)
				{
					displayAlert?.Invoke(e.Message);
				}
			}
		}
	}
}
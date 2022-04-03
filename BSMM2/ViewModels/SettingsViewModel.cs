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

		private bool _autoSave;
		public bool AutoSave {
			get => _autoSave;
			set => SetProperty(ref _autoSave, value, nameof(AutoSave), () => SetAutoSave(value));
		}

		private string _mailAddress;
		public string MailAddress{
			get=>_mailAddress;
			set	=> SetProperty(ref _mailAddress, value, nameof(MailAddress),()=>SetMailAddress(value));
		}

		private bool _isEnableActiveWebService;
		public bool IsEnableActiveWebService {
			get => _isEnableActiveWebService;
			set => SetProperty(ref _isEnableActiveWebService, value, nameof(IsEnableActiveWebService));
		}

		private bool _activeWebService;
		public bool ActiveWebService {
			get => App.ActiveWebService;
			set => SetProperty(ref _activeWebService, value, nameof(ActiveWebService),()=>App.ActiveWebService=value);
		}

		private bool _isDebugMode;
		public bool IsDebugMode
        {
			get => _isDebugMode;
			set => SetProperty(ref _isDebugMode, value, nameof(IsDebugMode), () => App.IsDebugMode=value);
		}

		private void SetAutoSave(bool value)
        {
			App.AutoSave = value;
            if (++_count > 10)
				IsDebugMode = !IsDebugMode;
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
			AutoSave = app.AutoSave;
			MailAddress = app.MailAddress;
			ActiveWebService = app.ActiveWebService;
			IsDebugMode = app.IsDebugMode;

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
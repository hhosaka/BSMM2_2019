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

		private BSMMApp _app;

		private int  _count = 0;

		private bool _autoSave;
		public bool AutoSave {
			get => _autoSave;
			set=> SetProperty(ref _autoSave, value,nameof(AutoSave), ()=>SetAutoSave(value));
		}

		public bool ActiveWebService {
			get => _app.ActiveWebService;
			set => _app.ActiveWebService = value;
		}

		private bool _isDebugMode;
		public bool IsDebugMode
        {
			get => _isDebugMode;
			set => SetProperty(ref _isDebugMode, value, nameof(IsDebugMode), () => _app.IsDebugMode=value);
		}

		public string MailAddress
		{
			get => _app.MailAddress;
			set => _app.MailAddress = value;
		}

		private void SetAutoSave(bool value)
        {
			_app.AutoSave = value;
            if (++_count > 10)
				IsDebugMode = !IsDebugMode;
        }

		private void SetActiveWebService(bool value) {
			_app.ActiveWebService = value;
		}

		public ICommand ExportAppCommand { get; }
		public ICommand ImportAppCommand { get; }

		public SettingsViewModel(BSMMApp app, Action<string> displayAlert) {
			_app = app;
			AutoSave = app.AutoSave;
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
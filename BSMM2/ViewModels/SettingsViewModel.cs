using BSMM2.Models;
using Plugin.Clipboard;
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

		private bool _isDebugMode;
		public bool IsDebugMode
        {
			get => _isDebugMode;
			set => SetProperty(ref _isDebugMode, value, nameof(IsDebugMode), () => _app.IsDebugMode=value);
		}

		private string _mailAddress;
		public string MailAddress
		{
			get => _mailAddress;
			set => SetProperty(ref _mailAddress, value, nameof(MailAddress), () => _app.MailAddress = value);
		}

		private void SetAutoSave(bool value)
        {
			_app.AutoSave = value;
            if (++_count > 10)
				IsDebugMode = !IsDebugMode;
        }

		public ICommand ExportAppCommand { get; }
		public ICommand ImportAppCommand { get; }

		public SettingsViewModel(BSMMApp app, Action<string> displayAlert) {
			_app = app;
			AutoSave = app.AutoSave;
			MailAddress = app.MailAddress;
			IsDebugMode = app.IsDebugMode;

			ExportAppCommand = new Command(Export);
			ImportAppCommand = new Command(Import);

			void Export()
			{
                try
                {
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
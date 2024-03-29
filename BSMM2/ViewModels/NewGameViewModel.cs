﻿using BSMM2.Models;
using BSMM2.Resource;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	internal class PlayerControlConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is PlayerCreator creator && parameter is string p)
				return creator.Keyword == p;

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}

	public class PlayerCreator {
		public string Label { get; }

		public string Keyword { get; }

		public Func<Players> Create { get; }

		public PlayerCreator(string label, string keyword, Func<Players> create) {
			Label = label;
			Keyword = keyword;
			Create = create;
		}
	}

	internal class NewGameViewModel : BaseViewModel {
		private BSMMApp _app;
		public string GameName { get; set; }

		public IRule Rule
		{
			get => _app.Rule;
			set
			{
				if (_app.Rule != value)
				{
					_app.Rule = value;
					OnPropertyChanged(nameof(Rule));
				}
			}
		}
		public IEnumerable<IRule> Rules => _app.Rules;
		public int PlayerCount { get; set; }
		public string EntrySheet { get; set; }
		public bool AsCurrentGame { get; set; }

		public bool UseUniqueId4WebService {
			get=>_app.UseUniqueId4WebService;
			set=> _app.UseUniqueId4WebService=value;
		}

		private PlayerCreator _playerMode;

		public PlayerCreator PlayerMode {
			get => _playerMode;
			set => SetProperty<PlayerCreator>(ref _playerMode, value);
		}

		public IEnumerable<PlayerCreator> PlayerModes { get; }

		public ICommand CreateCommand { get; }
		public ICommand SettingPointCommand { get; }
		public event ViewActionHandler OnSettingPoint;
		public NewGameViewModel(BSMMApp app, UI ui, Action close) {
			_app = app;
			GameName = Game.GameTitle;
			PlayerModes = new[]{
				new PlayerCreator(AppResources.ItemPlayerModeNumber,"Number", CreateByNumber),
				new PlayerCreator(AppResources.ItemPlayerModeEntrySheet,"EntrySheet",CreateByEntrySheet),
				new PlayerCreator(AppResources.ItemPlayerModeRestart,"Restart", CreateByCurrent),
			};
			PlayerMode = PlayerModes.First();
			PlayerCount = 8;
			EntrySheet = app.EntryTemplate;
			AsCurrentGame = true;

			CreateCommand = new DelegateCommand(ExecuteCreate);
			SettingPointCommand = new DelegateCommand(() => OnSettingPoint.Invoke());

			void ExecuteCreate() {
				var players = PlayerMode.Create();
				if (players != null) {
					if (app.Add(new Game(players, UseUniqueId4WebService ? (Guid?)null : app.Id, GameName), AsCurrentGame)) {
						MessagingCenter.Send<object>(this, Messages.REFRESH);
					}// TODO : Error handling is required?
					close?.Invoke();
				} else {
					ui.DisplayAlert(AppResources.TextError, AppResources.TextPlayersNumberExceeded, AppResources.ButtonOK);
				}
			}

			Players CreateByNumber() {
				if (PlayerCount <= Players.MAX_COUNT) {
					return new Players(Rule, PlayerCount);
				}
				return null;
			}

			Players CreateByEntrySheet() {
				app.EntryTemplate = EntrySheet;
				using (var reader = new StringReader(EntrySheet)) {
					var players = Players.FromStream(Rule, reader).ToList();
					if (players.Count() <= Players.MAX_COUNT) {
						return new Players(Rule, players);
					}
				}
				return null;
			}

			Players CreateByCurrent()
				=> new Players(app.Game.Rule, app.Game.Players);
		}
	}
}
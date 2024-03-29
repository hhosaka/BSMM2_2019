﻿using BSMM2.Models;
using BSMM2.Resource;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	public class DelegateMatch : INotifyPropertyChanged
	{
		public Match Match { get; }
		public IRecord Record1 => Match.Record1;
		public IRecord Record2 => Match.Record2;
		public bool IsFinished => Match.IsFinished;
		public int Id { get; }

		public DelegateMatch(Match match, int id) {
			Match = match;
			Id = id;
		}

		private event PropertyChangedEventHandler _propertyChanged;
		public event PropertyChangedEventHandler PropertyChanged {
			add {
				Match.PropertyChanged += OnPropertyChanged;
				_propertyChanged += value;
			}

			remove {
				Match.PropertyChanged -= OnPropertyChanged;
				_propertyChanged -= value;
			}
		}

		private void OnPropertyChanged(object sender, PropertyChangedEventArgs e) {
			_propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Record1)));
			_propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Record2)));
			_propertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFinished)));
		}

	}
	public class RoundViewModel : BaseViewModel {
		private BSMMApp _app;

		public Game Game => _app.Game;

		private IEnumerable<DelegateMatch> _matches;

		public IEnumerable<DelegateMatch> Matches {
			get => _matches;
			set { SetProperty(ref _matches, value); }
		}

		private string _timer;

		public string Timer {
			get => _timer;
			set => SetProperty(ref _timer, value);
		}

		private bool _isTimeVisible;

		public bool IsTimerVisible {
			get => _isTimeVisible;
			set => SetProperty(ref _isTimeVisible, value);
		}

		public DelegateCommand ShuffleCommand { get; }
		public DelegateCommand StartCommand { get; }
		public DelegateCommand StepToMatchingCommand { get; }
		public DelegateCommand ShowRoundsLogCommand { get; }
		public DelegateCommand StartTimerCommand { get; }
		public DelegateCommand ShowQRCodeCommand { get; }

		private UI _ui;

		public event ViewActionHandler OnShowRoundsLog;
		public event ViewActionHandler OnShowQRCode;

		public RoundViewModel(BSMMApp app, UI ui) {
			Debug.Assert(app != null);
			_app = app;
			_ui = ui;
			ShuffleCommand = CreateShuffleCommand();
			StartCommand = CreateStepToPlayingCommand();
			StepToMatchingCommand = CreateStepToMatchingCommand();
			ShowRoundsLogCommand = new DelegateCommand(()=>OnShowRoundsLog(), () => app.Game.Rounds.Any());
			StartTimerCommand = new DelegateCommand(ExecuteStartTimer, () => Game.CanExecuteStartTimer());
			ShowQRCodeCommand = new DelegateCommand(()=>OnShowQRCode(), () => app.ActiveWebService);
			MessagingCenter.Subscribe<object>(this, Messages.REFRESH,
				async (sender) => await ExecuteRefresh());

			StartTimer();
			Refresh();

			async void ExecuteStartTimer()
            {
				Game.StartTimer();
				_app.Save(false);
				await ExecuteRefresh();
			}
		}

		private async Task ExecuteRefresh() {
			if (!IsBusy) {
				IsBusy = true;
				try {
					await Task.Run(() => Refresh());
				} finally {
					IsBusy = false;
				}
			}
		}

		private void Refresh() {
			IsTimerVisible = (Game.StartTime != null);
			int id = 0;
			Matches = Game.ActiveRound.Matches.Select(match=>new DelegateMatch(match, ++id)).ToList();
			Title = Game.Headline;
			StartCommand?.RaiseCanExecuteChanged();
			ShuffleCommand?.RaiseCanExecuteChanged();
			StepToMatchingCommand?.RaiseCanExecuteChanged();
			ShowRoundsLogCommand?.RaiseCanExecuteChanged();
			StartTimerCommand?.RaiseCanExecuteChanged();
			ShowQRCodeCommand?.RaiseCanExecuteChanged();
		}

		private DelegateCommand CreateStepToPlayingCommand() {
			return new DelegateCommand(
				Execute,
				() => Game.CanExecuteStepToPlaying());

			async void Execute() {
				Game.StepToPlaying();
				_app.Save(false);
				await ExecuteRefresh();
			}
		}

		private async Task<bool> SwitchSetting() {
			var switcher = new Switcher().Get(_app.Game);
			if (switcher != null) {
				if (await _ui.DisplayAlert(AppResources.TextAlert, AppResources.TextFailToMakeMatch, switcher.Message, AppResources.TextGoToRuleSetting)) {
					switcher.Execute(_app.Game);
					return true;
				} else {
					_ui.PushPage(_app.Game.CreateRulePage());
				}
			} else {
				_ui?.DisplayAlert(AppResources.TextAlert, AppResources.TextMatchingIncompleted, AppResources.ButtonOK);
			}
			return false;
		}

		private DelegateCommand CreateShuffleCommand() {
			return new DelegateCommand(
				Execute,
				() => Game.CanExecuteShuffle());

			async void Execute() {
				if (Game.Shuffle()) {
					_app.Save(false);
					await ExecuteRefresh();
				} else {
					await SwitchSetting();
				}
			}
		}

		private DelegateCommand CreateStepToMatchingCommand() {
			return new DelegateCommand(
				Execute,
				() => Game.CanExecuteStepToMatching());

			async void Execute() {
				if (Game.IsFinished()) {
					_ui?.DisplayAlert(AppResources.TextAlert, AppResources.TextEndGame, AppResources.ButtonOK);
				} else {
					while (!Game.StepToMatching()) {
						if (!await SwitchSetting()) {
							return;
						}
					}
					_app.Save(false);
					await ExecuteRefresh();
				}
			}
		}

		private void StartTimer() {
			Device.StartTimer(TimeSpan.FromMilliseconds(100), () => {
				if (Game.StartTime == null) {
					IsTimerVisible = false;
				} else {
					Device.BeginInvokeOnMainThread(() => {
						Timer = (DateTime.Now - Game.StartTime)?.ToString(@"hh\:mm\:ss");
					});
				}
				return true;
			});
		}
	}
}
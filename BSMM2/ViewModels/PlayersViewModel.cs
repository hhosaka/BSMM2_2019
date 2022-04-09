using BSMM2.Models;
using BSMM2.Models.WebAccess;
using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

    public class PlayersViewModel : BaseViewModel
	{
		public event ViewActionHandler OnNewGame;
		public event ViewActionHandler OnSelectGame;
		public event ViewActionHandler OnDeleteGame;
		public event ViewActionHandler OnAddPlayer;
		public event ViewActionHandler OnShowQRCode;

		private BSMMApp _app;
		private Game Game => _app.Game;

		private ObservableCollection<OrderedPlayer> _players;

		public ObservableCollection<OrderedPlayer> Players {
			get => _players;
			set { SetProperty(ref _players, value); }
		}

		public DelegateCommand NewGameCommand { get; }
		public DelegateCommand RuleCommand { get; }
		public DelegateCommand SelectGameCommand { get; }
		public DelegateCommand DeleteGameCommand { get; }
		public DelegateCommand AddPlayerCommand { get; }
		public DelegateCommand ExportPlayersCommand { get; }
		public DelegateCommand SaveCommand { get; }
		public DelegateCommand ShowQRCodeCommand { get; }

		public PlayersViewModel(BSMMApp app) {
			_app = app;
			Players = new ObservableCollection<OrderedPlayer>();

			NewGameCommand = new DelegateCommand(() => OnNewGame.Invoke());
			SelectGameCommand = new DelegateCommand(() => OnSelectGame.Invoke(), () => _app.Games.Any());
			DeleteGameCommand = new DelegateCommand(() => OnDeleteGame.Invoke(), () => _app.Games.Any());
			AddPlayerCommand = new DelegateCommand(() => OnAddPlayer.Invoke(), () => _app.Game.CanAddPlayers());
			ExportPlayersCommand = new DelegateCommand(_app.ExportPlayers);
			SaveCommand = new DelegateCommand(async () => await _app.Save(true), () => !_app.AutoSave);
			ShowQRCodeCommand = new DelegateCommand(() => OnShowQRCode.Invoke(), () => _app.ActiveWebService);
			MessagingCenter.Subscribe<object>(this, Messages.REFRESH,
				async (sender) => await ExecuteRefresh());

			Refresh();
		}

		public async Task ExecuteRefresh() {
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
			Players = new ObservableCollection<OrderedPlayer>(Game.Players.GetOrderedPlayers());
			Title = Game.Headline;
			RuleCommand?.RaiseCanExecuteChanged();
			SelectGameCommand?.RaiseCanExecuteChanged();
			DeleteGameCommand?.RaiseCanExecuteChanged();
			AddPlayerCommand?.RaiseCanExecuteChanged();
			SaveCommand?.RaiseCanExecuteChanged();
			ShowQRCodeCommand?.RaiseCanExecuteChanged();
		}
	}
}
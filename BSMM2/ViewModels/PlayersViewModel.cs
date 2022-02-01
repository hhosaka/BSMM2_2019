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

    public class PlayersViewModel : BaseViewModel {
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
		public DelegateCommand UploadPlayersCommand { get; }
		public DelegateCommand SaveCommand { get; }
		public DelegateCommand HelpCommand { get; }

		public PlayersViewModel(BSMMApp app, Action newGame = null, Action selectGame = null, Action deleteGame = null, Action addPlayer = null) {
			_app = app;
			Players = new ObservableCollection<OrderedPlayer>();

			NewGameCommand = new DelegateCommand(() => newGame?.Invoke());
			SelectGameCommand = new DelegateCommand(() => selectGame?.Invoke(), () => _app.Games.Any());
			DeleteGameCommand = new DelegateCommand(() => deleteGame?.Invoke(), () => _app.Games.Any());
			AddPlayerCommand = new DelegateCommand(() => addPlayer?.Invoke(), () => _app.Game.CanAddPlayers());
			ExportPlayersCommand = new DelegateCommand(_app.ExportPlayers);
			UploadPlayersCommand = new DelegateCommand(()=>new Client().Upload(_app.Game));
			SaveCommand = new DelegateCommand(() => _app.Save(true), () => !_app.AutoSave);

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
			Players = new ObservableCollection<OrderedPlayer>(Models.Players.GetOrderedPlayers(Game.Rule, Game.Players.GetSortedSource(Game.Rule)));
			Title = Game.Headline;
			RuleCommand?.RaiseCanExecuteChanged();
			SelectGameCommand?.RaiseCanExecuteChanged();
			DeleteGameCommand?.RaiseCanExecuteChanged();
			AddPlayerCommand?.RaiseCanExecuteChanged();
			SaveCommand?.RaiseCanExecuteChanged();
		}
	}
}
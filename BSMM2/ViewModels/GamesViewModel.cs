using BSMM2.Models;
using System;
using System.Collections.Generic;

namespace BSMM2.ViewModels {

	public class GamesViewModel : BaseViewModel {
		private BSMMApp _app;
		private Game Game => _app.Game;

		public IEnumerable<Game> Games => _app.Games;

		private Action<Game> _action;

		private Game _selectedItem;

		public Game SelectedItem {
			get => _selectedItem;
			set {
				SetProperty(ref _selectedItem, value);
				if (_selectedItem != null) {
					_action?.Invoke(_selectedItem);
				}
			}
		}

		public GamesViewModel(BSMMApp app, string title, Action<Game> action) {
			_app = app;
			SelectedItem = app.Game;
			_action = action;
			Title = title;
		}
	}
}
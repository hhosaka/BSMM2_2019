using BSMM2.Models;
using Prism.Commands;
using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	public class AddPlayerViewModel : BaseViewModel {
		private BSMMApp _app;
		private Game Game => _app.Game;

		public string Data { get; set; }

		public ICommand AddPlayerCommand { get; }

		public AddPlayerViewModel(BSMMApp app, Action exit) {
			_app = app;

			AddPlayerCommand = new DelegateCommand(AddPlayer);

			void AddPlayer() {
					_app.Game.AddPlayers(Data);
					MessagingCenter.Send<object>(this, Messages.REFRESH);
					exit();
			}
		}
	}
}
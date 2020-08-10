using BSMM2.Models;
using System.Diagnostics;

namespace BSMM2.ViewModels {

	public class PlayerViewModel : BaseViewModel {
		private BSMMApp _app;
		public Player Player { get; }
		public bool IsMatching => !_app.Game.ActiveRound.IsPlaying;

		public bool Dropped {
			get => Player.Dropped;
			set {
				if (value != Player.Dropped) {
					Debug.Assert(!_app.Game.ActiveRound.IsPlaying);
					Player.Dropped = value;
					_app.Game.Shuffle();
				}
			}
		}

		public PlayerViewModel(BSMMApp app, Player player) {
			_app = app;
			Player = player;
		}
	}
}
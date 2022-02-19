using BSMM2.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace BSMM2.ViewModels {

	public class PlayerLogResultConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
				=> "(" + RESULTUtil.ToOpponents((value as IResult).RESULT).ToString() + ")";

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}

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

		public IEnumerable<IRecord> Opponents { get; }

		public PlayerViewModel(BSMMApp app, Player player) {
			_app = app;
			Player = player;
			Opponents = app.Game.GetMatches(player).Select(match => match.GetOpponentRecord(player));
		}
	}
}
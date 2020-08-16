using BSMM2.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.SingleMatch {

	using LifePoints = IEnumerable<LifePointItem>;

	internal class SingleMatchViewModel : BaseViewModel {
		private SingleMatch _match;
		private SingleMatchRule _rule;

		public bool EnableLifePoint => _rule.EnableLifePoint;
		public ResultItem ResultItem { get; }
		public LifePointItem Player1LP { get; set; }
		public LifePointItem Player2LP { get; set; }
		public IPlayer Player1 => _match.Record1.Player;
		public IPlayer Player2 => _match.Record2.Player;

		public LifePoints LifePoints
			=> LifePointItem.Instance;

		public ICommand DoneCommand { get; }

		public SingleMatchViewModel(SingleMatchRule rule, SingleMatch match, Action back) {
			DoneCommand = new Command(Done);

			_match = match;
			_rule = rule;

			Player1LP = LifePointItem.GetItem(match.Record1.Result.LifePoint);
			Player2LP = LifePointItem.GetItem(match.Record2.Result.LifePoint);

			ResultItem = new ResultItem(match.Record1.Result.RESULT, () => OnPropertyChanged(nameof(ResultItem)));

			void Done() {
				match.SetSingleMatchResult(ResultItem.RESULT, EnableLifePoint ? Player1LP.Point : 0, EnableLifePoint ? Player2LP.Point : 0);
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				back?.Invoke();
			}
		}
	}
}
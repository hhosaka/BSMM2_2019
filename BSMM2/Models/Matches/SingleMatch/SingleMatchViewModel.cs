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
		public string Player1Name => _match.Record1.Player.Name;
		public string Player2Name => _match.Record2.Player.Name;

		public LifePoints LifePoints
			=> LifePointItem.Instance;

		public ICommand DoneCommand { get; }

		public SingleMatchViewModel(SingleMatchRule rule, SingleMatch match, Action back) {
			DoneCommand = new Command(Done);

			_match = match;
			_rule = rule;

			ResultItem = new ResultItem(EnableLifePoint, match.Record1.Result, match.Record2.Result,() => OnPropertyChanged(nameof(ResultItem)));

			void Done() {
				match.SetSingleMatchResult(ResultItem.RESULT, ResultItem.LifePoint1, ResultItem.LifePoint2);
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				back?.Invoke();
			}
		}
	}
}
using BSMM2.Resource;
using BSMM2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace BSMM2.Models.Matches.SingleMatch {

	public class SingleMatchSimpleViewModel : BaseViewModel {
		private SingleMatch _match;
		private Action _back;

		public IEnumerable<KeyValuePair<string,RESULT_T>> RESULTs { get; }

		private KeyValuePair<string, RESULT_T> _result;
		public KeyValuePair<string, RESULT_T> RESULT {
			get => _result;
			set {
				_match.SetSingleMatchResult(value.Value, null, null);
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				_back?.Invoke();
			}
		}

		public SingleMatchSimpleViewModel(SingleMatchRule rule, SingleMatch match, Action back) {

			_match = match;
			_back = back;

			RESULTs = new[] {
				new KeyValuePair<string,RESULT_T>(string.Format(AppResources.FormatSingleMatchLabel,match.Record1.Player.Name),RESULT_T.Win),
				new KeyValuePair<string,RESULT_T>(AppResources.ItemResultDraw,RESULT_T.Draw),
				new KeyValuePair<string,RESULT_T>(string.Format(AppResources.FormatSingleMatchLabel,match.Record2.Player.Name),RESULT_T.Lose),
			};
		}
	}
}
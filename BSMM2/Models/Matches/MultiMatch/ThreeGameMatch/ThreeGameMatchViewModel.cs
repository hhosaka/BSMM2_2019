using BSMM2.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static BSMM2.Models.Matches.MultiMatch.MultiMatch;

namespace BSMM2.Models.Matches.MultiMatch.ThreeGameMatch {

	internal class TheConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value is ResultItem[] resultItems) {
				switch (parameter.ToString()) {
					case "1":
						return !(resultItems[0].RESULT == RESULT_T.Progress && resultItems[1].RESULT == RESULT_T.Progress && resultItems[2].RESULT == RESULT_T.Progress);

					case "2":
						return !(resultItems[1].RESULT == RESULT_T.Progress && resultItems[2].RESULT == RESULT_T.Progress);
				}
			}

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}

	internal class ThreeGameMatchViewModel : BaseViewModel {
		private MultiMatch _match;
		private ThreeGameMatchRule _rule;

		public bool EnableLifePoint => _rule.EnableLifePoint;

		public IEnumerable<LifePointItem> LifePointItems
			=> LifePointItem.Instance;

		public ResultItem[] ResultItems { get; }

		public IPlayer Player1 => _match.Record1.Player;
		public IPlayer Player2 => _match.Record2.Player;

		public ICommand DoneCommand { get; }

		public ThreeGameMatchViewModel(ThreeGameMatchRule rule, MultiMatch match, Action back) {
			_match = match;
			_rule = rule;

			ResultItems = CreateItems(match.Record1.Result as MultiMatchResult).ToArray();

			DoneCommand = new Command(Done);

			IEnumerable<ResultItem> CreateItems(MultiMatchResult result) {

				var e1 = (match.Record1.Result as MultiMatchResult)?.Results.GetEnumerator() ?? Enumerable.Empty<SingleMatch.SingleMatchResult>().GetEnumerator();
				var e2 = (match.Record2.Result as MultiMatchResult)?.Results.GetEnumerator() ?? Enumerable.Empty<SingleMatch.SingleMatchResult>().GetEnumerator();
				for (int i=0; i<3; ++i)
                {
					yield return new ResultItem(
						e1.MoveNext() ? e1.Current : null,
						e2.MoveNext() ? e2.Current : null,
						() => OnPropertyChanged(nameof(ResultItems)));
				}
			}

			void Done() {

				match.SetMultiMatchResult(Create());
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				back?.Invoke();

                IEnumerable<Score> Create()
                {
                    for (int i = 0; i < 3; ++i)
                    {
                        var item = ResultItems[i];
                        if (item.RESULT != RESULT_T.Progress)
                            yield return new Score(item.RESULT,
								EnableLifePoint ? ResultItems[i].LifePoint[0].Point : 0,
								EnableLifePoint ? ResultItems[i].LifePoint[1].Point : 0);
                    }

                }
            }
		}
	}
}
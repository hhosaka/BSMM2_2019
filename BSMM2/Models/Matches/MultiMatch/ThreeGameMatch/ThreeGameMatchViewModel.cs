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
						return resultItems[0].IsFinished(false);

					case "2":
						return resultItems[0].IsFinished(false) && resultItems[1].IsFinished(false);
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

				match.SetMultiMatchResult(CreateScores());
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				back?.Invoke();

                IEnumerable<IScore> CreateScores()
                {
					foreach(var item in ResultItems)
                    {
						if (item.IsEmpty) break;
						yield return item;
						if (!item.IsFinished(false)) break;
					}
                }
            }
		}
	}
}
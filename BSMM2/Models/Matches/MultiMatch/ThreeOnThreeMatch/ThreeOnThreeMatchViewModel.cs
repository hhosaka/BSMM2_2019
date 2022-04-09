using BSMM2.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using static BSMM2.Models.Matches.MultiMatch.MultiMatch;

namespace BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch {

	internal class ThreeOnThreeMatchViewModel : BaseViewModel {
		private MultiMatch _match;
		private ThreeOnThreeMatchRule _rule;

		public IEnumerable<LifePointItem> LifePoints
			=> LifePointItem.Instance;

		public bool EnableLifePoint => _rule.EnableLifePoint;
		public ResultItem[] ResultItems { get; }
		public string Player1Name => _match.Record1.Player.Name;
		public string Player2Name => _match.Record2.Player.Name;

		public ICommand DoneCommand { get; }

		public ThreeOnThreeMatchViewModel(ThreeOnThreeMatchRule rule, MultiMatch match, Action back) {
			DoneCommand = new Command(Done);

			_match = match;
			_rule = rule;

			ResultItems = CreateItems(match.Record1.Result as MultiMatchResult).ToArray();

			IEnumerable<ResultItem> CreateItems(MultiMatchResult result) {

				var e1 = (match.Record1.Result as MultiMatchResult)?.Results.GetEnumerator() ?? Enumerable.Empty<SingleMatch.SingleMatchResult>().GetEnumerator();
				var e2 = (match.Record2.Result as MultiMatchResult)?.Results.GetEnumerator() ?? Enumerable.Empty<SingleMatch.SingleMatchResult>().GetEnumerator();
				for (int i = 0; i < 3; ++i) {
					yield return new ResultItem(
						EnableLifePoint,
						e1.MoveNext() ? e1.Current : null,
						e2.MoveNext() ? e2.Current : null,
						() => OnPropertyChanged(nameof(ResultItems)));
				}
			}

			void Done() {

				match.SetMultiMatchResult(rule, ResultItems);
				MessagingCenter.Send<object>(this, Messages.REFRESH);
				back?.Invoke();

			}
		}
	}
}
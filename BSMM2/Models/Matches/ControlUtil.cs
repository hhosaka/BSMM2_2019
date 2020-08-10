using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BSMM2.Models.Matches {

	using LIFEPOINTITEM_T = System.Collections.Generic.KeyValuePair<string, int>;
	using LifePoints = IEnumerable<LifePoint>;
	using RESULTITEM_T = System.Collections.Generic.KeyValuePair<string, RESULT_T>;

	internal class LifePoint {
		private static LifePoints _instance;
		private static LifePoint _null = new LifePoint();

		public string Label { get; private set; }
		public int Point { get; private set; }

		public static LifePoints Instance => GetInstance();

		private static LifePoints GetInstance() {
			if (_instance == null) {
				_instance = new List<LifePoint> {
					new LifePoint{ Label = "-", Point = -1},
					new LifePoint{ Label = "5", Point = 5},
					new LifePoint{ Label = "4", Point = 4},
					new LifePoint{ Label = "3", Point = 3},
					new LifePoint{ Label = "2", Point = 2},
					new LifePoint{ Label = "1", Point = 1},
					new LifePoint{ Label = "0", Point = 0},
				};
			}
			return _instance;
		}

		public static LifePoint GetItem(int point) {
			return GetInstance().First(lp => lp.Point == point);
		}
	}

	internal class ResultItem {
		private Action _onPropertyChanged;

		public RESULT_T RESULT { get; private set; }

		public ResultItem(RESULT_T result, Action onPropertyChanged) {
			Initial(result);
			_onPropertyChanged = onPropertyChanged;
		}

		public bool Player1Win {
			get => RESULT == RESULT_T.Win;
			set {
				if (value != Player1Win) {
					if (value) {
						RESULT = RESULT_T.Win;
					} else {
						RESULT = RESULT_T.Progress;
					}
					Draw = Player2Win = false;
					_onPropertyChanged?.Invoke();
				}
			}
		}

		public bool Draw {
			get => RESULT == RESULT_T.Draw;
			set {
				if (value != Draw) {
					if (value) {
						RESULT = RESULT_T.Draw;
					} else {
						RESULT = RESULT_T.Progress;
					}
					Player1Win = Player2Win = false;
					_onPropertyChanged?.Invoke();
				}
			}
		}

		public bool Player2Win {
			get => RESULT == RESULT_T.Lose;
			set {
				if (value != Player2Win) {
					if (value) {
						RESULT = RESULT_T.Lose;
					} else {
						RESULT = RESULT_T.Progress;
					}
					Draw = Player1Win = false;
					_onPropertyChanged?.Invoke();
				}
			}
		}

		private void Initial(RESULT_T result) {
			RESULT = RESULT_T.Progress;
			switch (result) {
				case RESULT_T.Win:
					Player1Win = true;
					break;

				case RESULT_T.Draw:
					Draw = true;
					break;

				case RESULT_T.Lose:
					Player2Win = true;
					break;
			}
		}
	}

	internal class ControlUtil {

		public static ObservableCollection<RESULTITEM_T> CreateResultSelector(string player1, string player2) {
			var items = new ObservableCollection<RESULTITEM_T>();
			items.Add(new RESULTITEM_T(player1 + " Win", RESULT_T.Win));
			items.Add(new RESULTITEM_T("Draw", RESULT_T.Draw));
			items.Add(new RESULTITEM_T(player2 + " Win", RESULT_T.Lose));
			return items;
		}

		public static ObservableCollection<LIFEPOINTITEM_T> CreateLifePointItems() {
			var items = new ObservableCollection<LIFEPOINTITEM_T>();
			items.Add(new LIFEPOINTITEM_T("Undefined", -1));
			for (int i = 5; i >= 0; --i) {
				items.Add(new LIFEPOINTITEM_T(i.ToString(), i));
			}
			return items;
		}
	}
}
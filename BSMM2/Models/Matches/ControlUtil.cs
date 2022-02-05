using BSMM2.Models.Matches.SingleMatch;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace BSMM2.Models.Matches {
    using static BSMM2.Models.Matches.MultiMatch.MultiMatch;
    using LIFEPOINTITEM_T = System.Collections.Generic.KeyValuePair<string, int>;
	using LifePoints = IEnumerable<LifePointItem>;
	using RESULTITEM_T = System.Collections.Generic.KeyValuePair<string, RESULT_T>;

	internal class LifePointItem {
		private static LifePoints _instance;

		public string Label { get; private set; }
		public int Point { get; private set; }
		public static LifePoints Instance => GetInstance();

		private static LifePoints GetInstance() {
			if (_instance == null) {
				_instance = new List<LifePointItem> {
					new LifePointItem{ Label = "-", Point = -1},
					new LifePointItem{ Label = "5", Point = 5},
					new LifePointItem{ Label = "4", Point = 4},
					new LifePointItem{ Label = "3", Point = 3},
					new LifePointItem{ Label = "2", Point = 2},
					new LifePointItem{ Label = "1", Point = 1},
					new LifePointItem{ Label = "0", Point = 0},
				};
			}
			return _instance;
		}

		public static LifePointItem GetItem(double? lifePoint)
			=> GetInstance().First(lp => lp.Point == (lifePoint ?? -1));

	}

	internal class LifePointItems {
		private Action _onPropertyChanged;
		private LifePointItem[] _items;
		public LifePointItem this[int index] {
			get => _items[index];
			set {
				_items[index] = value;
				_onPropertyChanged?.Invoke();
			}
		}
		public LifePointItems(Action onPropertyChanged) {
			_onPropertyChanged=onPropertyChanged;
			_items = new LifePointItem[2];
		}
	}

	internal class ResultItem : IScore{

		private Action _onPropertyChanged;

		public LifePointItems LifePoints { get; }

		public RESULT_T RESULT { get; private set; }

		public bool IsFinished(bool AcceptDraw)
			=> (RESULT == RESULT_T.Win || RESULT == RESULT_T.Lose || (AcceptDraw && RESULT == RESULT_T.Draw)) && LifePoint1>=0 && LifePoint2 >= 0;

		public bool IsEmpty => RESULT == RESULT_T.Progress && (LifePoints==null || (LifePoint1 == -1 && LifePoint2 == -1));

		public ResultItem(bool enableLifePoint, IResult result1, IResult result2, Action onPropertyChanged)
		{
			InitialRESULT(result1?.RESULT??RESULT_T.Progress);
			if (enableLifePoint) {
				LifePoints = new LifePointItems(onPropertyChanged);
				LifePoints[0] = Matches.LifePointItem.GetItem(result1?.LifePoint);
				LifePoints[1] = Matches.LifePointItem.GetItem(result2?.LifePoint);
			}
			_onPropertyChanged = onPropertyChanged;

			void InitialRESULT(RESULT_T result) {
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

		public int LifePoint1 => LifePoints?[0].Point ?? 0;

		public int LifePoint2 => LifePoints?[1].Point ?? 0;
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

		public static string DecorateNumber(int num) {
			switch (num) {
				case 1: return "1st";
				case 2: return "2nd";
				case 3: return "3rd";
				default: return num + "th";
			}
		}
	}
}
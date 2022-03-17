using BSMM2.Models;
using BSMM2.Models.Matches;
using BSMM2.Resource;
using System;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	public class RoundResultConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			switch (parameter) {
				case "BGCOLOR":
					return (bool)value ? Color.Aqua : Color.White;

				case "ID":
					return ControlUtil.DecorateNumber((int)value);

				case "IS_GAP_MATCH":
					return (bool)value ? "[GAP]":"";

				case "RESULTMARK":
					return "(" + (value as IRecord).Result.RESULT.ToString() + ")";

				default:
					throw new ArgumentException();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}

	public class PlayerResultConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			switch (parameter) {
				case "DESCRIPTION": {
						if (value is IPoint point) {
							var buf = new StringBuilder();
							buf.Append(AppResources.TextMatchPoint);
							buf.Append(" = ");
							buf.Append(point.MatchPoint);
							buf.Append("/ ");
							buf.Append(AppResources.TextWinPoint);
							buf.Append(" = ");
							buf.Append(string.Format("{0:F}", point.WinPoint));
							if (point.LifePoint != null) {
								buf.Append("/ ");
								buf.Append(AppResources.TextLifePoint);
								buf.Append(" = ");
								buf.Append(point.LifePoint >= 0 ? point.LifePoint.ToString() : "-");
							}
							return buf.ToString();
						}
						return "---";
					}
				default:
					throw new ArgumentException();
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
			=> throw new NotImplementedException();
	}
}
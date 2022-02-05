using BSMM2.Models;
using BSMM2.Models.Matches;
using System;
using System.Globalization;
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
}
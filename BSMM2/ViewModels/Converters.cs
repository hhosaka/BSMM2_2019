using BSMM2.Models;
using System;
using System.Globalization;
using Xamarin.Forms;

namespace BSMM2.ViewModels {

	public class RoundResultConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			switch (parameter) {
				case "BGCOLOR":
					return (bool)value ? Color.Aqua : Color.White;

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
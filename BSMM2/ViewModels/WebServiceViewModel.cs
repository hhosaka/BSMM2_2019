using BSMM2.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace BSMM2.ViewModels
{
	public class WebServiceViewModel {
		public ImageSource ImageSource { get; }
		public string URL { get; }
		public Command Command { get; }

		public WebServiceViewModel(BSMMApp app, string serviceCode) {
			URL = BSMMApp.WebURL + serviceCode + app.Game.WebServiceId;
			ImageSource = ImageSource.FromUri(
				new System.Uri($"https://chart.googleapis.com/chart?cht=qr&chl={System.Web.HttpUtility.UrlEncode(URL)}&chs=300x300&chld=H|1"));
		}
	}
}

using BSMM2.Models;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace BSMM2.ViewModels
{
	public class WebViewModel {
		public WebViewSource Source { get; }

		public WebViewModel(string url) {
			Source = new UrlWebViewSource() { Url= url };
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BSMM2.ViewModels
{
	public interface UI
	{
		Task DisplayAlert(string title, string message, string ok);
		Task<bool> DisplayAlert(string title, string message, string ok, string cancel);

		void PushPage(Page page);
	};
}

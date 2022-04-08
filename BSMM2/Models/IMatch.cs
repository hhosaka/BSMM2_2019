using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	public interface IMatch {

		int Id { get; }

		bool IsGapMatch { get; }

		bool IsFinished { get; }

		bool IsByeMatch { get; }

		//public IEnumerable<string> PlayerNames
		//	=> _records.Select(result => result.Player.Name);

		IRecord Record1 { get; }

		IRecord Record2 { get; }
	}
}
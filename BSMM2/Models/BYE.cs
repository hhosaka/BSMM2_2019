using BSMM2.Resource;
using Newtonsoft.Json;

namespace BSMM2.Models {

	[JsonObject]
	internal class BYE : IPlayer {

		[JsonIgnore]
		public string Name => AppResources.TextBYE;
	}
}
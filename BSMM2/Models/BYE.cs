using BSMM2.Resource;
using Newtonsoft.Json;

namespace BSMM2.Models
{

	[JsonObject]
	internal class BYE : Player
	{
		private static Player _instance;
		public const int Id = -1;
		public static Player Instance
			=> _instance ??(_instance = new BYE());

		private BYE()
			:base(null, AppResources.TextBYE) {
		}
	}
}
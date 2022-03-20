using Newtonsoft.Json;

namespace BSMM2.Models {

	public interface IPoint : IExportableObject {
		int MatchPoint { get; }

		double WinPoint { get; }

		double?LifePoint { get; }
	}
}
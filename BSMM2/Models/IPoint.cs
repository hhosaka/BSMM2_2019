using Newtonsoft.Json;

namespace BSMM2.Models {

	public interface IPoint : IExportable {
		int MatchPoint { get; }

		double WinPoint { get; }

		double LifePoint { get; }
	}
}
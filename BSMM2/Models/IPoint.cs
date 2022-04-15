using Newtonsoft.Json;

namespace BSMM2.Models {

	public interface IPoint {
		int MatchPoint { get; }

		double WinPoint { get; }

		double?LifePoint { get; }
	}

	public interface IExportablePoint : IPoint, IExportableObject
	{
	}
}
namespace BSMM2.Models {

	public interface IPoint : IExportable {
		int MatchPoint { get; }

		double WinPoint { get; }

		int LifePoint { get; }
	}
}
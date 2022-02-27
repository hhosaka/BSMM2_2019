namespace BSMM2.Models {

	public interface IRecord {
		Player Player { get; }

		IResult Result { get; }
	}
}
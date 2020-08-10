namespace BSMM2.Models {

	public interface IRecord {
		IPlayer Player { get; }

		IResult Result { get; }
	}
}
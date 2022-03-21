namespace BSMM2.Models {

	public interface IResult {
		RESULT_T RESULT { get; }
		IPoint Point { get; }

		bool IsFinished { get; }
	}
}
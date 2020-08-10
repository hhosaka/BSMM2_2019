namespace BSMM2.Models {

	public interface IResult : IPoint {
		RESULT_T RESULT { get; }

		bool IsFinished { get; }
	}
}
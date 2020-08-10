namespace BSMM2.Models {

	public enum RESULT_T { Win, Lose, Draw, Progress }

	internal class RESULTUtil {
		public const int DEFAULT_LIFE_POINT = 5;

		public static RESULT_T ToOpponents(RESULT_T result) {
			switch (result) {
				case RESULT_T.Win:
					return RESULT_T.Lose;

				case RESULT_T.Lose:
					return RESULT_T.Win;
			}
			return result;
		}
	}
}
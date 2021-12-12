namespace BSMM2.Models {

	public enum LEVEL { Mandatory, Required, Option };

	public interface IComparer {
		string Label { get; }

		bool Selectable { get; }

		LEVEL Level { get; }		

		bool Active { get; set; }

		int Compare(Player p1, Player p2);
	}
}
using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BSMM2.Models {

	[JsonObject]
	public interface IRule {
		[JsonProperty]
		IEnumerable<IComparer> Comparers { get; }

		[JsonProperty]
		string Name { get; }

		[JsonProperty]
		string Description { get; }

		[JsonProperty]
		string Prefix { get; set; }

		[JsonProperty]
		PointRule PointRule { get; }

		Match CreateMatch(Player player1, Player player2 = null);

		ContentPage CreateMatchPage(Match match);

		ContentPage CreateRulePage(Game game);

		IRule Clone();

		IExportablePoint Point(IEnumerable<IPoint> results);

		Comparer<Player> GetComparer(bool force);
	}
}
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
		bool EnableLifePoint { get; }

		[JsonProperty]
		int DrawMatchPoint { get; }

		[JsonProperty]
		double DrawWinPoint { get; }

		Match CreateMatch(Player player1, Player player2 = null);

		ContentPage CreateMatchPage(Match match);

		ContentPage CreateRulePage(Game game);

		IRule Clone();

		IPoint Point(IEnumerable<IPoint> results);

		Comparer<Player> GetComparer(bool force);
	}
}
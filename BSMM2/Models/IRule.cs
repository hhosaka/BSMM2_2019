using Newtonsoft.Json;
using System.Collections.Generic;
using Xamarin.Forms;

namespace BSMM2.Models {

	[JsonObject]
	public interface IRule {
		IEnumerable<IComparer> Comparers { get; }

		string Name { get; }

		string Description { get; }

		string Prefix { get; set; }

		Match CreateMatch(int id, IPlayer player1, IPlayer player2 = null);

		ContentPage CreateMatchPage(Match match);

		ContentPage CreateRulePage(Game game);

		IRule Clone();

		string GetDescription(Player player);

		IPoint Point(IEnumerable<IPoint> results);

		Comparer<Player> GetComparer(bool force);
	}
}
using BSMM2.Resource;
using BSMM2.ViewModels;

namespace BSMM2.Models.Matches.SingleMatch {

	internal class SingleMatchRuleViewModel : BaseViewModel {
		public Game Game { get; }

		public IRule Rule { get; }

		public SingleMatchRuleViewModel(Game game) {
			Game = game;
			Rule = game.Rule;
			Title = AppResources.LabelRuleSetting + " - " + Rule.Name;
		}
	}
}
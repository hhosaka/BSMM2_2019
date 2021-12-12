using BSMM2.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSMM2.Models
{
	public interface ISwitcher
	{
		string Message { get; }
		bool IsTarget(Game game);
		void Execute(Game game);
	}

	public class OmitDetailSwitcher : ISwitcher
	{
		public string Message => AppResources.TextOmitDetailComparison;
		public bool IsTarget(Game game)
			=> game.Rule.Comparers.Any(c => c.Level == LEVEL.Option && c.Active);
		public void Execute(Game game) {
			foreach (var comparer in game.Rule.Comparers.Where(c => c.Level == LEVEL.Option)) {
				comparer.Active = false;
			}
		}
	}

	public class OmitLoosersGapMatchDuplication : ISwitcher
	{
		public string Message => AppResources.TextOmitLoosersGapMatchDuplication;
		public bool IsTarget(Game game)
			=> !game.AcceptLosersGapMatchDuplication;
		public void Execute(Game game)
			=> game.AcceptLosersGapMatchDuplication = true;
	}

	public class OmitGapMatchDuplication : ISwitcher
	{
		public string Message => AppResources.TextOmitGapMatchDuplication;
		public bool IsTarget(Game game)
			=> !game.AcceptGapMatchDuplication;
		public void Execute(Game game)
			=> game.AcceptGapMatchDuplication = true;
	}

	public class OmitByeMatchDuplication : ISwitcher
	{
		public string Message => AppResources.TextOmitByeMatchDuplication;
		public bool IsTarget(Game game)
			=> !game.AcceptByeMatchDuplication;
		public void Execute(Game game)
			=> game.AcceptByeMatchDuplication = true;
	}

	public class ConciderOnlyWinner : ISwitcher
	{
		public string Message => AppResources.TextWinnerOnly;
		public bool IsTarget(Game game)
			=> game.Rule.Comparers.Any(c => c.Level == LEVEL.Required && c.Active);
		public void Execute(Game game) {
			foreach (var comparer in game.Rule.Comparers.Where(c => c.Level == LEVEL.Required)) {
				comparer.Active = false;
			}
		}
	}
	public class Switcher
	{
		private ISwitcher[] _switchers = new ISwitcher[] {
				new OmitDetailSwitcher(),
				new OmitLoosersGapMatchDuplication(),
				new OmitGapMatchDuplication(),
				new OmitByeMatchDuplication(),
				new ConciderOnlyWinner()};

		public ISwitcher Get(Game game)
			=> _switchers.FirstOrDefault(s => s.IsTarget(game));
	}

}

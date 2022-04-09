using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace BSMM2.Models {

	[JsonObject]
	public class Round {

		[JsonProperty]
		public List<Match> _matches;

		[JsonIgnore]
		public IEnumerable<Match> Matches => _matches;

		[JsonProperty]
		public bool IsPlaying { get; private set; }

		[JsonIgnore]
		public bool IsFinished
			=> !_matches.Any(match => !match.IsFinished);

		public bool Swap(int m1, int m2)
			=> Swap(_matches.ElementAt(m1), _matches.ElementAt(m2));

		public bool Swap(Match m1, Match m2) {
			if (!IsPlaying) {
				m1.Swap(m2);
				return true;
			}
			return false;
		}

		public IEnumerable<Match> StepToPlaying(IRule rule) {
			IsPlaying = true;
			foreach (var m in Matches) {
				if (m.IsByeMatch)
					m.SetResult(rule, RESULT_T.Win);
			}
			return _matches;
		}

		public Round() {
		}

		public Round(IEnumerable<Match> matches) {
			_matches = new List<Match>(matches);
		}
	}
}
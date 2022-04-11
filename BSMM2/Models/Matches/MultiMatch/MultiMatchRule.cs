using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;

namespace BSMM2.Models.Matches.MultiMatch {

	[JsonObject]
	public abstract class MultiMatchRule : SingleMatchRule {

		protected MultiMatchRule() {
		}
		protected MultiMatchRule(PointRule pointRule):base(pointRule)
		{
		}

		protected MultiMatchRule(SingleMatchRule rule) : this(rule.PointRule) {
		}
	}
}
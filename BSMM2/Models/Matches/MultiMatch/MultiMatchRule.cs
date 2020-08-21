using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;

namespace BSMM2.Models.Matches.MultiMatch {

	[JsonObject]
	public abstract class MultiMatchRule : SingleMatchRule {
		protected MultiMatchRule() {
		}

		protected MultiMatchRule(bool enableLifePoint=false):base(enableLifePoint)
		{
		}

		protected MultiMatchRule(SingleMatchRule rule) : base(rule) {
		}
	}
}
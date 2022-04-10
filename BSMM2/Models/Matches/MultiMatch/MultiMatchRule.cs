using BSMM2.Models.Matches.SingleMatch;
using Newtonsoft.Json;

namespace BSMM2.Models.Matches.MultiMatch {

	[JsonObject]
	public abstract class MultiMatchRule : SingleMatchRule {

		protected MultiMatchRule(bool enableLifePoint=false,int drawMatchPoint=1,double drawWinPoint=0.5):base(enableLifePoint, drawMatchPoint,drawWinPoint)
		{
		}

		protected MultiMatchRule(SingleMatchRule rule) : base(rule) {
		}
	}
}
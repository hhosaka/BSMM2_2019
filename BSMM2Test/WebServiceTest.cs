using System;
using System.Collections.Generic;
using System.Text;
using BSMM2.Models;
using BSMM2.Models.Matches.MultiMatch.NthGameMatch;
using BSMM2.Models.Matches.MultiMatch.ThreeOnThreeMatch;
using BSMM2.Models.Matches.SingleMatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using static BSMM2.Models.RESULT_T;
using System.Linq;
using System.IO;
using Newtonsoft.Json;

namespace BSMM2Test
{
	[TestClass]
	public class WebServiceTest {
		[TestMethod]
		public void WebServiceTest1() {
			// TBD
			var game = new FakeGame(new SingleMatchRule(), 6);
		}
	}
}

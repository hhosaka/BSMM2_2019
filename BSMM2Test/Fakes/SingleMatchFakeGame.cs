using BSMM2.Models;
using BSMM2.Models.Matches.SingleMatch;
using System;
using System.Collections.Generic;
using System.IO;

namespace BSMM2Test {

	public class SingleMatchFakeGame : FakeGame {

		public SingleMatchFakeGame() {
		}

		public SingleMatchFakeGame(int count)
			: base(new SingleMatchRule(), count) {
		}

		public SingleMatchFakeGame(TextReader r)
			: base(new SingleMatchRule(), r) {
		}

		public SingleMatchFakeGame(Players players)
			: base(new SingleMatchRule(),  players) {
		}
	}
}
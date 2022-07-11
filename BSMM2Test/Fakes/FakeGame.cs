using BSMM2.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace BSMM2Test {

	public class FakeGame : Game {

		public FakeGame() {
		}

		public FakeGame(IRule rule, int count)
			: base(new Players(rule, count), null, DateTime.Now.ToString(),p => p) {
		}

		public FakeGame(IRule rule, TextReader r)
			: base(new Players(rule, Players.FromStream(rule, r)), null, DateTime.Now.ToString(), p => p) {
		}

		public FakeGame(IRule rule, Players players)
			: base(new Players(rule, players), null, DateTime.Now.ToString(), p => p) {
		}
	}
}
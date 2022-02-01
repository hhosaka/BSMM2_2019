using BSMM2.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace BSMM2Test {

	public class FakeGame : Game {

		public FakeGame() {
		}

		public FakeGame(IRule rule, int count)
			: base(rule, new Players(rule, count), DateTime.Now.ToString(),p => p) {
		}

		public FakeGame(IRule rule, TextReader r)
			: base(rule, new Players(rule, r), DateTime.Now.ToString(), p => p) {
		}

		public FakeGame(IRule rule, Players players)
			: base(rule, new Players(rule, players), DateTime.Now.ToString(), p => p) {
		}
	}
}
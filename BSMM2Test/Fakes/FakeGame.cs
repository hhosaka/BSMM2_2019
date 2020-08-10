using BSMM2.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace BSMM2Test {

	public class FakeGame : Game {

		protected override IEnumerable<Player> RandomizePlayer(IEnumerable<Player> players)
			=> players;

		public FakeGame() {
		}

		public FakeGame(IRule rule, int count, string prefix = "Player")
			: base(rule, new Players(rule, count, prefix), DateTime.Now.ToString()) {
		}

		public FakeGame(IRule rule, TextReader r)
			: base(rule, new Players(rule, r), DateTime.Now.ToString()) {
		}

		public FakeGame(IRule rule, Players players)
			: base(rule, new Players(players), DateTime.Now.ToString()) {
		}
	}
}
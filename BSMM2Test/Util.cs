using BSMM2.Models;
using BSMM2.Models.Matches.SingleMatch;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace BSMM2Test {

	internal class Util {
		public static readonly string DefaultOrigin = "Player";

		public static int ConvId(string origin, string name) {
			if (!name.StartsWith(origin)) {
				return -1;
			} else {
				Assert.AreEqual(0, name.IndexOf(origin));
				return int.Parse(name.Substring(name.Length - 3));
			}
		}

		public static void Check(IEnumerable<int> expect, IEnumerable<Player> players) {
			var result = players.Select(player => ConvId(DefaultOrigin, player.Name));
			CollectionAssert.AreEqual(expect.ToArray(), result.ToArray(), Message(expect, result));
		}

		public static void CheckWithOrder(IRule rule, IEnumerable<int> expectedPlayers, IEnumerable<int> expectedOrder, IEnumerable<Player> players) {
			Check(expectedPlayers, players);
			CheckOrder(rule, expectedOrder, players);
		}

		public static void CheckOrder(IRule rule, IEnumerable<int> expectedOrder, IEnumerable<Player> players) {
			var result = Players.GetOrderedPlayers(rule, players).Select(player => player.Order);
			CollectionAssert.AreEqual(expectedOrder.ToArray(), result.ToArray(), Message(expectedOrder, result));
		}

		public static void Check(IEnumerable<int> expect, Round round) {
			Check(expect, DefaultOrigin, round);
		}

		public static void Check(IEnumerable<int> expect, string origin, Round round) {
			var result = round.Matches.SelectMany(match => match.PlayerNames.Select(name => ConvId(origin, name)));
			CollectionAssert.AreEqual(expect.ToArray(), result.ToArray(), Message(expect, result));
		}

		public static void Check(IResult a, IResult b) {
			if (a == null && b == null) return;//どちらも未設定
			Assert.IsNotNull(a);
			Assert.IsNotNull(b);
			Assert.AreEqual(a.IsFinished, b.IsFinished);
			Assert.AreEqual(a.MatchPoint, b.MatchPoint);
			Assert.AreEqual(a.WinPoint, b.WinPoint);
			Assert.AreEqual(a.LifePoint, b.LifePoint);
		}

		public static void Check(IPoint a, IPoint b) {
			Assert.AreEqual(a.MatchPoint, b.MatchPoint);
			Assert.AreEqual(a.LifePoint, b.LifePoint);
			Assert.AreEqual(a.WinPoint, b.WinPoint);
		}

		public static void Check(Game ga, Player a, Game gb, Player b) {
			if (a is Player pa && b is Player pb) {
				Assert.AreEqual(pa.Dropped, pb.Dropped);
				Assert.AreEqual(pa.ByeMatchCount(), pb.ByeMatchCount());
				Assert.AreEqual(pa.HasGapMatch(), pb.HasGapMatch());
				Check(pa.Point, pb.Point);
			}
			Assert.AreEqual(a.Name, b.Name);
		}

		public static void Check(Game ga, Match a, Game gb, Match b) {
			Check(ga, a.Record1.Player, gb, b.Record1.Player);
			Check(ga, a.Record2.Player, gb, b.Record2.Player);
			Check(a.Record1.Result, b.Record1.Result);
			Check(a.Record2.Result, b.Record2.Result);
		}

		public static void Check(Game ga, Round a, Game gb, Round b) {
			Assert.AreEqual(a.IsFinished, b.IsFinished);
			Assert.AreEqual(a.IsPlaying, b.IsPlaying);
			Check(ga, a.Matches, gb, b.Matches);
		}

		public static void Check(Game ga, IEnumerable<Match> a, Game gb, IEnumerable<Match> b) {
			var ita = a.GetEnumerator();
			var itb = b.GetEnumerator();
			while (ita.MoveNext() && itb.MoveNext()) {
				Check(ga, ita.Current as Match, gb, itb.Current as Match);
			}
		}

		public static void Check(Game a, Game b) {
			Assert.AreEqual(a.Title, b.Title);//TODO
			Assert.AreEqual(a.Id, b.Id);
			Assert.AreEqual(a.StartTime, b.StartTime);
			Assert.AreEqual(a.Rule.Name, b.Rule.Name);
			Assert.AreEqual(a.AcceptByeMatchDuplication, b.AcceptByeMatchDuplication);
			Assert.AreEqual(a.AcceptGapMatchDuplication, b.AcceptGapMatchDuplication);
			Assert.AreEqual(a.AcceptLosersGapMatchDuplication, b.AcceptLosersGapMatchDuplication);
			if (a.Rule is SingleMatchRule srule) {
				Assert.AreEqual(srule.EnableLifePoint, srule.EnableLifePoint);
			}
			Assert.AreEqual(a.Players.Count, b.Players.Count);
			var ita = a.GetSortedSource().GetEnumerator();
			var itb = b.GetSortedSource().GetEnumerator();
			while (ita.MoveNext() && itb.MoveNext()) {
				Check(a, ita.Current, b, itb.Current);
			}
			Check(a, a.ActiveRound, b, b.ActiveRound);
			CheckRounds();

			void CheckRounds() {
				var it1 = a.Rounds.GetEnumerator();
				var it2 = b.Rounds.GetEnumerator();
				while (it1.MoveNext() && it2.MoveNext()) {
					Check(a, it1.Current as Round, b, it2.Current as Round);
				}
			}
		}

		private static String Message(IEnumerable<int> expect, IEnumerable<int> result) {
			return "result=" + ToString(result) + " expected=" + ToString(expect);

			String ToString(IEnumerable<int> array) {
				var builder = new StringBuilder();
				builder.Append("{");
				array.ToList().ForEach(i => builder.Append(i.ToString() + ","));
				builder.Append("}");
				return builder.ToString();
			}
		}

		public static Match GetMatch(Game game, int index)
			=> game.ActiveRound.Matches.ElementAt(index);

		public static void SetResult(Game game, int index, RESULT_T result)
			=> GetMatch(game, index).SetResult(result);

		public static string Export(Game game) {
			var buf = new StringBuilder();
			game.Players.Export(game.Rule, new StringWriter(buf));
			return buf.ToString();
		}

		public static string Export(IRule rule, Players players) {
			var buf = new StringBuilder();
			using (var writer = new StringWriter(buf)) {
				players.Export(rule, writer);
			}
			return buf.ToString();
		}

		public static void CreateSingleMatchRound(Game game, int[] playerlist) {
			game.AddRound(CreateSingleMatches());

			IEnumerable<Match> CreateSingleMatches() {
				var id = 0;
				for (int i = 0; i < playerlist.Count(); i += 2) {
					yield return
						new SingleMatch(
							++id,
							new Match.Record(game.GetPlayer(playerlist[i]-1), new SingleMatchResult(RESULT_T.Win)),
							new Match.Record(game.GetPlayer(playerlist[i + 1]-1), new SingleMatchResult(RESULT_T.Lose))
							);
				}
			}
		}


	}
}
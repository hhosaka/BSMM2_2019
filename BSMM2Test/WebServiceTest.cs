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
using BSMM2.Models.WebAccess;

namespace BSMM2Test
{
	[TestClass]
	public class WebServiceTest {
		[TestMethod]
		public void WebServiceTest1() {
			var app= BSMMApp.Create("",true);
			var id=Guid.NewGuid();
			app.Add(new Game(new Players(app.Rule,8),id),true);
			Assert.AreEqual(app.Game.WebServiceId, id);
			app.Add(new Game(new Players(app.Rule, 8)), true);
			Assert.AreNotEqual(app.Game.WebServiceId, id);
		}

		[TestMethod]
		public void OutlineTest1() {
			const string owner = "owner";
			const string title = "title";
			const string mailAddress = "mailAddress";
			var app = BSMMApp.Create("", true);
			app.Owner = owner;
			app.MailAddress = mailAddress;
			var id = Guid.NewGuid();
			var game = new Game(new Players(app.Rule, 8), id, title);
			app.Add(game, true);
			var outline = GameOutline.Create(app);
			Assert.AreEqual(id.ToString(), outline.Id);
			Assert.AreEqual(owner, outline.Owner);
			Assert.AreEqual(mailAddress, outline.MailAddress);
			Assert.AreEqual(title, outline.Title);
			Assert.AreEqual(game.Rule.Name, outline.RuleName);
		}
	}
}

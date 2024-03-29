﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BSMM2.Models.WebAccess
{
	[JsonObject]
	public class MatchOutline
	{
		public static IEnumerable<MatchOutline> Create(IEnumerable<Match> matches) {
			int id=0;
			return matches.Select(match => new MatchOutline(match, ++id));
		}

		[JsonProperty]
		public int Id { get; }
		[JsonProperty]
		public bool Status => _match.IsFinished;
		[JsonProperty]
		public bool IsGapMatch => _match.IsGapMatch;
		[JsonProperty]
		public string Player1Name=>_match.Record1.Player.Name;
		[JsonProperty]
		public bool IsPlayer1Win => _match.Record1.Result.RESULT == RESULT_T.Win;

		public string Player2Name => _match.Record2.Player.Name;
		[JsonProperty]
		public bool IsPlayer2Win => _match.Record2.Result.RESULT == RESULT_T.Win;

		[JsonIgnore]
		private Match _match;
		private MatchOutline(Match match,int id) {
			_match = match;
			Id = id;
		}
	}
}

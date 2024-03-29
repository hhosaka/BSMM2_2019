﻿using BSMM2.Resource;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	[JsonObject]
	public abstract class Match {

		public event PropertyChangedEventHandler PropertyChanged;

		[JsonObject]
		public class Record : IRecord {

			private class DefaultResult : IResult {

				public RESULT_T RESULT => RESULT_T.Progress;

				[JsonIgnore]
				public int MatchPoint => 0;

				[JsonIgnore]
				public double?LifePoint => null;

				[JsonIgnore]
				public double WinPoint => 0;

				[JsonIgnore]
				public bool IsFinished => false;

				[JsonIgnore]
				public IPoint Point => null;

			}

			private static readonly IResult _defaultResult = new DefaultResult();

			[JsonProperty]
			private Player _player;

			[JsonIgnore]
			public Player Player {
				get
					=>_player??BYE.Instance;
				private set
					=>_player=value;
			}

			[JsonProperty]
			IResult _result;

			[JsonIgnore]
			public IResult Result {
				get => _result ?? _defaultResult;
				private set =>_result = value;
			}

			public void SetResult(IRule rule, IResult result)
				=> Result = result;

			private Record() {
			}

			public Record(Player player,IResult result=null) {
				Player = player;
				Result = result;
			}
		}

		[JsonProperty]
		public Record[] _records;

		[JsonProperty]
		public bool IsGapMatch { get; private set; }

		[JsonIgnore]
		public bool IsFinished
			=> !_records.Any(record => !record.Result.IsFinished);

		[JsonIgnore]
		public bool IsByeMatch
			=> _records.Any(result => result.Player == BYE.Instance);

		[JsonIgnore]
		public IEnumerable<string> PlayerNames
			=> _records.Select(result => result.Player.Name);

		[JsonIgnore]
		public IRecord Record1 => _records[0];

		[JsonIgnore]
		public IRecord Record2 => _records[1];

		public bool HasPlayer(Player player)
			=>_records.Any(record => record.Player.Id == player.Id);

		public abstract void SetResult(IRule rule, RESULT_T result);

		protected void SetResults(IRule rule, IResult result1, IResult result2) {
			_records[0].SetResult(rule, result1);
			_records[1].SetResult(rule, result2);
			PropertyChanged?.Invoke(this, null);
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Record1)));
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Record2)));
			//PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFinished)));
		}

		public void Swap(Match other) {
			var temp = _records[0];
			_records[0] = other._records[0];
			other._records[0] = temp;

			SetIsGapMatch();
			other.SetIsGapMatch();
		}

		public Record GetRecord(Player player)
			=> _records.First(r => r.Player == player);

		public Record GetOpponentRecord(Player player)
			=> _records.First(r => r.Player != player);

		public Match() {// For Serializer
		}

		private void SetIsGapMatch() {
			IsGapMatch = !IsByeMatch && (_records[0].Player as Player)?.Point.MatchPoint != (_records[1].Player as Player)?.Point.MatchPoint;
		}

		public Match(Player player1, Player player2 = null)
			: this(new Record(player1), new Record(player2)) {
		}
		protected Match(Record record1, Record record2) {
			_records = new[] { record1, record2 };
			SetIsGapMatch();
		}
	}
}
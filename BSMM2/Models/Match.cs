using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms.Internals;

namespace BSMM2.Models {

	[JsonObject]
	public abstract class Match : INotifyPropertyChanged {
		private static IPlayer BYE = new BYE();

		public event PropertyChangedEventHandler PropertyChanged;

		[JsonProperty]
		public int Id { get; private set; }

		[JsonObject]
		public class Record : IRecord {

			private class DefaultResult : IResult {

				public RESULT_T RESULT => RESULT_T.Progress;

				[JsonIgnore]
				public int MatchPoint => 0;

				[JsonIgnore]
				public double LifePoint => -1;

				[JsonIgnore]
				public double WinPoint => 0;

				[JsonIgnore]
				public bool IsFinished => false;

				public IExportData Export(IExportData data)
					=> throw new System.NotImplementedException();
			}

			private static readonly IResult _defaultResult = new DefaultResult();

			[JsonProperty]
			private int _player_id;

			[JsonProperty]
			public IPlayer Player { get; private set; }

			[JsonProperty]
			public IResult Result { get; private set; }

			public void SetResult(IResult result)
				=> Result = result;

			private Record() {
			}

			public Record(IPlayer player,IResult result=null) {
				Player = player;
				Result = result??_defaultResult;
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
			=> _records.Any(result => result.Player is BYE);

		[JsonIgnore]
		public IEnumerable<string> PlayerNames
			=> _records.Select(result => result.Player.Name);

		[JsonIgnore]
		public IRecord Record1 => _records[0];

		[JsonIgnore]
		public IRecord Record2 => _records[1];

		public bool HasPlayer(Player player)
			=>_records.Any(record => record.Player.Name == player.Name);// TODO tentative. It will be compared by id

		public abstract void SetResult(RESULT_T result);

		protected void SetResults(IResult result1, IResult result2) {
			_records[0].SetResult(result1);
			_records[1].SetResult(result2);
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Record1)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Record2)));
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFinished)));
		}

		public void Swap(Match other) {
			var temp = _records[0];
			_records[0] = other._records[0];
			other._records[0] = temp;

			SetIsGapMatch();
			other.SetIsGapMatch();
		}

		public void Commit()
			=> _records.ForEach(result => (result.Player as Player)?.Commit(this));

		public Record GetRecord(IPlayer player)
			=> _records.First(r => r.Player == player);

		public Record GetOpponentRecord(IPlayer player)
			=> _records.First(r => r.Player != player);

		public Match() {// For Serializer
		}

		private void SetIsGapMatch() {
			IsGapMatch = !IsByeMatch && (_records[0].Player as Player)?.Point.MatchPoint != (_records[1].Player as Player)?.Point.MatchPoint;
		}

		public Match(int id, IPlayer player1, IPlayer player2 = null) {
			Id = id;
			if (player2 != null) {
				_records = new[] { new Record(player1), new Record(player2) };
				SetIsGapMatch();
			} else {
				_records = new[] { new Record(player1), new Record(BYE) };
			}
		}
		public Match(int id, Record record1, Record record2) {
			Id = id;
			_records = new[] { record1, record2 };
			SetIsGapMatch();
		}
	}
}
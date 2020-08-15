using System;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;

namespace BSMM2.Models {

	public class Storage {
		public static readonly string LOGFILE = "Serializer.log";
		private IsolatedStorageFile _store;

		public T Load<T>(string filename, Func<T> initiator) where T : new() {
			if (_store.FileExists(filename)) {
				//using (var errs = _store.OpenFile(LOGFILE, FileMode.Append))
				//using (var err = new StreamWriter(errs))
				using (var strm = _store.OpenFile(filename, FileMode.Open))
				using (var reader = new StreamReader(strm)) {
					return new Serializer<T>().Deserialize(reader);
				}
			}
			return initiator();
		}

		public void Save<T>(T data, string filename) {
			using (var strm = _store.CreateFile(filename))
			using (var writer = new StreamWriter(strm)) {
				new Serializer<T>().Serialize(writer, data);
			}
		}

		public void Delete(string filename) {
			_store.DeleteFile(filename);
		}

		public string Log() {
			using (var errs = _store.OpenFile(LOGFILE, FileMode.Open))
			using (var err = new StreamReader(errs)) {
				return err.ReadToEnd();
			}
		}

		public Storage() {
			_store = IsolatedStorageFile.GetUserStoreForApplication();
		}
	}
}
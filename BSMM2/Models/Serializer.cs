using Newtonsoft.Json;
using System.Diagnostics;
using System.Globalization;
using System.IO;

namespace BSMM2.Models {

	public class Serializer<T> {

		public void Serialize(TextWriter w, T obj, TextWriter err = null) {
			var settings
				= new JsonSerializerSettings {
					PreserveReferencesHandling = PreserveReferencesHandling.Objects,
					TypeNameHandling = TypeNameHandling.Auto,
					Error = (sender, args) => OnError(args)
				};

			w.Write(JsonConvert.SerializeObject(obj, settings));

			void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs args) {
				Debug.Write(args);
				err?.WriteLine(args.ErrorContext.Error.Message);
				err?.WriteLine();
			}
		}

		public void Serialize(string filename, T obj, TextWriter err = null) {
			using (var w = new StreamWriter(filename)) {
				Serialize(w, obj, err);
				w.Close();
			}
		}

		public T Deserialize(TextReader r, TextWriter err = null) {
			var settings
				= new JsonSerializerSettings {
					PreserveReferencesHandling = PreserveReferencesHandling.Objects,
					TypeNameHandling = TypeNameHandling.Auto,
					Error = (sender, args) => OnError(args)
				};

			return JsonConvert.DeserializeObject<T>(r.ReadToEnd(), settings);

			void OnError(Newtonsoft.Json.Serialization.ErrorEventArgs args) {
				Debug.Write(args);
				err?.WriteLine(args.ErrorContext.Error.Message);
				err?.WriteLine();
			}
		}

		public T Deserialize(string filename) {
			using (var r = new StreamReader(filename)) {
				return Deserialize(r);
			}
		}
	}
}
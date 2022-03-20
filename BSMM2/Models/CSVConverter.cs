using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSMM2.Models
{
	public class CSVConverter
	{
		public static void Convert(IDictionary<string, object> data, TextWriter writer)
			=> Convert(data, writer, true);

		private static void Convert(IDictionary<string, object> data, TextWriter writer,bool title) {

			foreach (var d in data) {
				if (d.Value is IDictionary<string, object> dic) {
					Convert(dic, writer, title);
					writer.WriteLine();
					title = false;
				} else {
					if (title) {
						foreach (var key in data.Keys) {
							writer.Write(key);
							writer.Write(",");
						}
						title = false;
						writer.WriteLine();
					}
					var v = d.Value;
					if (v is string s) {
						writer.Write("\"" + s + "\"");
					} else {
						writer.Write(v.ToString());
					}
					writer.Write(",");
				}
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSMM2.Models
{
	public class ExportSource : Dictionary<string, object> { }

	public interface IExportableObject {
		ExportSource Export(ExportSource src, string origin = "");
	}
}

using System.Collections.Generic;

namespace BSMM2.Models {

	public class ExportData : Dictionary<string, object> { }

	public interface IExportable {

		ExportData Export(ExportData data);
	}
}
using System.Collections.Generic;

namespace BSMM2.Models {

	public interface IExportData : IDictionary<string, object> { }

	public class ExportData : Dictionary<string, object>, IExportData { }

	public interface IExportable {

		IExportData Export(IExportData data);
	}
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BSMM2.Models
{
	public interface IExportableObject {
		bool ExportTitle(TextWriter writer, string origin="");
		bool ExportData(TextWriter writer);
	}

	public interface IExporter
	{
		IExportableObject ExportPlayers(Game game);
	}
}

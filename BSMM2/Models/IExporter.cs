using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models
{
	interface IExportableObject {
	}

	public interface IExporter
	{
		object ExportPlayers(Game game);
	}
}

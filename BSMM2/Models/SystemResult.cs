using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models
{
	public class SystemResult {
		enum REUSULT_T {Success,NetworkError,SystemError};
		RESULT_T Result { get; }
		public SystemResult() { }
	}
}

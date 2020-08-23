using System;
using System.Collections.Generic;
using System.Text;

namespace BSMM2.Models
{
	public interface IScore
	{
		RESULT_T RESULT { get; }
		int LifePoint1 { get; }
		int LifePoint2 { get; }
	}
}

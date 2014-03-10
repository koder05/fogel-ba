using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.BL.PC
{
	public interface IPcReports
	{
		IEnumerable<DdsReport> GetDdsReport(Guid personId);
	}
}

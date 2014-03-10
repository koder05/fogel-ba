using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EF.Poco;

namespace RF.BL.EF.PocoProxy
{
	public interface IPocoProxy
	{
		BasePoco Poco { get; }
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Transactions
{
	public interface ITransaction : IDisposable
	{
		void Complete();
	}

	public interface ITransactionLauncher
	{
		ITransaction StartNew();
	}
}

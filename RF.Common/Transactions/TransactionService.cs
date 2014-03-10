using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Transactions
{
	public class TransactionService
	{
		private ITransactionLauncher _trans;
		public TransactionService(ITransactionLauncher trans)
		{
			_trans = trans;
		}
		
		public ITransaction StartNew()
		{
			return _trans.StartNew();
		}
	}
}

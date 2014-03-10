using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RF.Common.Transactions
{
	public static class Transactions
	{
		public static TransactionService Service { get; set; }

		public static ITransaction StartNew()
		{
			if (Service == null)
			{
				throw new InvalidOperationException("Не задан сервис транзакций");
			}

			return Service.StartNew();
		}
	}
}

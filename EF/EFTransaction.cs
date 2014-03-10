using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

using RF.Common.Transactions;

namespace EF
{
	public class EFTransaction : ITransaction
	{
		public EFTransaction()
		{
			TransactionOptions options = new TransactionOptions();
			options.IsolationLevel = IsolationLevel.ReadCommitted;
			_scope = new TransactionScope(TransactionScopeOption.Required, options);
		}

		~EFTransaction()
		{
			Dispose(false);
		}

		public void Complete()
		{
			_scope.Complete();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				_scope.Dispose();
			}
		}

		private readonly TransactionScope _scope;
	}

	public class TransactionLauncher : ITransactionLauncher
	{
		public ITransaction StartNew()
		{
			return new EFTransaction();
		}
	}
}

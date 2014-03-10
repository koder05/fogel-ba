using System;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.Entity.Infrastructure;

using RF.Common.Transactions;

namespace EF
{
    public class EFImplicitTransaction : ITransaction
    {
        public EFImplicitTransaction(DbConnection connection)
        {
            connection.StateChange += ConnectionStateChangeHandler;
            connection.Open();
            _conn = connection;
        }

        ~EFImplicitTransaction()
        {
            Dispose(false);
        }

        private void ConnectionStateChangeHandler(object sender, StateChangeEventArgs e)
        {
            if (e.CurrentState == ConnectionState.Open && _scope == null)
                _scope = (sender as DbConnection).BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public void Complete()
        {
            _scope.Commit();
            isCommited = true;
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
                DbConnection curConn = _scope.Connection ?? _conn;

                if (isCommited == false && (curConn != null && curConn.State != ConnectionState.Closed && curConn.State != ConnectionState.Broken))
                {
                    _scope.Rollback();
                }

                if (curConn != null && curConn.State != ConnectionState.Closed && curConn.State != ConnectionState.Broken)
                {
                    curConn.Close();
                }

                _scope.Dispose();
            }
        }

        private bool isCommited = false;
        private DbTransaction _scope;
        private DbConnection _conn;
    }

    public class EFImplicitTransactionLauncher : ITransactionLauncher
    {
        private AssetsEFCtx _db;

        public EFImplicitTransactionLauncher(AssetsEFCtx db)
        {
            _db = db;
        }

        public ITransaction StartNew()
        {
            ObjectContext context = ((IObjectContextAdapter)_db).ObjectContext;
            return new EFImplicitTransaction(context.Connection);
        }
    }
}

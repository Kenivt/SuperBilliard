using SuperBilliardServer.Debug;
using System.Collections.Concurrent;

namespace SuperBilliardServer.Sql
{
    public class SqlManager : ServerCore.Sington.SingtonBase<SqlManager>
    {
        private readonly ConcurrentStack<SqlConnectionController> sqlConnectionControllers = new ConcurrentStack<SqlConnectionController>();

        public int AcquireCount { get; private set; }
        public int ReleaseCount { get; private set; }
        public int UsingCount { get; private set; }
        public int IdelCount { get; private set; }

        public SqlConnectionController GetConnection()
        {
            if (sqlConnectionControllers.TryPop(out SqlConnectionController controller))
            {
                AcquireCount++;
                UsingCount++;
                controller.OpenConnect();
                return controller;
            }
            else
            {
                SqlConnectionController sqlConnectionController = new SqlConnectionController();
                UsingCount++;
                sqlConnectionController.OpenConnect();
                return sqlConnectionController;
            }
        }

        public void ReleaseConnection(SqlConnectionController sqlConnectionController)
        {
            if (sqlConnectionController == null)
            {
                return;
            }
            ReleaseCount++;
            UsingCount--;
            IdelCount++;
            sqlConnectionController.CloseConnect();
            sqlConnectionControllers.Push(sqlConnectionController);
        }

        private Dictionary<Type, ISqlHandler> _sqlHandlerDic = new Dictionary<Type, ISqlHandler>();

        public T GetSqlHandler<T>() where T : ISqlHandler
        {
            return (T)_sqlHandlerDic[typeof(T)];
        }

        public void RigisterSqlHandler<T>(T sqlHandler) where T : ISqlHandler
        {
            Type type = typeof(T);
            if (_sqlHandlerDic.ContainsKey(type) == false)
            {
                _sqlHandlerDic.Add(typeof(T), sqlHandler);
            }
            else
            {
                Log.Error("RigisterSqlHandler error, '{0}' is already exist", type.Name);
            }
        }
        public override void Dispose()
        {
            while (sqlConnectionControllers.TryPop(out SqlConnectionController controller))
            {
                controller?.Dispose();
            }
            base.Dispose();
        }
    }
}

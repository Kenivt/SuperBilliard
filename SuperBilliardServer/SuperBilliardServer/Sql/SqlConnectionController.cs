using GameFramework;
using Microsoft.Data.SqlClient;

namespace SuperBilliardServer.Sql
{
    public class SqlConnectionController : IDisposable, IReference
    {
        public readonly string defaultConnectKey =
            "Data Source=.;Initial Catalog=SuperBilliardGame;" +
            "Integrated Security=True;TrustServerCertificate=true";

        internal SqlConnection Connection => _sqlConnection;

        private SqlConnection _sqlConnection;

        public SqlConnectionController()
        {
            _sqlConnection = new SqlConnection(defaultConnectKey);
        }

        public void OpenConnect()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                _sqlConnection.Open();
            }
        }

        public void CloseConnect()
        {
            if (_sqlConnection.State == System.Data.ConnectionState.Open)
            {
                _sqlConnection.Close();
            }
        }

        public static SqlConnectionController Create()
        {
            SqlConnectionController sqlConnectionController = ReferencePool.Acquire<SqlConnectionController>();
            return sqlConnectionController;
        }

        public void Dispose()
        {
            CloseConnect();
            _sqlConnection.Dispose();
        }

        public void Clear()
        {
            CloseConnect();
        }
    }
}

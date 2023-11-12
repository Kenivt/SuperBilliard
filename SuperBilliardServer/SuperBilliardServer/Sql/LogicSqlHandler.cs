using GameMessage;
using System.Data;
using Microsoft.Data.SqlClient;
using SuperBilliardServer.Debug;

namespace SuperBilliardServer.Sql
{
    public interface ILoginSqlHandler : ISqlHandler
    {
        Task Login(string userName, string password, SCLogin sCLogin);
        Task<bool> Rigister(string username, string password);
        Task UnloginAsync(string userName);
        void Unlogin(string userName);
    }

    public class LogicSqlHandler : SqlHandlerBase, ILoginSqlHandler
    {

        public const string findSql = "SELECT COUNT(*) FROM PlayerData WHERE username = @username";
        public const string insertSql = "INSERT INTO PlayerData (username,userpassword) VALUES (@Value1, @Value2)";

        public async Task Login(string userName, string password, SCLogin sCLogin)
        {
            SqlConnectionController controller = SqlManager.Instance.GetConnection();
            SqlConnection connection = controller.Connection;
            SqlCommand command = new SqlCommand("SELECT p_password, p_islogin FROM PlayerData WHERE p_userName = @userName", connection);
            command.Parameters.AddWithValue("@userName", userName);

            SqlDataReader reader = await command.ExecuteReaderAsync().ConfigureAwait(true);

            if (reader.Read())
            {
                string dbPassword = reader.GetString(0);
                object value = reader.GetBoolean(1); // 从数据库中获取值
                bool isLogin = Convert.ToBoolean(value);

                if (dbPassword == password)
                {
                    if (!isLogin)
                    {
                        reader.Close();

                        command = new SqlCommand("UPDATE PlayerData SET p_islogin = 1 WHERE p_userName = @userName", connection);
                        command.Parameters.AddWithValue("@userName", userName);
                        await command.ExecuteNonQueryAsync().ConfigureAwait(true);
                        sCLogin.Username = userName;
                        sCLogin.Result = ReturnResult.Success;
                    }
                    else
                    {
                        sCLogin.Result = ReturnResult.Failure;
                    }
                }
                else
                {
                    sCLogin.Result = ReturnResult.Failure;
                }
            }
            else
            {
                sCLogin.Result = ReturnResult.Failure;
            }

            SqlManager.Instance.ReleaseConnection(controller);
        }

        public async Task<bool> Rigister(string username, string password)
        {
            bool flag = false;
            var controller = SqlManager.Instance.GetConnection();
            var connection = controller.Connection;

            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM PlayerData WHERE p_userName = @userName", connection))
            {
                // 添加参数
                command.Parameters.Add(new SqlParameter("@userName", SqlDbType.Char, 12)).Value = username;

                // 执行命令并获取结果
                int count = (int)command.ExecuteScalar();

                if (count == 0)
                {
                    // 创建命令对象
                    using (SqlCommand insertCommand = new SqlCommand("INSERT INTO PlayerData (p_userName, p_password, p_islogin) VALUES (@userName, @password, @islogin)", connection))
                    {
                        // 添加参数
                        insertCommand.Parameters.Add(new SqlParameter("@userName", SqlDbType.Char, 12)).Value = username;
                        insertCommand.Parameters.Add(new SqlParameter("@password", SqlDbType.VarChar, 20)).Value = password;
                        insertCommand.Parameters.Add(new SqlParameter("@islogin", SqlDbType.Bit)).Value = false;

                        // 执行命令
                        await insertCommand.ExecuteNonQueryAsync();
                        flag = true;
                    }
                }

                SqlManager.Instance.ReleaseConnection(controller);
                return flag;
            }
        }

        public async Task UnloginAsync(string userName)
        {
            var controller = SqlManager.Instance.GetConnection();

            SqlConnection connection = controller.Connection;
            using (SqlCommand command = new SqlCommand("UPDATE PlayerData SET p_islogin = 0 WHERE p_userName = @userName", connection))
            {
                command.Parameters.AddWithValue("@userName", userName);
                int count = await command.ExecuteNonQueryAsync();
                if (count == 0)
                {
                    Log.Error("玩家{0}退出登录失败,执行异常", userName);
                }
            }
        }
        public void Unlogin(string userName)
        {
            var controller = SqlManager.Instance.GetConnection();

            SqlConnection connection = controller.Connection;
            using (SqlCommand command = new SqlCommand("UPDATE PlayerData SET p_islogin = 0 WHERE p_userName = @userName", connection))
            {
                command.Parameters.AddWithValue("@userName", userName);
                int count = command.ExecuteNonQuery();
                if (count == 0)
                {
                    Log.Error("玩家{0}退出登录失败,执行异常", userName);
                }
            }
        }
    }
}


using MySql.Data.MySqlClient;
using System.Data;

namespace Sundar_Tyres___Wheels.Database
{
    public class DatabaseHandler
    {
        public static readonly Lazy<DatabaseHandler> _instance = new(() => new DatabaseHandler());
        public static DatabaseHandler Instance => _instance.Value;

        private MySqlConnection _connection;
        private readonly string _connectionString = "Server=mw.c3kmsoeogow5.us-east-2.rds.amazonaws.com;Port=3306;Database=temp;User Id=munees;Password=Muneeswaran8072.p;";

        private DatabaseHandler()
        {
            _connection = new MySqlConnection(_connectionString);
        }

        public void openConnection()
        {
            if(_connection.State == System.Data.ConnectionState.Closed)
            {
                _connection.Open();
            }
        }
        public void CloseConnection()
        {
            if (_connection.State == ConnectionState.Open)
            {
                _connection.Close();
            }
        }

        public MySqlConnection Connection => _connection;


    }
}

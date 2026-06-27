using MySql.Data.MySqlClient;
using System;

namespace CyberChatBot.Data
{
    public class DatabaseHelper
    {
        private readonly DbContext _context;

        public DatabaseHelper()
        {
            _context = new DbContext();
            _context.InitializeDatabase();
        }

        public MySqlConnection GetConnection()
        {
            return _context.GetConnection();
        }

        public int ExecuteNonQuery(string query, Action<MySqlParameterCollection> addParameters = null)
        {
            var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand(query, connection);
            addParameters?.Invoke(cmd.Parameters);
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string query, Action<MySqlParameterCollection> addParameters = null)
        {
            var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand(query, connection);
            addParameters?.Invoke(cmd.Parameters);
            return cmd.ExecuteScalar();
        }

        public MySqlDataReader ExecuteReader(string query, Action<MySqlParameterCollection> addParameters = null)
        {
            var connection = GetConnection();
            connection.Open();
            var cmd = new MySqlCommand(query, connection);
            addParameters?.Invoke(cmd.Parameters);
            return cmd.ExecuteReader();
        }
    }
}
using MySql.Data.MySqlClient;
using System;
using System.Data;

namespace CyberChatBot.Data
{
    public class DbContext
    {
        private readonly string _connectionString;

        public DbContext()
        {
            _connectionString = "Server=localhost;Database=cyberchatbot;Uid=root;Pwd=yourpassword;";
        }

        public string insertQuery { get; private set; }

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public void InitializeDatabase()
        {
            try
            {
                var connection = GetConnection();
                connection.Open();

                string createDatabase = @"
                    CREATE DATABASE IF NOT EXISTS cyberchatbot;
                    USE cyberchatbot;

                    CREATE TABLE IF NOT EXISTS Users (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Name VARCHAR(100) NOT NULL,
                        CurrentSentiment VARCHAR(50),
                        LastInteraction DATETIME
                    );

                    CREATE TABLE IF NOT EXISTS Tasks (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Title VARCHAR(200) NOT NULL,
                        Description TEXT,
                        ReminderDate DATETIME,
                        CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP,
                        IsCompleted BOOLEAN DEFAULT FALSE,
                        UserId INT,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );

                    CREATE TABLE IF NOT EXISTS ActivityLogs (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        UserId INT,
                        Action VARCHAR(100),
                        Details TEXT,
                        Timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
                        FOREIGN KEY (UserId) REFERENCES Users(Id)
                    );

                    CREATE TABLE IF NOT EXISTS QuizQuestions (
                        Id INT AUTO_INCREMENT PRIMARY KEY,
                        Question TEXT NOT NULL,
                        Type VARCHAR(20) NOT NULL,
                        Options JSON,
                        CorrectAnswer VARCHAR(50),
                        Explanation TEXT
                    );";

                var command = new MySqlCommand(createDatabase, connection);
                command.ExecuteNonQuery();

                // Insert sample quiz questions if empty
                string checkQuestions = "SELECT COUNT(*) FROM QuizQuestions";
                var checkCmd = new MySqlCommand(checkQuestions, connection);
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());

                if (count == 0)
                {
                    InsertSampleQuestions(connection);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Database initialization error: {ex.Message}");
                throw;
            }
        }

        private void InsertSampleQuestions(MySqlConnection connection)
        {
           

            var cmd = new MySqlCommand(insertQuery, connection);
            cmd.ExecuteNonQuery();
        }
    }
}
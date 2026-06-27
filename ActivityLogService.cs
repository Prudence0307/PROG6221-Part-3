using CyberChatBot.Data;
using CyberChatBot.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CyberChatBot.Services
{
    public class ActivityLogService
    {
        private readonly DatabaseHelper _dbHelper;

        public ActivityLogService()
        {
            _dbHelper = new DatabaseHelper();
        }

        public bool LogActivity(int userId, string action, string details = "")
        {
            try
            {
                string query = @"
                    INSERT INTO ActivityLogs (UserId, Action, Details, Timestamp)
                    VALUES (@UserId, @Action, @Details, @Timestamp)";

                return _dbHelper.ExecuteNonQuery(query, parameters =>
                {
                    parameters.AddWithValue("@UserId", userId);
                    parameters.AddWithValue("@Action", action);
                    parameters.AddWithValue("@Details", details ?? "");
                    parameters.AddWithValue("@Timestamp", DateTime.Now);
                }) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error logging activity: {ex.Message}");
                return false;
            }
        }

        public List<ActivityLogEntry> GetRecentActivities(int userId, int limit = 10)
        {
            var activities = new List<ActivityLogEntry>();
            try
            {
                string query = @"
                    SELECT * FROM ActivityLogs 
                    WHERE UserId = @UserId 
                    ORDER BY Timestamp DESC 
                    LIMIT @Limit";

                 var reader = _dbHelper.ExecuteReader(query, parameters =>
                {
                    parameters.AddWithValue("@UserId", userId);
                    parameters.AddWithValue("@Limit", limit);
                });

                while (reader.Read())
                {
                    activities.Add(new ActivityLogEntry
                    {
                        Id = reader.GetInt32("Id"),
                        UserId = reader.GetInt32("UserId"),
                        Action = reader.GetString("Action"),
                        Details = reader.IsDBNull(reader.GetOrdinal("Details")) ? "" : reader.GetString("Details"),
                        Timestamp = reader.GetDateTime("Timestamp")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting activity log: {ex.Message}");
            }
            return activities;
        }

        public bool ClearAllActivities(int userId)
        {
            try
            {
                string query = "DELETE FROM ActivityLogs WHERE UserId = @UserId";
                return _dbHelper.ExecuteNonQuery(query, parameters =>
                {
                    parameters.AddWithValue("@UserId", userId);
                }) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing activities: {ex.Message}");
                return false;
            }
        }
    }
}

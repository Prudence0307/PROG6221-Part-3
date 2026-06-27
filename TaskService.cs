using CyberChatBot.Data;
using CyberChatBot.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace CyberChatBot.Services
{
    public class TaskService
    {
        private readonly DatabaseHelper _dbHelper;

        public TaskService()
        {
            _dbHelper = new DatabaseHelper();
        }

        public bool AddTask(TaskItem task)
        {
            try
            {
                string query = @"
                    INSERT INTO Tasks (Title, Description, ReminderDate, UserId, CreatedDate)
                    VALUES (@Title, @Description, @ReminderDate, @UserId, @CreatedDate)";

                return _dbHelper.ExecuteNonQuery(query, parameters =>
                {
                    parameters.AddWithValue("@Title", task.Title);
                    parameters.AddWithValue("@Description", task.Description ?? "");
                    parameters.AddWithValue("@ReminderDate", task.ReminderDate);
                    parameters.AddWithValue("@UserId", task.UserId);
                    parameters.AddWithValue("@CreatedDate", DateTime.Now);
                }) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding task: {ex.Message}");
                return false;
            }
        }

        public List<TaskItem> GetTasks(int userId)
        {
            var tasks = new List<TaskItem>();
            try
            {
                string query = "SELECT * FROM Tasks WHERE UserId = @UserId ORDER BY ReminderDate ASC";
                var reader = _dbHelper.ExecuteReader(query, parameters =>
                {
                    parameters.AddWithValue("@UserId", userId);
                });

                while (reader.Read())
                {
                    tasks.Add(new TaskItem
                    {
                        Id = reader.GetInt32("Id"),
                        Title = reader.GetString("Title"),
                        Description = reader.IsDBNull(reader.GetOrdinal("Description")) ? "" : reader.GetString("Description"),
                        ReminderDate = reader.GetDateTime("ReminderDate"),
                        CreatedDate = reader.GetDateTime("CreatedDate"),
                        IsCompleted = reader.GetBoolean("IsCompleted"),
                        UserId = reader.GetInt32("UserId")
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting tasks: {ex.Message}");
            }
            return tasks;
        }

        public bool UpdateTask(int taskId, bool isCompleted)
        {
            try
            {
                string query = "UPDATE Tasks SET IsCompleted = @IsCompleted WHERE Id = @TaskId";
                return _dbHelper.ExecuteNonQuery(query, parameters =>
                {
                    parameters.AddWithValue("@IsCompleted", isCompleted);
                    parameters.AddWithValue("@TaskId", taskId);
                }) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating task: {ex.Message}");
                return false;
            }
        }

        public bool DeleteTask(int taskId)
        {
            try
            {
                string query = "DELETE FROM Tasks WHERE Id = @TaskId";
                return _dbHelper.ExecuteNonQuery(query, parameters =>
                {
                    parameters.AddWithValue("@TaskId", taskId);
                }) > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting task: {ex.Message}");
                return false;
            }
        }
    }
}
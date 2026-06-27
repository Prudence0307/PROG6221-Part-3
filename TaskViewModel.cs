using CyberChatBot.Models;
using CyberChatBot.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CyberChatBot.App.ViewModels
{
    public class TaskViewModel : INotifyPropertyChanged
    {
        private readonly TaskService _taskService;
        private readonly ActivityLogService _logService;
        private readonly User _currentUser;

        private string _newTaskTitle;
        private string _newTaskDescription;
        private DateTime _newTaskReminder;
        private ObservableCollection<TaskItem> _tasks;

        public string NewTaskTitle
        {
            get => _newTaskTitle;
            set { _newTaskTitle = value; OnPropertyChanged(); }
        }

        public string NewTaskDescription
        {
            get => _newTaskDescription;
            set { _newTaskDescription = value; OnPropertyChanged(); }
        }

        public DateTime NewTaskReminder
        {
            get => _newTaskReminder;
            set { _newTaskReminder = value; OnPropertyChanged(); }
        }

        public ObservableCollection<TaskItem> Tasks
        {
            get => _tasks;
            set { _tasks = value; OnPropertyChanged(); }
        }

        public ICommand AddTaskCommand { get; }
        public ICommand ToggleTaskCommand { get; }
        public ICommand DeleteTaskCommand { get; }
        public TaskViewModel DataContext { get; internal set; }

        public TaskViewModel(User user)
        {
            _currentUser = user;
            _taskService = new TaskService();
            _logService = new ActivityLogService();

            Tasks = new ObservableCollection<TaskItem>();
            NewTaskReminder = DateTime.Now.AddDays(1);

            AddTaskCommand = new RelayCommand(ExecuteAddTask);
            ToggleTaskCommand = new RelayCommand(ExecuteToggleTask);
            DeleteTaskCommand = new RelayCommand(ExecuteDeleteTask);

            LoadTasks();
        }

        private void ExecuteDeleteTask()
        {
            throw new NotImplementedException();
        }

        private void ExecuteToggleTask()
        {
            throw new NotImplementedException();
        }

        private void LoadTasks()
        {
            try
            {
                var tasks = _taskService.GetTasks(_currentUser.Id);
                Tasks.Clear();
                foreach (var task in tasks)
                {
                    Tasks.Add(task);
                }
                _logService.LogActivity(_currentUser.Id, "Tasks Loaded", $"Loaded {Tasks.Count} tasks");
            }
            catch (Exception ex)
            {
                _logService.LogActivity(_currentUser.Id, "Error Loading Tasks", ex.Message);
            }
        }

        private void ExecuteAddTask()
        {
            if (string.IsNullOrWhiteSpace(NewTaskTitle))
                return;

            try
            {
                var task = new TaskItem
                {
                    Title = NewTaskTitle.Trim(),
                    Description = NewTaskDescription?.Trim() ?? "",
                    ReminderDate = NewTaskReminder,
                    UserId = _currentUser.Id,
                    CreatedDate = DateTime.Now
                };

                if (_taskService.AddTask(task))
                {
                    Tasks.Add(task);
                    NewTaskTitle = string.Empty;
                    NewTaskDescription = string.Empty;
                    NewTaskReminder = DateTime.Now.AddDays(1);
                    _logService.LogActivity(_currentUser.Id, "Task Added", $"Title: {task.Title}");
                }
            }
            catch (Exception ex)
            {
                _logService.LogActivity(_currentUser.Id, "Error Adding Task", ex.Message);
            }
        }

        private void ExecuteToggleTask(object parameter)
        {
            if (parameter is TaskItem task)
            {
                try
                {
                    task.IsCompleted = !task.IsCompleted;
                    if (_taskService.UpdateTask(task.Id, task.IsCompleted))
                    {
                        _logService.LogActivity(_currentUser.Id, "Task Toggled",
                            $"Task: {task.Title}, Completed: {task.IsCompleted}");
                    }
                }
                catch (Exception ex)
                {
                    _logService.LogActivity(_currentUser.Id, "Error Toggling Task", ex.Message);
                }
            }
        }

        private void ExecuteDeleteTask(object parameter)
        {
            if (parameter is TaskItem task)
            {
                try
                {
                    if (_taskService.DeleteTask(task.Id))
                    {
                        Tasks.Remove(task);
                        _logService.LogActivity(_currentUser.Id, "Task Deleted", $"Title: {task.Title}");
                    }
                }
                catch (Exception ex)
                {
                    _logService.LogActivity(_currentUser.Id, "Error Deleting Task", ex.Message);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
using CyberChatBot.Models;
using CyberChatBot.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace CyberChatBot.App.ViewModels
{
    public class ActivityLogViewModel : INotifyPropertyChanged
    {
        private readonly ActivityLogService _logService;
        private readonly User _currentUser;

        private ObservableCollection<ActivityLogEntry> _logEntries;
        private string _logCount;

        public ObservableCollection<ActivityLogEntry> LogEntries
        {
            get => _logEntries;
            set { _logEntries = value; OnPropertyChanged(); }
        }

        public string LogCount
        {
            get => _logCount;
            set { _logCount = value; OnPropertyChanged(); }
        }

        public ICommand ClearLogCommand { get; }
        public ICommand RefreshLogCommand { get; }

        public ActivityLogViewModel(User user)
        {
            _currentUser = user;
            _logService = new ActivityLogService();

            LogEntries = new ObservableCollection<ActivityLogEntry>();
            ClearLogCommand = new RelayCommand(ExecuteClearLog);
            RefreshLogCommand = new RelayCommand(ExecuteRefreshLog);

            LoadActivities();
        }

        private void LoadActivities()
        {
            try
            {
                var activities = _logService.GetRecentActivities(_currentUser.Id, 50);
                LogEntries.Clear();
                foreach (var activity in activities)
                {
                    LogEntries.Add(activity);
                }
                LogCount = $"Total entries: {LogEntries.Count}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading activities: {ex.Message}");
            }
        }

        private void ExecuteClearLog()
        {
            try
            {
                if (_logService.ClearAllActivities(_currentUser.Id))
                {
                    LogEntries.Clear();
                    LogCount = "Log cleared";
                    _logService.LogActivity(_currentUser.Id, "Log Cleared", "User cleared activity log");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error clearing log: {ex.Message}");
            }
        }

        private void ExecuteRefreshLog()
        {
            LoadActivities();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

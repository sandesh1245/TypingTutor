using System;
using System.Threading.Tasks;

namespace TypingTutor.Services
{
    public class SyncService
    {
        private static SyncService? _instance;
        public static SyncService Instance => _instance ??= new SyncService();

        public bool IsSyncing { get; private set; }
        public DateTime? LastSyncTime { get; private set; }
        public string SyncStatusMessage { get; private set; } = "Offline Ready";

        public event Action? OnSyncStatusChanged;

        public async Task TriggerSyncAsync()
        {
            if (IsSyncing) return;

            IsSyncing = true;
            SyncStatusMessage = "Synchronizing with Cloud Profile...";
            OnSyncStatusChanged?.Invoke();

            await Task.Delay(1500); // Simulate network sync

            IsSyncing = false;
            LastSyncTime = DateTime.Now;
            SyncStatusMessage = $"Synced at {LastSyncTime.Value:HH:mm:ss}";
            OnSyncStatusChanged?.Invoke();
        }
    }
}

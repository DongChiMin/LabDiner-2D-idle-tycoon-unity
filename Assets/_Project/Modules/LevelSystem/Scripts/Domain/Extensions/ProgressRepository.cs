using LabDiner.LevelSystem.Domain;
using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.LevelSystem
{
    public static class ProgressRepository 
    {
        public const string PROGRESS_FILE_NAME = "player_progress.dat";

        public static PlayerProgress LoadProgress() 
        {
            string json = JSONExecutor.ReadFromFile(PROGRESS_FILE_NAME, true);
            if (string.IsNullOrEmpty(json)) return new PlayerProgress();
            return JsonUtility.FromJson<PlayerProgress>(json);
        }

        public static void SaveProgress(int nextLevel) 
        {
            var progress = new PlayerProgress { currentLevelIndex = nextLevel };
            JSONExecutor.WriteToFile(progress.ToJson(), PROGRESS_FILE_NAME, true);
        }
    }
}

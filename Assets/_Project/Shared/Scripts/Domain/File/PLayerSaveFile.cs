using UnityEngine;

namespace LabDiner.Shared
{
    public static class PlayerSaveFile 
    {
        public const string PROGRESS_FILE_NAME = "player_progress.dat";

        public static PlayerSave LoadFromFile() 
        {
            string json = JSONExecutor.ReadFromFile(PROGRESS_FILE_NAME, true);
            if (string.IsNullOrEmpty(json)) return new PlayerSave();
            return JsonUtility.FromJson<PlayerSave>(json);
        }

        public static void SaveToFile(PlayerSave progress) 
        {
            JSONExecutor.WriteToFile(JsonUtility.ToJson(progress, true), PROGRESS_FILE_NAME, true);
        }
    }
}

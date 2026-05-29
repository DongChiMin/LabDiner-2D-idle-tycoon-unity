using UnityEditor;
using UnityEngine;
using System.IO;
using LabDiner.Shared;

namespace LabDiner.LevelSystem
{
    public class EditorShortcuts
    {
        [MenuItem("LabDiner/Shortcuts/Open Persistent Data Path")]
        public static void OpenPersistentDataPath()
        {
            EditorUtility.RevealInFinder(Application.persistentDataPath);
        }

        [MenuItem("LabDiner/Shortcuts/Clear Save Data (Reset)")]
        public static void ClearSaveData()
        {
            string path = Path.Combine(Application.persistentDataPath, PlayerSaveFile.PROGRESS_FILE_NAME); // Tên file của cậu
            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("<color=red>Đã xóa file save để test lại!</color>");
            }
            else
            {
                Debug.Log("Không tìm thấy file save nào để xóa.");
            }
        }
    }
}
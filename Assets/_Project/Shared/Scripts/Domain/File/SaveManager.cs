using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using LabDiner.Shared.DesignPattern;
using UnityEngine;

namespace LabDiner.Shared
{
    [System.Serializable]
    public class SaveManager : Singleton<SaveManager>
    {
        [SerializeField] private float autoSaveInterval = 10f;
        private LevelProgressSave _levelProgressSave;
        private PlayerSave _playerSave;

        public void StartAutoSave(LevelProgressSave levelProgressSave, PlayerSave playerSave)
        {
            if(levelProgressSave == null || playerSave == null) return;

            _levelProgressSave = levelProgressSave;
            _playerSave = playerSave;
            StartCoroutine(AutoSave());
        }

        IEnumerator AutoSave()
        {
            while (true)
            {
                yield return new WaitForSeconds(autoSaveInterval);
                SaveProgress();
                Debug.Log("Progress auto-saved.");
            }
        }

        private void SaveProgress()
        {
            if (_playerSave.IsDirty)
            {
                PlayerSaveFile.SaveToFile(_playerSave);
                _playerSave.SetDirty(false);
            }
            if (_levelProgressSave.isDirty)
            {
                LevelProgressSaveFile.SaveToFile(_levelProgressSave);
                _levelProgressSave.SetDirty(false);
            }
        }

        #region Unity Lifecycle Events (Bẫy sự kiện tự động)

        // 1. Kích hoạt khi người chơi TẮT GAME hoàn toàn (Trên PC/Console)
        private void OnApplicationQuit()
        {
            Debug.Log("Phát hiện tắt game! Đang tự động lưu...");
            SaveProgress();
        }

        // 2. Kích hoạt khi người chơi ẨN GAME ra ngoài màn hình (Rất quan trọng trên Mobile)
        // Hoặc khi bấm nút Home, có cuộc gọi đến, hoặc mở đa nhiệm.
        private void OnApplicationFocus(bool hasFocus)
        {
            // hasFocus = false nghĩa là game bị ẩn đi (mất focus)
            if (!hasFocus)
            {
                Debug.Log("Phát hiện ẩn game (Mobile)! Đang tự động lưu...");
                SaveProgress();
            }
        }

        // 3. Tương tự như Focus nhưng dành cho một số nền tảng di động khác
        private void OnApplicationPause(bool pauseStatus)
        {
            // pauseStatus = true nghĩa là game đang bị tạm dừng hệ thống để ẩn đi
            if (pauseStatus)
            {
                Debug.Log("Phát hiện Pause hệ thống! Đang tự động lưu...");
                SaveProgress();
            }
        }

    #endregion
    }
}

using UnityEngine;
using LabDiner.LevelSystem.Domain;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Event;
using LabDiner.Shared;

namespace LabDiner.Restaurant.Managers
{
    public class LevelSpawner : MonoBehaviour
    {
        [Header("Level Load")]
        [SerializeField] private LevelRegistrySO _levelRegistry;
        [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;

        void Start()
        {
            _progressRuntimeSO.Init();
            LevelConfigSO currentLevel = LoadConfigFromFile();
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            GameObject levelInstance = Instantiate(currentLevel.LevelPrefab, transform);
            LevelLoader loader = levelInstance.GetComponent<LevelLoader>();
            loader.LoadLevel(currentLevel);
        }

        private LevelConfigSO LoadConfigFromFile()
        {
            // 1. Load tiến độ hiện tại
            PlayerSave progress = _progressRuntimeSO.PlayerSave;

            // 2. Lấy Config (Check Null ngay tại đây!)
            LevelConfigSO config = _levelRegistry.GetConfigByLevel(progress.currentLevelIndex);

            if (config == null) 
            {
                Debug.LogError($"[LevelLoader] Không tìm thấy Config cho Level {progress.currentLevelIndex}. Kiểm tra lại LevelRegistry!");
                return null;
            }

            // 3. Trả về Config
            return config;
        }
    }
}

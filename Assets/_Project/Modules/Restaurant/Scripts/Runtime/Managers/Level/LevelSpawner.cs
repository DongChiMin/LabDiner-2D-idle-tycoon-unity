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

            // 2. Lấy Config
            // 2.1. Nếu mới tạo file save (currentID rỗng) -> lấy level đầu tiên
            if(string.IsNullOrEmpty(progress.CurrentLevelID))
            {
                return _levelRegistry.GetFirstLevelConfigSO();
            }
            //2.2. Láy level theo ID
            else 
            {
                LevelConfigSO config = _levelRegistry.GetConfigByID(progress.CurrentLevelID);
                return config;
            }
        }
    }
}

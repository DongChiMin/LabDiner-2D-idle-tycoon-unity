

using System;
using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "ProgressRuntimeSet", menuName = "SO/Runtime/Progress")]
    public class ProgressSaveRuntimeSO : ScriptableObject
    {
        //Được gọi khi có dữ liệu progress được load
        public Action<ProgressSaveRuntimeSO> OnProgressInject;
        public LevelProgressSave LevelProgressSave => _levelProgressSave;
        public PlayerSave PlayerSave => _playerSave;
        
        [SerializeField] private LevelProgressSave _levelProgressSave;
        [SerializeField] private PlayerSave _playerSave;

        public void Init()
        {
            _playerSave = PlayerSaveFile.LoadFromFile();
            _levelProgressSave = LevelProgressSaveFile.LoadFromFile();
            SaveManager.Instance.StartAutoSave(_levelProgressSave, _playerSave);
        }

        public void InitializeProgress(LevelConfigSO configSO)
        {
            double levelCoin = configSO.InitialMoney;
            List<LevelMissionProgress> levelMissionProgresses = new List<LevelMissionProgress>();
            List<LevelUpgradeProgress> levelUpgradeProgresses = new List<LevelUpgradeProgress>();
            foreach (var mission in configSO.AvailableMissions)
            {
                levelMissionProgresses.Add(new LevelMissionProgress
                {
                    MissionID = mission.Id,
                    isCollected = false
                });
            }
            foreach (var upgrade in configSO.AvailableUpgrades)
            {
                levelUpgradeProgresses.Add(new LevelUpgradeProgress
                {
                    UpgradeID = upgrade.Id,
                    isPurchased = false
                });
            }

            _levelProgressSave.SetLevelIndex(configSO.LevelIndex);
            _levelProgressSave.SetLevelCoin(levelCoin);
            _levelProgressSave.SetLevelMissionProgress(levelMissionProgresses);
            _levelProgressSave.SetLevelUpgradeProgress(levelUpgradeProgresses);

        }


        // public void Clear()
        // {
        //     _levelProgressSave = null;
        //     OnValueChanged?.Invoke();
        // }

        [ContextMenu("Force Save Progress")]
        private void SaveProgress()
        {
            LevelProgressSaveFile.SaveToFile(_levelProgressSave);
            PlayerSaveFile.SaveToFile(_playerSave);
        }
    }
}
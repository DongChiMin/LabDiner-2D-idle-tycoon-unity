using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared
{
    [Serializable]
    public struct CoreStationLevel
    {
        public string CoreStationID;
        public int level;
    }

    [Serializable]
    public struct LevelMissionProgress
    {
        public string MissionID;
        public bool isCollected;
    }

    [Serializable]
    public struct LevelUpgradeProgress
    {
        public string UpgradeID;
        public bool isPurchased;
    }

    [Serializable]
    public class LevelProgressSave
    {
        public LevelProgressSave()
        {
            coreStationLevels = new List<CoreStationLevel>();
            levelMissionProgresses = new List<LevelMissionProgress>();
            levelUpgradeProgresses = new List<LevelUpgradeProgress>();
            levelIndex = 0;
            levelCoin = 0;
            hasSeenIntro = false;
            isDirty = false;
        }

        public void SetLevelMissionProgress(List<LevelMissionProgress> missionProgresses)
        {
            levelMissionProgresses = missionProgresses;
            isDirty = true;
        }

        public void SetLevelUpgradeProgress(List<LevelUpgradeProgress> upgradeProgresses)
        {
            levelUpgradeProgresses = upgradeProgresses;
            isDirty = true;
        }

        public void SetLevelCoin(double coin)
        {
            UpdateLevelCoin(coin);
            isDirty = true;
        }

        public void SetLevelIndex(int index)
        {
            levelIndex = index;
            isDirty = true;
        }

        public List<CoreStationLevel> coreStationLevels;
        public List<LevelMissionProgress> levelMissionProgresses;
        public List<LevelUpgradeProgress> levelUpgradeProgresses;
        public int levelIndex;
        public double levelCoin;
        public bool hasSeenIntro;
        public bool isDirty;

        public void UpdateCoreStationLevel(string stationID, int level)
        {
            isDirty = true;

            bool isExist = false;
            for (int i = 0; i < coreStationLevels.Count; i++)
            {
                if (coreStationLevels[i].CoreStationID == stationID)
                {
                    coreStationLevels[i] = new CoreStationLevel
                    {
                        CoreStationID = stationID,
                        level = level
                    };
                    isExist = true;
                    break;
                }
            }

            // ✨ NẾU CHƯA CÓ TRONG LIST THÌ PHẢI ADD MỚI
            if (!isExist)
            {
                coreStationLevels.Add(new CoreStationLevel
                {
                    CoreStationID = stationID,
                    level = level
                });
            }
        }

        public void UpdateLevelMission(string missionID, bool isCollected)
        {
            isDirty = true;

            bool isExist = false;
            for (int i = 0; i < levelMissionProgresses.Count; i++)
            {
                if (levelMissionProgresses[i].MissionID == missionID)
                {
                    levelMissionProgresses[i] = new LevelMissionProgress
                    {
                        MissionID = missionID,
                        isCollected = isCollected
                    };
                    isExist = true;
                    break;
                }
            }

            // ✨ NẾU CHƯA CÓ TRONG LIST THÌ PHẢI ADD MỚI
            if (!isExist)
            {
                Debug.LogError("Không tìm thấy missionID trong progress, thêm mới vào progress. MissionID: " + missionID);
                levelMissionProgresses.Add(new LevelMissionProgress
                {
                    MissionID = missionID,
                    isCollected = isCollected
                });
            }
        }

        public void UpdateLevelUpgrade(string upgradeID, bool isPurchased)
        {
            isDirty = true;

            bool isExist = false;
            for (int i = 0; i < levelUpgradeProgresses.Count; i++)
            {
                if (levelUpgradeProgresses[i].UpgradeID == upgradeID)
                {
                    levelUpgradeProgresses[i] = new LevelUpgradeProgress
                    {
                        UpgradeID = upgradeID,
                        isPurchased = isPurchased
                    };
                    isExist = true;
                    break;
                }
            }

            // ✨ NẾU CHƯA CÓ TRONG LIST THÌ PHẢI ADD MỚI
            if (!isExist)
            {
                Debug.LogError("Không tìm thấy upgradeID trong progress, thêm mới vào progress. UpgradeID: " + upgradeID);
                levelUpgradeProgresses.Add(new LevelUpgradeProgress
                {
                    UpgradeID = upgradeID,
                    isPurchased = isPurchased
                });
            }
        }

        public void UpdateLevelCoin(double coin)
        {
            levelCoin = coin;
            isDirty = true;
        }

        public void UpdateHasSeenIntro(bool hasSeen)
        {
            hasSeenIntro = hasSeen;
            isDirty = true;
        }

        public void SetDirty(bool dirty)
        {
            isDirty = dirty;
        }

    }
}
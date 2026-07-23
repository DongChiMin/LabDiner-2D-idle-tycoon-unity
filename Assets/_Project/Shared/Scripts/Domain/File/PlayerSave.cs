using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared
{
    [Serializable]
    public class TutorialSaveData
    {
        public List<string> CompletedTutorials = new List<string>();

        public TutorialSaveData()
        {

        }

        public TutorialSaveData(TutorialSaveData other)
        {
            CompletedTutorials = new List<string>(other.CompletedTutorials);
        }

        public bool IsTutorialCompleted(string tutorialId)
        {
            return CompletedTutorials.Contains(tutorialId);
        }

        public void MarkTutorialCompleted(string tutorialId)
        {
            Debug.Log("[Tutorial] Mark tutorial completed: " + tutorialId);
            if (!CompletedTutorials.Contains(tutorialId))
            {
                CompletedTutorials.Add(tutorialId);
            }
        }
    }

    public enum LevelStatus
    {
        InProgress,
        Completed,
    }

    [Serializable]
    public class PlayedLevel
    {
        public string LevelID;
        public LevelStatus Status;
        public long levelStartedTimestamp;    // Timestamp khi level được bắt đầu, dùng để tính toán thời gian chơi level
        public long levelCompletedTimestamp;    // Timestamp khi level được bắt đầu, dùng để tính toán thời gian chơi level
        public double playtimeOnline;         // Tổng thời gian chơi level, tính bằng giây, chỉ tính khi đang online
        public double playtimeOffline;        // Tổng thời gian chơi level, tính bằng giây, chỉ tính khi đang offline
    }

    [Serializable]
    public class PlayerSave
    {
        public PlayerSave()
        {
            gem = 0;
        }

        public bool IsDirty => isDirty;
        public int Gem => gem;
        public string CurrentLevelID => currentLevelID;
        public TutorialSaveData TutorialData => new TutorialSaveData(tutorialData);
        public List<PlayedLevel> PlayedLevels => new List<PlayedLevel>(playedLevels);

        public TutorialSaveData tutorialData = new TutorialSaveData();
        public List<PlayedLevel> playedLevels = new List<PlayedLevel>();
        public string currentLevelID;
        public bool isDirty = false;
        public int gem;

        // Cậu có thể thêm các thông tin khác sau này như:
        // public int totalMoney;

        public void SetDirty(bool dirty)
        {
            isDirty = dirty;
        }

        public void StartNewLevel(string ID)
        {
            currentLevelID = ID;
            PlayedLevel playedLevel = new PlayedLevel
            {
                LevelID = ID,
                Status = LevelStatus.InProgress,
                levelStartedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            playedLevels.Add(playedLevel);
            isDirty = true;
        }

        public void SetLevelCompleted(string ID)
        {
            PlayedLevel completedLevel = playedLevels.Find(level => level.LevelID == ID);
            if (completedLevel != null)
            {
                completedLevel.Status = LevelStatus.Completed;
                completedLevel.levelCompletedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            }
            else
            {
                Debug.LogWarning($"[PlayerSave] Level with ID {ID} not found in playedLevels.");
            }
            isDirty = true;
        }

        public void UpdateCurrentLevelID(string ID)
        {
            currentLevelID = ID;
            isDirty = true;
        }

        public void UpdateGem(int gemAmount)
        {
            gem = gemAmount;
            isDirty = true;
        }

    }
}
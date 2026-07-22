using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared
{
    [Serializable]
    public class TutorialSaveData
    {
        public List<string> CompletedTutorials = new List<string>();

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
        public string ID;
        public int LevelIndex;
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
            currentLevelIndex = 1;
            Gem = 0;
        }

        public TutorialSaveData tutorialData = new TutorialSaveData();
        public List<PlayedLevel> playedLevels = new List<PlayedLevel>();
        public int currentLevelIndex = 1;
        public bool isDirty = false;
        public int Gem;

        // Cậu có thể thêm các thông tin khác sau này như:
        // public int totalMoney;

        public void SetDirty(bool dirty)
        {
            isDirty = dirty;
        }

        public void StartNewLevel(int index, string ID)
        {
            currentLevelIndex = index;
            PlayedLevel playedLevel = new PlayedLevel
            {
                ID = ID,
                LevelIndex = index,
                Status = LevelStatus.InProgress,
                levelStartedTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
            playedLevels.Add(playedLevel);
            isDirty = true;
        }

        public void SetLevelCompleted(int index, string ID)
        {
            currentLevelIndex = index + 1;
            PlayedLevel completedLevel = playedLevels.Find(level => level.ID == ID);
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

        public void UpdateGem(int gemAmount)
        {
            Gem = gemAmount;
            isDirty = true;
        }
    }
}
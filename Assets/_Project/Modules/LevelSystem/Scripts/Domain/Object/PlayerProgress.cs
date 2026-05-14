using System;

namespace LabDiner.LevelSystem.Domain
{
    [Serializable]
    public class PlayerProgress
    {
        public PlayerProgress()
        {
            currentLevelIndex = 1;
            hasSeenIntro = false;
        }

        public int currentLevelIndex = 1;
        public bool hasSeenIntro = false;
        
        // Cậu có thể thêm các thông tin khác sau này như:
        // public int totalMoney;
    }
}
using System;

namespace LabDiner.Shared
{
    [Serializable]
    public class PlayerSave
    {
        public PlayerSave()
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
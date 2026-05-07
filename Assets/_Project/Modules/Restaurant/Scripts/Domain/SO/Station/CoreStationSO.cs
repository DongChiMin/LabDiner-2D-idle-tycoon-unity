using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "CoreStation", menuName = "Game/Station/CoreStation")]
    public class CoreStationSO : ScriptableObject
    {
        public string Id => name; // Sử dụng tên của ScriptableObject làm ID

        [Header("Basic Info")]
        public DishSO Dish;

        [Header("Level Upgrade")]
        public int LevelPerStar = 10;
        public List<StationStarSO> StationStars;

        [Header("Currency")]
        public double BaseProfit = 10;

        public float BaseProcessTime = 5f;
        
        public double BaseUpgradeCost = 15;
        public float CostMultiplier = 1.07f;
        public float ProfitMultiplier = 1.15f;

        [Header("[DEBUG]")]
        [ReadOnly] public int MaxLevel;
        [ReadOnly] public int MaxStar;
        [ReadOnly] public int MaxQuantity;

        private void OnValidate()
        {
            // Tự động tính toán lại mỗi khi thay đổi giá trị trong Inspector
            MaxStar = StationStars.Count;
            MaxLevel = MaxStar * LevelPerStar;
            
            MaxQuantity = 1;
            foreach(var star in StationStars)
            {
                List<StationStarBuffSO> buffs = star.Buffs;
                foreach(var buff in buffs)
                {
                    if(buff.EffectType == StationStarBuffType.CreateNewStation)
                    {
                        MaxQuantity++;
                    }
                }
            }
        }
    }
}
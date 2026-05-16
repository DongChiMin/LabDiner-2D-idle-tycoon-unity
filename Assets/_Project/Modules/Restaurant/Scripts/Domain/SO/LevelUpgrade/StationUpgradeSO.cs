using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum StationUpgradeType
    {
        CookingSpeed,      // Nấu nhanh hơn
        Profit,           // Tăng profit của món ăn
    }

    [CreateAssetMenu(fileName = "New Station Upgrade", menuName = "Game/Upgrades/Station Upgrade")]
    public class StationUpgradeSO : BaseUpgradeSO 
    {
        [Header("Station Upgrade")]
        public StationUpgradeType UpgradeType;
        [Tooltip("Bỏ trống nếu muốn áp dụng cho tất cả món ăn")]
        public DishSO TargetDish;

        [Header("Static")]
        public StationUpgradeEvent OnUpgradeRaised;

        public override void ApplyUpgrade()
        {
            // Gửi chính Asset này đi qua Event
            if (OnUpgradeRaised != null)
                OnUpgradeRaised.Raise(this);
        }
    }
}
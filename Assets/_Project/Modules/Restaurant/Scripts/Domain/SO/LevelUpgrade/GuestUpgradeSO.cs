using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum GuestUpgradeType
    {
        Quantity,   // Tăng số lượng khách có thể xuất hiện cùng lúc
        SpawnTime // Tăng tốc độ xuất hiện của khách hàng (giảm thời gian giữa các lần spawn)
    }

    [CreateAssetMenu(fileName = "New Guest Upgrade", menuName = "Game/Upgrades/Guest Upgrade")]
    public class GuestUpgradeSO : BaseUpgradeSO 
    {
        [Header("Guest Upgrade")]
        public GuestUpgradeType UpgradeType;

        [Header("Static")]
        public GuestUpgradeEvent OnUpgradeRaised;

        public override void ApplyUpgrade()
        {
            // Gửi chính Asset này đi qua Event
            if (OnUpgradeRaised != null)
                OnUpgradeRaised.Raise(this);
        }
    }
}
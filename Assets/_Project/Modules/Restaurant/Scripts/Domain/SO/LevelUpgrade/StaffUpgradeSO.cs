using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    
     public enum StaffUpgradeType
    {
        CookingSpeed,      // Nấu nhanh hơn
        MoveSpeed,    // Nhân viên chạy nhanh
        Quantity       // Tăng số lượng nhân viên có thể thuê
    }


    [CreateAssetMenu(fileName = "New Staff Upgrade", menuName = "Game/Upgrades/Staff Upgrade")]
    public class StaffUpgradeSO : BaseUpgradeSO 
    {
        [Header("Staff Upgrade")]
        public StaffUpgradeType UpgradeType;
        public StaffType Target;

        [Header("Static")]
        public StaffUpgradeEvent OnUpgradeRaised;

        public override void ApplyUpgrade()
        {
            // Gửi chính Asset này đi qua Event
            if (OnUpgradeRaised != null)
                OnUpgradeRaised.Raise(this);
        }
    }
}
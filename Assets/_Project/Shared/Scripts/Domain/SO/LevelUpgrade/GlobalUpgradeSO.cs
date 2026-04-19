
using UnityEngine;

namespace LabDiner.Shared.SO
{
    
    // Nhóm 2: Nâng cấp dành riêng cho món ăn
    //- Tăng profit
    //- Giảm thời gian nấu
    [CreateAssetMenu(fileName = "New Global Upgrade", menuName = "Game/Upgrades/Global Upgrade")]
    public class GlobalUpgradeSO : BaseUpgradeSO 
    {
        [Header("Events")]
        public GlobalUpgradeEvent OnUpgradeRaised;

        public override void ApplyUpgrade()
        {
            // Gửi chính Asset này đi qua Event
            if (OnUpgradeRaised != null)
                OnUpgradeRaised.Raise(this);
        }
    }
}
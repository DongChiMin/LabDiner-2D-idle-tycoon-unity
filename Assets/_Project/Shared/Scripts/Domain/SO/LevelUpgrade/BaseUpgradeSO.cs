using UnityEngine;

namespace LabDiner.Shared.SO
{
    // public enum UpgradeType
    // {
    //     CookingSpeed,      // Nấu nhanh hơn
    //     StaffMoveSpeed,    // Nhân viên chạy nhanh
    //     ChefQuantity,      // + Số lượng đầu bếp
    //     MultitaskChefQuantity, // + Số lượng đầu bếp có thể làm nhiều món cùng lúc
    //     WaiterQuantity,    // + Số lượng phục vụ
    //     GuestCapacity,     // + Số lượng khách
    //     DishProfitMultiplier,    // x3 Profit 1 món (cần thêm ID món)
    //     GlobalProfitMultiplier   // x3 Profit toàn bộ
    // }

    [CreateAssetMenu(fileName = "New Level Upgrade", menuName = "Game/Upgrades/Level Upgrade")]
    public class BaseUpgradeSO : ScriptableObject
    {
        [Header("Upgrade Info")]
        public string Title;
        [TextArea] public string Description;
        public Sprite Icon;
        public Sprite UpgradeTypeSprite;

        [Header("Upgrade Effect")]
        public double UpgradeCost;
        public float UpgradeValue;
        public LevelUpgradeEvent OnUpgradeRaised;

        public void ApplyUpgrade()
        {
            // Gửi chính Asset này đi qua Event
            if (OnUpgradeRaised != null)
                OnUpgradeRaised.Raise(this);
        }
    }
}
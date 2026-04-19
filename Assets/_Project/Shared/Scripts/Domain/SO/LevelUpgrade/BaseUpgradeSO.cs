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

    public abstract class BaseUpgradeSO : ScriptableObject
    {
        [Header("Upgrade Info")]
        public string Title;
        [TextArea] public string Description;
        public Sprite Icon;
        public Sprite UpgradeTypeSprite;

        [Header("Upgrade Effect")]
        public double UpgradeCost;
        [Tooltip("Giá trị nâng cấp dựa theo Event\n- dishProcessTime: 1 (100%) --> Tăng gấp đôi tốc độ nấu\n- dishProfit: 2 --> tăng gấp đôi profit\n- Quantity: cộng số lượng nhân viên")]
        public float UpgradeValue;

        public abstract void ApplyUpgrade();
    }
}
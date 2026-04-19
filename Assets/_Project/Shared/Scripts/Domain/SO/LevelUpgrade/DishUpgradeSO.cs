
using UnityEngine;

namespace LabDiner.Shared.SO
{
    
    // Nhóm 2: Nâng cấp dành riêng cho món ăn
    //- Tăng profit
    //- Giảm thời gian nấu
    [CreateAssetMenu(fileName = "New Dish Upgrade", menuName = "Game/Upgrades/Dish Upgrade")]
    public class DishUpgradeSO : BaseUpgradeSO 
    {
        [Header("Dish Effect")]
        public DishSO Dish;
    }
}
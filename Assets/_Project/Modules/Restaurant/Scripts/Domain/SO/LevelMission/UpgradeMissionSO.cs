using System.Linq;
using LabDiner.Restaurant.Manager;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{

    /// <summary>
    /// Nhiệm vụ nâng cấp
    /// </summary>
    [CreateAssetMenu(fileName = "New Upgrade Mission", menuName = "Game/Missions/Upgrade Mission")]
    public class UpgradeMissionSO : BaseMissionSO
    {
        [Header("Target (optional)")]
        [Tooltip("Nếu để trống, nhiệm vụ sẽ tính theo tổng số upgrade đã hoàn thành")]
        public BaseUpgradeSO TargetUpgrade;
        
        [Header("Static")]
        public LevelUpgradeRuntimeSO levelUpgradeRuntimeSO;

        void OnEnable()
        {
            if(levelUpgradeRuntimeSO != null)
            {
                levelUpgradeRuntimeSO.OnValueChanged += HandleValueChanged;
            }
        }

        void OnDisable()
        {
            if(levelUpgradeRuntimeSO != null)
            {
                levelUpgradeRuntimeSO.OnValueChanged -= HandleValueChanged;
            }
        }

        private void HandleValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public override float GetCurrentValue()
        {
            if(TargetUpgrade != null)
            {
                return levelUpgradeRuntimeSO.CompletedUpgrades.Count(upgrade => upgrade == TargetUpgrade);
            }
            else 
            {
                return levelUpgradeRuntimeSO.CompletedUpgrades.Count;
            }
        }
    }
}
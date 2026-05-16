using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "LevelUpgradeRuntime", menuName = "SO/Runtime/LevelUpgrade")]
    public class LevelUpgradeRuntimeSO : ScriptableObject
    {
        //Được gọi khi có upgrade được apply
        public System.Action OnValueChanged;

        public List<BaseUpgradeSO> ActiveUpgrades => _activeUpgrades;
        public List<BaseUpgradeSO> CompletedUpgrades => _completedUpgrades;

        // Danh sách tất cả các ghế đã được spawn, được quản lý bởi DiningTableManager
        private List<BaseUpgradeSO> _activeUpgrades = new List<BaseUpgradeSO>();
        private List<BaseUpgradeSO> _completedUpgrades = new List<BaseUpgradeSO>();

        public void Complete(BaseUpgradeSO upgrade)
        {
            if (_activeUpgrades.Contains(upgrade))
            {
                _activeUpgrades.Remove(upgrade);
                if (!_completedUpgrades.Contains(upgrade)) _completedUpgrades.Add(upgrade);
            }
            OnValueChanged?.Invoke();
        }

        public void Add(BaseUpgradeSO upgrade)
        {
            if (!_activeUpgrades.Contains(upgrade)) _activeUpgrades.Add(upgrade);
        }

        public void Remove(BaseUpgradeSO upgrade)
        {
            if (_activeUpgrades.Contains(upgrade)) _activeUpgrades.Remove(upgrade);
        }

        // Reset danh sách khi game bắt đầu/kết thúc
        public void Clear()
        {
            _activeUpgrades.Clear();
            _completedUpgrades.Clear();
        }
    }
}
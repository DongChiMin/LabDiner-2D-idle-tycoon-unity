using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.LevelTutorial
{
    [Tooltip("start highlight khi người chơi đủ tiền để nâng cấp station lần đầu tiên \n Bước 2. Highlight vào nút nâng cấp")]
    public class TutorialUpgradetation2 : BaseTutorialAnchor
    {
        [SerializeField] private CoreStation _coreStation;
        [SerializeField] private CoreStationUI _coreStationUI;
        void OnEnable()
        {
            _coreStation.OnCoreStationUpgradable += HandleCoreStationUpgradable;
            _coreStationUI.OnUpgradeUIShow += HandleCoreStationUpgradable;
            _coreStationUI.OnUpgradeButtonClicked += CompleteTutorial;
        }

        void OnDisable()
        {
            _coreStation.OnCoreStationUpgradable -= HandleCoreStationUpgradable;
            _coreStationUI.OnUpgradeUIShow -= HandleCoreStationUpgradable;
            _coreStationUI.OnUpgradeButtonClicked -= CompleteTutorial;
        }

        private void HandleCoreStationUpgradable()
        {
            bool isOpenedUI = _coreStationUI.gameObject.activeSelf;
            bool canUpgrade = _coreStation.CanUpgrade;
            if(canUpgrade && isOpenedUI)
            {
                StartTutorial();
            }
        }
    }
}
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.LevelTutorial
{
    [Tooltip("start highlight khi người chơi đủ tiền để nâng cấp station lần đầu tiên \n Bước 1. Highlight vào coreStation")]
    public class TutorialUpgradetation1 : BaseTutorialAnchor
    {
        [SerializeField] private CoreStation _coreStation;
        [SerializeField] private CoreStationUI _coreStationUI;
        void OnEnable()
        {
            _coreStation.OnCoreStationUpgradable += HandleCoreStationUpgradable;
            _coreStationUI.OnUpgradeButtonClicked += CompleteTutorial;
        }

        void OnDisable()
        {
            _coreStation.OnCoreStationUpgradable -= HandleCoreStationUpgradable;
            _coreStationUI.OnUpgradeButtonClicked -= CompleteTutorial;
        }

        private void HandleCoreStationUpgradable()
        {
            bool isOpenedUI = _coreStationUI.gameObject.activeSelf;
            if(!isOpenedUI)
            {
                StartTutorial();
            }
            else
            {
                Debug.Log("[Tutorial] TutorialUpgradetation1: CoreStation UI is already opened, completing tutorial.");
                CompleteTutorial();
            }
        }
    }
}
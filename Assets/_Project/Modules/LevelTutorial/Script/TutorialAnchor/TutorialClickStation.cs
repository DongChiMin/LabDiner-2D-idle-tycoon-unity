using LabDiner.Restaurant.Environment;
using UnityEngine;

namespace LabDiner.LevelTutorial
{
    [Tooltip("start highlight bếp ko cần điều kiện, hoàn thành khi mở khóa bếp, nâng lên level 1")]
    public class TutorialClickStation : BaseTutorialAnchor
    {
        [SerializeField] private CoreStation _coreStation;

        void OnEnable()
        {
            StartTutorial();    //không cần điều kiện 
            _coreStation.OnCoreStationClicked += InteractTutorial;
            _coreStation.OnCoreStationUnlocked += CompleteTutorial;
        }

        void OnDisable()
        {
            _coreStation.OnCoreStationClicked -= InteractTutorial;
            _coreStation.OnCoreStationUnlocked -= CompleteTutorial;
        }
    }
}
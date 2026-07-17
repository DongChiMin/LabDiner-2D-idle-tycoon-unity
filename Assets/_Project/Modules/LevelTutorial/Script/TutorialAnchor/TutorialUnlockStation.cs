using LabDiner.Restaurant.Environment;
using UnityEngine;

namespace LabDiner.LevelTutorial
{
    [Tooltip("start highlight khi bếp được click để mở UI, hoàn thành khi mở khóa bếp, nâng lên level 1")]
    public class TutorialUnlockStation : BaseTutorialAnchor
    {
        [SerializeField] private CoreStation _coreStation;

        void OnEnable()
        {
            Debug.Log("[Tutorial] TutorialUnlockStation OnEnable");
            StartTutorial();    //không cần điều kiện vì ban đầu gameObject này mặc định tắt, khi được click vào bếp thì sẽ mở
            _coreStation.OnCoreStationUnlocked += CompleteTutorial;
        }

        void OnDisable()
        {
            _coreStation.OnCoreStationClicked -= StartTutorial;
            _coreStation.OnCoreStationUnlocked -= CompleteTutorial;
        }
    }
}
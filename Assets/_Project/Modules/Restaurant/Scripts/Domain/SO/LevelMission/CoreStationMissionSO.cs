using System.Linq;
using LabDiner.Restaurant.Manager;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum CoreStationMissionType
    {
        UpgradeCoreStation,      // Nâng cấp core station a đến level b
    }

    /// <summary>
    /// Nhiệm vụ yêu cầu nâng cấp level của một core Station cụ thể lên một mốc nhất định nào đó (ví dụ: nâng cấp Core Station Burger lên level 10)
    /// </summary>
    [CreateAssetMenu(fileName = "New CoreStation Mission", menuName = "Game/Missions/CoreStation Mission")]
    public class CoreStationMissionSO : BaseMissionSO
    {
        [Header("Target")]
        public CoreStationSO TargetCoreStation;
        public CoreStationMissionType MissionType;
        
        [Header("Static")]
        public CoreStationRuntimeSO coreStationRuntimeSO;

        void OnEnable()
        {
            if(coreStationRuntimeSO != null)
            {
                coreStationRuntimeSO.OnValueChanged += HandleValueChanged;
            }
        }

        void OnDisable()
        {
            if (coreStationRuntimeSO != null)
            {
                coreStationRuntimeSO.OnValueChanged -= HandleValueChanged;
            }
        }

        private void HandleValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public override float GetCurrentValue()
        {
            switch (MissionType)
            {
                case CoreStationMissionType.UpgradeCoreStation:
                    //Lấy level hiện tại của core station tương ứng từ CoreStationRuntimeSO
                    int currentLevel = coreStationRuntimeSO.GetCoreStationLevel(TargetCoreStation);
                    return currentLevel;
                default:
                    Debug.LogError("Unsupported Mission Type");
                    return 0;
            }
            
        }
    }
}
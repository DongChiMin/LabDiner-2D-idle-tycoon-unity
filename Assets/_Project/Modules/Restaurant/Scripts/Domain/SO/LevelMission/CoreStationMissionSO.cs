using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Manager;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum CoreStationMissionType
    {
        UpgradeStation,      // Nâng cấp core station a đến level b
        UnlockStation,      // Mở khóa core station a
        MaxLevelAllStation  // Tất cả core station đạt max level
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
                coreStationRuntimeSO.OnAnyStationChanged += HandleValueChanged;
            }
        }

        void OnDisable()
        {
            if (coreStationRuntimeSO != null)
            {
                coreStationRuntimeSO.OnAnyStationChanged -= HandleValueChanged;
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
                case CoreStationMissionType.UpgradeStation:
                    //Lấy level hiện tại của core station tương ứng từ CoreStationRuntimeSO
                    int currentLevel = coreStationRuntimeSO.GetCoreStationLevel(TargetCoreStation);
                    return currentLevel;
                case CoreStationMissionType.UnlockStation:
                    // Kiểm tra xem core station có được mở khóa không
                    return coreStationRuntimeSO.IsCoreStationUnlocked(TargetCoreStation) ? 1 : 0;
                case CoreStationMissionType.MaxLevelAllStation:
                    List<CoreStation> coreStations = coreStationRuntimeSO.CoreStations;

                    int totalCoreStationCurrentLevel = coreStations.Sum(s => s.CurrentLevel);
                    int totalCoreStationMaxLevel = coreStations.Sum(s => s.CoreStationSO.LevelPerStar * s.CoreStationSO.StationStars.Count);

                    return totalCoreStationCurrentLevel/(float) totalCoreStationMaxLevel;
                default:
                    Debug.LogError("Unsupported Mission Type");
                    return 0;
            }
            
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            switch(MissionType)
            {
                case CoreStationMissionType.UnlockStation:
                    TargetValue = 0;
                    break;
                case CoreStationMissionType.MaxLevelAllStation:
                    TargetValue = 0;
                    TargetCoreStation = null;
                    break;
            }
        }
        #endif
    }
}
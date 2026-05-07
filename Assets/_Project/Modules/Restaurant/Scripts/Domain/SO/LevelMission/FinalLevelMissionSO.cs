using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Manager;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum FinalMissionType
    {
        AllCoreStationLevel, // Tổng level của tất cả core Station đạt mốc nhất định nào đó
    }

    /// <summary>
    /// Nhiệm vụ yêu cầu nâng cấp level của tất cả core Station trong level lên một mốc nhất định nào đó (ví dụ: tổng level của tất cả core Station đạt 50)
    /// Value của nhiệm vụ theo dạng %
    /// </summary>
    [CreateAssetMenu(fileName = "New Final Level Mission", menuName = "Game/Missions/Final Level Mission")]
    public class FinalLevelMissionSO : BaseGemMissionSO
    {
        [Header("Target")]
        public FinalMissionType MissionType;
        public override float GetCurrentValue()
        {
            List<CoreStation> coreStations = LevelManagerContext.Instance.CoreStationManager.CoreStations;

            int totalCoreStationCurrentLevel = coreStations.Sum(s => s.CurrentLevel);
            int totalCoreStationMaxLevel = coreStations.Sum(s => s.CoreStationSO.LevelPerStar * s.CoreStationSO.StationStars.Count);

            return totalCoreStationCurrentLevel/(float) totalCoreStationMaxLevel;
        }

        //Mặc định khi tạo SO sẽ để TargetValue là 1 (tức là 100%)
        protected virtual void Reset()
        {
            TargetValue = 1;
        }
    }
}
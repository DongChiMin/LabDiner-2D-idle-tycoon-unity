using System.Linq;
using LabDiner.Restaurant.Manager;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    /// <summary>
    /// Nhiệm vụ yêu cầu nâng cấp level của một core Station cụ thể lên một mốc nhất định nào đó (ví dụ: nâng cấp Core Station Burger lên level 10)
    /// </summary>
    [CreateAssetMenu(fileName = "New CoreStation Mission", menuName = "Game/Missions/CoreStation Mission")]
    public class CoreStationLevelMissionSO : BaseGemMissionSO
    {
        [Header("Target")]
        public CoreStationSO TargetCoreStation;

        public override float GetCurrentValue()
        {
            var station = LevelManagerContext.Instance.CoreStationManager.CoreStations
            .FirstOrDefault(s => s.CoreStationSO == TargetCoreStation);
            return station != null ? station.CurrentLevel : 0;
        }
    }
}
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    /// <summary>
    /// 
    /// </summary>
    [CreateAssetMenu(fileName = "New CoreStation Mission", menuName = "Game/Missions/CoreStation Mission")]
    public class CoreStationLevelMissionSO : BaseGemMissionSO
    {
        [Header("Target")]
        public CoreStationSO TargetCoreStation;
    }
}
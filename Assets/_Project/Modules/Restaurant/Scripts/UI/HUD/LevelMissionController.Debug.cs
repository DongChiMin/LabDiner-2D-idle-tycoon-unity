using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.UI;
using UnityEngine;

#if UNITY_EDITOR
namespace LabDiner.Restaurant.UI
{
    public partial class LevelMissionController : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private CoreStationRuntimeSO _coreStationRuntime;

        #region EDITOR ONLY
        #if UNITY_EDITOR
        private partial void Debug_ValidateData()
        {
            List<CoreStation> coreStations = _coreStationRuntime.CoreStations;

            foreach(BaseMissionSO mission in _remainingMissions)
            {
                if (mission is CoreStationMissionSO coreStationMission)
                {
                    CoreStationSO targetCoreStation = coreStationMission.TargetCoreStation;
                    var targetStation = coreStations.FirstOrDefault(s => s.CoreStationSO == targetCoreStation);
                    if(targetStation != null)
                    {
                        CoreStationSO stationSO = targetStation.CoreStationSO;
                        if(coreStationMission.TargetValue > stationSO.MaxLevel)
                        {
                            Debug.LogError($"[LevelMissionController] Nhiệm vụ {mission.Title} yêu cầu level vượt quá level tối đa của coreStation ({stationSO.MaxLevel}), vui lòng điều chỉnh lại hoặc nâng cấp trạm chính này trong data để tránh lỗi khi chạy game!");
                        }
                    }
                    else
                    {
                        Debug.LogError($"[LevelMissionController] Không tìm thấy trạm chính nào phù hợp với yêu cầu của nhiệm vụ {mission.Title}, vui lòng điều chỉnh lại data của nhiệm vụ này để tránh lỗi khi chạy game!");
                    }
                }
            }
        }
        #endif
        #endregion
    }
}
#endif
using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public class LevelMissionController : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private LevelMissionHUD _missionHUD;
        [SerializeField] private CoreStationEvent _onCoreStationLevelUpgraded;
        [SerializeField] private ToggleAttentionEffect _attentionEffect;
        [SerializeField] private Transform _rewardStartPos;

        [Header("[DEBUG]")]
        [SerializeField] private List<BaseGemMissionSO> _missions = new List<BaseGemMissionSO>();
        [SerializeField] private List<BaseGemMissionSO> _remainingMissions = new List<BaseGemMissionSO>();
        [SerializeField] BaseGemMissionSO _currentMission;

        void OnEnable()
        {
            _onCoreStationLevelUpgraded.Register(HandleCoreStationLevelUpgraded);
            _missionHUD.OnRewardClaimed += HandleRewardClaim;
        }

        void OnDisable()
        {
            _onCoreStationLevelUpgraded.Unregister(HandleCoreStationLevelUpgraded);
            _missionHUD.OnRewardClaimed -= HandleRewardClaim;
        }

        public void Init(LevelConfigSO config)
        {
            _missions = new List<BaseGemMissionSO>(config.AvailableMissions);
            _remainingMissions = new List<BaseGemMissionSO>(config.AvailableMissions);
            PopMission();
            
            #if UNITY_EDITOR
            ValidateData();
            #endif
        }

        #region Private Methods

        private void PopMission()
        {
            _attentionEffect.ToggleAttention(false);
            if (_remainingMissions.Count == 0)
            {
                Debug.Log("TODO: No more missions!");
                _currentMission = null;
                _missionHUD.gameObject.SetActive(false);
                return;
            }
            
            _currentMission = _remainingMissions[0];
            int currentProgress = GetMissionProgress(_currentMission);
            _missionHUD.Setup(_currentMission, currentProgress, onCompleted: () =>
            {
                _attentionEffect.ToggleAttention(true);
            });
            _remainingMissions.RemoveAt(0);
        }

        private void HandleCoreStationLevelUpgraded(CoreStation station)
        {
            //Nếu CoreStation vừa upgrade là nhiệm vụ hiện tại
            if(_currentMission != null &&_currentMission is CoreStationLevelMissionSO coreStationMission)
            {
                if (coreStationMission.TargetCoreStation == station.CoreStationSO)
                {
                    _missionHUD.UpdateProgress(station.CurrentLevel, Mathf.RoundToInt(coreStationMission.MissionValue), () =>
                    {
                        _attentionEffect.ToggleAttention(true);
                    });
                }
            }
        }

        private int GetMissionProgress(BaseGemMissionSO mission)
        {
            if (mission is CoreStationLevelMissionSO coreStationMission)
            {
                CoreStation targetStation = LevelManagerContext.Instance.CoreStationManager.CoreStations
                    .FirstOrDefault(s => s.CoreStationSO == coreStationMission.TargetCoreStation);
                if (targetStation != null)
                {
                    return targetStation.CurrentLevel;
                }
            }

            Debug.LogError($"[LevelMissionController] Không tìm thấy progress nào phù hợp với loại nhiệm vụ {mission.Title}, vui lòng kiểm tra lại logic của method GetMissionProgress để đảm bảo nó trả về progress chính xác cho từng loại nhiệm vụ!");
            return 0;
        }

        private void HandleRewardClaim()
        {
            if (_currentMission != null)
            {
                _currentMission.ApplyReward(_rewardStartPos.position);
                PopMission();
            }
            else
            {
                Debug.LogError("[LevelMissionController] Người chơi đang cố gắng nhận thưởng nhưng không có nhiệm vụ nào đang hoạt động, vui lòng kiểm tra lại logic của method HandleRewardClaim để đảm bảo nó chỉ được gọi khi có một nhiệm vụ hợp lệ đang hoạt động!");
            }
        }

        #endregion

        #region EDITOR ONLY

        #if UNITY_EDITOR

        private void ValidateData()
        {
            List<CoreStation> coreStations = LevelManagerContext.Instance.CoreStationManager.CoreStations;

            foreach(BaseGemMissionSO mission in _missions)
            {
                if (mission is CoreStationLevelMissionSO coreStationMission)
                {
                    CoreStationSO targetCoreStation = coreStationMission.TargetCoreStation;
                    var targetStation = coreStations.FirstOrDefault(s => s.CoreStationSO == targetCoreStation);
                    if(targetStation != null)
                    {
                        CoreStationSO stationSO = targetStation.CoreStationSO;
                        if(coreStationMission.MissionValue > stationSO.MaxLevel)
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

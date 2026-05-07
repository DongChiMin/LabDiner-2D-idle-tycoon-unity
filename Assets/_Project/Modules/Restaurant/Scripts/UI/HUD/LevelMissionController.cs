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
        [Header("Events")]
        [SerializeField] private LevelCompleteEvent _onLevelComplete;
        [SerializeField] private CoreStationEvent _onCoreStationLevelUpgraded;

        [Header("Item References")]
        [SerializeField] private LevelMissionHUD _missionHUD;
        [SerializeField] private ToggleAttentionEffect _attentionEffect;
        [SerializeField] private Transform _rewardStartPos;

        [Header("[DEBUG]")]
        [SerializeField] private List<BaseGemMissionSO> _remainingMissions = new List<BaseGemMissionSO>();
        [SerializeField] BaseGemMissionSO _currentMission;
        [SerializeField] private BaseGemMissionSO _finalMission;

        void OnEnable()
        {
            _onCoreStationLevelUpgraded.Register(HandleProgressUpdate);
            _missionHUD.OnRewardClaimed += HandleRewardClaim;
        }

        void OnDisable()
        {
            _onCoreStationLevelUpgraded.Unregister(HandleProgressUpdate);
            _missionHUD.OnRewardClaimed -= HandleRewardClaim;
        }

        public void Init(LevelConfigSO config)
        {
            _remainingMissions = new List<BaseGemMissionSO>(config.AvailableMissions);
            _finalMission = config.FinalMission;

            //Khởi động nhiệm vụ đầu tiên
            ActivateNextMission();

            #if UNITY_EDITOR
            ValidateData();
            #endif
        }

        #region Private Methods
        private void ActivateNextMission()
        {
            _attentionEffect.ToggleAttention(false);

            if (_remainingMissions.Count > 0)
            {
                _currentMission = _remainingMissions[0];
                _remainingMissions.RemoveAt(0);
                _missionHUD.ToggleProgressText(true);
            }
            else
            {
                _currentMission = _finalMission;
                _missionHUD.ToggleProgressText(false);
            }

            UpdateMissionUI();
        }

        private void UpdateMissionUI()
        {
            if (_currentMission == null) return;
            
            float val = _currentMission.GetCurrentValue();
            _missionHUD.Setup(_currentMission, () => _attentionEffect.ToggleAttention(true));
        }

        private void HandleProgressUpdate(CoreStation station)
        {
            if (_currentMission == null) return;

            // Update UI cho nhiệm vụ hiện tại
            _missionHUD.UpdateProgress(() => _attentionEffect.ToggleAttention(true));

            // Logic tracking nhiệm vụ cuối (nếu là dạng AllCoreStation)
            CheckFinalMissionProgress();
        }

        private void CheckFinalMissionProgress()
        {
            if (_finalMission == null)
            {
                Debug.LogError("[LevelMissionController] Nhiệm vụ cuối đang bị thiếu trong config của level này, vui lòng kiểm tra lại để tránh lỗi khi chạy game!");
                return;
            }

            if (_finalMission.IsCompleted())
            {
                CompleteLevel();
            }
        }

        private void HandleRewardClaim()
        {
            if (_currentMission == null) return;

            _currentMission.ApplyReward(_rewardStartPos.position);
            
            if (_currentMission == _finalMission)
            {
                CompleteLevel();
            }
            else
            {
                ActivateNextMission();
            }   
        }

        private void CompleteLevel()
        {
            //Nhận thưởng cho nhiệm vụ cuối
            _finalMission.ApplyReward(_rewardStartPos.position);

            // Tự động nhận hết phần thưởng còn lại trong queue nếu có
            while (_remainingMissions.Count > 0)
            {
                _remainingMissions[0].ApplyReward(_rewardStartPos.position);
                _remainingMissions.RemoveAt(0);
            }

            _onLevelComplete.Raise(0);
            _missionHUD.gameObject.SetActive(false);
        }
        #endregion


        #region EDITOR ONLY
        #if UNITY_EDITOR
        private void ValidateData()
        {
            List<CoreStation> coreStations = LevelManagerContext.Instance.CoreStationManager.CoreStations;

            foreach(BaseGemMissionSO mission in _remainingMissions)
            {
                if (mission is CoreStationLevelMissionSO coreStationMission)
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

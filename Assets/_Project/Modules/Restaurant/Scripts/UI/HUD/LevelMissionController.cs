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
    public partial class LevelMissionController : MonoBehaviour, ILevelInitializable
    {
        [Header("Events")]
        [SerializeField] private IntEvent _onLevelComplete;
        [SerializeField] private CoreStationEvent _onCoreStationLevelUpgraded;
        [SerializeField] private LevelConfigEvent _onLevelInit;

        [Header("Item References")]
        [SerializeField] private LevelMissionHUD _missionHUD;
        [SerializeField] private ToggleAttentionEffect _attentionEffect;
        [SerializeField] private Transform _rewardStartPos;

        [Header("[Runtime]")]
        [SerializeField] private List<BaseGemMissionSO> _remainingMissions = new List<BaseGemMissionSO>();
        [SerializeField] BaseGemMissionSO _currentMission;
        [SerializeField] private BaseGemMissionSO _finalMission;

        private int _currentLevel = -1;

        void OnEnable()
        {
            _onLevelInit.Register(Init);
            _onCoreStationLevelUpgraded.Register(HandleProgressUpdate);
            _missionHUD.OnRewardClaimed += HandleRewardClaim;
        }

        void OnDisable()
        {
            _onLevelInit.Unregister(Init);
            _onCoreStationLevelUpgraded.Unregister(HandleProgressUpdate);
            _missionHUD.OnRewardClaimed -= HandleRewardClaim;
        }

        public void Init(LevelConfigSO config)
        {
            _remainingMissions = new List<BaseGemMissionSO>(config.AvailableMissions);
            _finalMission = config.FinalMission;
            _currentLevel = config.LevelIndex;

            //Khởi động nhiệm vụ đầu tiên
            ActivateNextMission();

            Debug_ValidateData();
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

            _onLevelComplete.Raise(_currentLevel);
            _missionHUD.gameObject.SetActive(false);
        }
        #endregion

        private partial void Debug_ValidateData();
    }
}

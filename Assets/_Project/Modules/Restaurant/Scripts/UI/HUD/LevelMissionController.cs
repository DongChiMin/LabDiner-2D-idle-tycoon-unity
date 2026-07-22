using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Shared;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public partial class LevelMissionController : MonoBehaviour, ILevelInitializable, ILevelProgress
    {
        [Header("Events")]
        [SerializeField] private LevelConfigEvent _onLevelComplete;
        [SerializeField] private LevelConfigEvent _onLevelInit;

        [Header("Item References")]
        [SerializeField] private LevelMissionHUD _missionHUD;
        [SerializeField] private ToggleAttentionEffect _attentionEffect;
        [SerializeField] private Transform _rewardStartPos;
        [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;

        [Header("[Runtime]")]
        [SerializeField] private List<BaseMissionSO> _remainingMissions = new List<BaseMissionSO>();
        [SerializeField] BaseMissionSO _currentMission;

        private LevelConfigSO _currentLevelConfigSO;
        private bool _isFinalMission = false;

        void OnEnable()
        {
            _onLevelInit.Register(Init);
            _missionHUD.OnRewardClaimed += HandleRewardClaim;
            if(_currentMission != null) _currentMission.OnValueChanged += HandleProgressUpdate;
            _progressRuntimeSO.OnProgressInject += LoadProgress;
        }

        void OnDisable()
        {
            _onLevelInit.Unregister(Init);
            _missionHUD.OnRewardClaimed -= HandleRewardClaim;
            if(_currentMission != null) _currentMission.OnValueChanged -= HandleProgressUpdate;
            _progressRuntimeSO.OnProgressInject -= LoadProgress;
        }

        public void Init(LevelConfigSO config)
        {
            _isFinalMission = false;
            if(_currentMission != null) _currentMission.OnValueChanged -= HandleProgressUpdate;
            _currentMission = null;
            _remainingMissions = new List<BaseMissionSO>(config.AvailableMissions);
            _currentLevelConfigSO = config;

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
                if(_currentMission != null) _currentMission.OnValueChanged -= HandleProgressUpdate;
                _currentMission = _remainingMissions[0];
                _currentMission.OnValueChanged += HandleProgressUpdate;
                _remainingMissions.RemoveAt(0);
                _missionHUD.ToggleProgressText(true);
            }

            //Sau khi ở trên đã pop, check Nếu là nhiệm vụ cuối cùng
            if(_remainingMissions.Count == 0)
            {
                _missionHUD.ToggleProgressText(false);
                _isFinalMission = true;
            }

            UpdateMissionUI();
        }

        private void UpdateMissionUI()
        {
            if (_currentMission == null) return;
            
            _missionHUD.Setup(_currentMission, () => _attentionEffect.ToggleAttention(true));
        }

        private void HandleProgressUpdate()
        {
            if (_currentMission == null) return;

            // Logic tracking nhiệm vụ cuối (nếu là dạng AllCoreStation)
            if (_isFinalMission && _currentMission.IsCompleted())
            {
                _missionHUD.UpdateProgress();
                CompleteLevel();
                return;
            }

             // Update UI cho nhiệm vụ hiện tại
            _missionHUD.UpdateProgress(() => _attentionEffect.ToggleAttention(true));
        }

        private void HandleRewardClaim()
        {
            if (_currentMission == null) return;
            
            UpdateProgress();

            if (_isFinalMission)
            {
                CompleteLevel();
            }
            else
            {
                _currentMission.ApplyReward(_rewardStartPos.position);
                ActivateNextMission();
            }   
        }

        private void CompleteLevel()
        {
            if (_currentMission != null) _currentMission.OnValueChanged -= HandleProgressUpdate;
            //Nhận thưởng cho nhiệm vụ cuối
            _currentMission.ApplyReward(_rewardStartPos.position);

            // Tự động nhận hết phần thưởng còn lại trong queue nếu có
            while (_remainingMissions.Count > 0)
            {
                _remainingMissions[0].ApplyReward(_rewardStartPos.position);
                _remainingMissions.RemoveAt(0);
            }

            _onLevelComplete.Raise(_currentLevelConfigSO);
            _missionHUD.gameObject.SetActive(false);
        }

        public void LoadProgress(ProgressSaveRuntimeSO progressRuntimeSO)
        {
            LevelProgressSave progress = progressRuntimeSO.LevelProgressSave;
            List<LevelMissionProgress> missionProgresses = progress.levelMissionProgresses;
            if (missionProgresses == null || missionProgresses.Count == 0) return;

            while (_currentMission != null && missionProgresses.Any(m => m.MissionID == _currentMission.Id && m.isCollected))
            {
                ActivateNextMission();
            }
        }

        public void UpdateProgress()
        {
            _progressRuntimeSO.LevelProgressSave.UpdateLevelMission(_currentMission.Id, true);
        }
        #endregion

        partial void Debug_ValidateData();
    }
}

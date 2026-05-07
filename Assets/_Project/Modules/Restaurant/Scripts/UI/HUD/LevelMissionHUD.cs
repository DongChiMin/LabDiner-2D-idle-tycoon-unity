using System;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using LabDiner.Shared.Extension;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class LevelMissionHUD : MonoBehaviour
    {
        public Action OnRewardClaimed;

        [Header("Common UI")]
        [SerializeField] private Image _missionIcon;

        [Header("Mission UI")]
        [SerializeField] private GameObject _missionUI;    //UI phiên bản đang thực thi nhiệm vụ
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Image _rewardMissionIcon;
        [SerializeField] private Slider _progressSlider;

        [Header("Reward UI")]
        [SerializeField] private GameObject _rewardUI;    //UI phiên bản đã hoàn thành nhiệm vụ, có thể nhận thưởng
        [SerializeField] private Button _claimRewardButton;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _rewardValueText;

        private BaseGemMissionSO _currentMission;
        private bool _isRewardable = false;

        void Awake()
        {
            _claimRewardButton.onClick.AddListener(() => OnRewardClaimed?.Invoke());
        }

        #region API
        public void Setup(BaseGemMissionSO mission, Action onCompleted = null)
        {
            _currentMission = mission;

            _isRewardable = false;
            
            //Common UI Setup
            _missionIcon.sprite = mission.MissionIcon;

            //Upgradable UI Setup
            _missionUI.SetActive(true);
            _missionText.text = mission.Title;
            _rewardMissionIcon.sprite = mission.RewardIcon;
            _progressText.text = $"{mission.GetCurrentValue():F0}/{mission.TargetValue:F0}";
            _progressSlider.value = (float)mission.GetCurrentValue() / mission.TargetValue;

            //Reward UI Setup
            _rewardUI.SetActive(false);
            _rewardIcon.sprite = mission.RewardIcon;
            _rewardValueText.text = mission.RewardValue.ToString();

            FetchCompleteStatus(onCompleted);
        }

        public void UpdateProgress(Action onCompleted = null)
        {
            if(_isRewardable) return; // Nếu đã hoàn thành nhiệm vụ, không cập nhật tiến độ nữa

            _progressText.text = $"{_currentMission.GetCurrentValue():F0}/{_currentMission.TargetValue:F0}";
            _progressSlider.value = (float)_currentMission.GetCurrentValue() / _currentMission.TargetValue;

            FetchCompleteStatus(onCompleted);
        }

        public void ToggleProgressText(bool isVisible)
        {
            _progressText.gameObject.SetActive(isVisible);
        }
        #endregion
        

        #region Private Methods
        private void FetchCompleteStatus(Action onCompleted )
        {
            if(_currentMission.IsCompleted() && !_isRewardable)
            {
                _isRewardable = true;
                onCompleted?.Invoke();
                _missionUI.SetActive(false);
                _rewardUI.SetActive(true);
            }
        }
        #endregion
    }
}

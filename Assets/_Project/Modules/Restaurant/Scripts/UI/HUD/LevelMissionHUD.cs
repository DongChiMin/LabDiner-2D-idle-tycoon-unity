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
        [SerializeField] private Image _backgroundImage;

        [Header("Mission UI")]
        [SerializeField] private GameObject _missionUI;    //UI phiên bản đang thực thi nhiệm vụ
        [SerializeField] private TextMeshProUGUI _missionText;
        [SerializeField] private TextMeshProUGUI _progressText;
        [SerializeField] private Image _rewardMissionIcon;
        [SerializeField] private Image _progressFillImage;

        [Header("Reward UI")]
        [SerializeField] private GameObject _rewardUI;    //UI phiên bản đã hoàn thành nhiệm vụ, có thể nhận thưởng
        [SerializeField] private Button _claimRewardButton;
        [SerializeField] private Image _rewardIcon;
        [SerializeField] private TextMeshProUGUI _rewardValueText;

        private bool _isRewardable = false;

        void Awake()
        {
            _claimRewardButton.onClick.AddListener(() => OnRewardClaimed?.Invoke());
        }

        public void Setup(BaseGemMissionSO mission, int currentProgress, Action onCompleted = null)
        {
            _isRewardable = false;
            
            //Common UI Setup
            _backgroundImage.color = Color.white;
            _missionIcon.sprite = mission.MissionIcon;

            //Upgradable UI Setup
            _missionUI.SetActive(true);
            _missionText.text = mission.Title;
            _rewardMissionIcon.sprite = mission.RewardIcon;
            _progressText.text = $"{currentProgress}/{mission.MissionValue}";
            _progressFillImage.fillAmount = (float)currentProgress / mission.MissionValue;

            //Reward UI Setup
            _rewardUI.SetActive(false);
            _rewardIcon.sprite = mission.RewardIcon;
            _rewardValueText.text = mission.RewardValue.ToString();

            if(currentProgress >= mission.MissionValue)
            {
                _isRewardable = true;
                onCompleted?.Invoke();
                HandleRewardable();
            }
        }

        public void UpdateProgress(int currentValue, int targetValue, Action onRewardable = null)
        {
            if(_isRewardable) return; // Nếu đã hoàn thành nhiệm vụ, không cập nhật tiến độ nữa

            _progressText.text = $"{currentValue}/{targetValue}";
            float fillAmount = (float)currentValue / targetValue;
            _progressFillImage.fillAmount = fillAmount;

            if (currentValue >= targetValue)
            {
                _isRewardable = true;
                onRewardable?.Invoke();
                HandleRewardable();
            }
        }

        private void HandleRewardable()
        {
            _missionUI.SetActive(false);
            _rewardUI.SetActive(true);

            _backgroundImage.color = Color.green;
        }
    }
}

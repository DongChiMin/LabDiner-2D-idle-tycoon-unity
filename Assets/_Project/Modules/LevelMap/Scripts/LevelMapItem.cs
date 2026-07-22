using LabDiner.Restaurant.SO;
using LabDiner.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.LevelMap.UI
{
    public class LevelMapItem : MonoBehaviour
    {
        public LevelConfigSO LevelConfig => _levelConfig;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _levelNameText;
        [SerializeField] private TextMeshProUGUI _levelDescriptionText;
        [SerializeField] private Image _levelIconImage;

        [Header("Level Status")]
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _buttonText;

        private LevelConfigSO _levelConfig;

        public void Setup(LevelConfigSO levelConfig)
        {
            _levelConfig = levelConfig;
            _levelNameText.text = levelConfig.LevelName;
            _levelDescriptionText.text = levelConfig.LevelDescription;
            _levelIconImage.sprite = levelConfig.LevelIcon;
        }

        public void SetInProgressUI()
        {
            _buttonText.text = "Playing";
        }

        public void SetCompletedUI(PlayedLevel playedLevelData)
        {
            _buttonText.text = "Completed";
        }

        public void SetLockedUI()
        {
            _playButton.interactable = false;
            _buttonText.text = "Locked";
        }

    }
}

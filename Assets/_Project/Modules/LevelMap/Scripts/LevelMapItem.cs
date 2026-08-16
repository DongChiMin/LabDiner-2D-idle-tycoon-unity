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
        [Header("Data")]
        [SerializeField] private Sprite placeholderIcon;
        [SerializeField] private Sprite PlayingIcon;
        [SerializeField] private Sprite CompletedIcon;
        [SerializeField] private Sprite LockedIcon;

        [Header("UI References")]
        [SerializeField] private TextMeshProUGUI _levelNameText;
        [SerializeField] private TextMeshProUGUI _levelDescriptionText;
        [SerializeField] private Image _levelIconImage;
        [SerializeField] private Image _levelStatusImage;

        [Header("Level Status")]
        [SerializeField] private Button _playButton;
        [SerializeField] private TextMeshProUGUI _buttonText;

        private LevelConfigSO _levelConfig;

        public void Setup(LevelConfigSO levelConfig)
        {
            _levelConfig = levelConfig;
            _levelNameText.text = levelConfig.LevelName;
            _levelDescriptionText.text = levelConfig.LevelDescription;
            _levelIconImage.sprite = levelConfig.LevelIcon ?? placeholderIcon;
        }

        public void SetInProgressUI()
        {
            // _buttonText.text = "Playing";
            _levelStatusImage.sprite = PlayingIcon;
        }

        public void SetCompletedUI(PlayedLevel playedLevelData)
        {
            // _buttonText.text = "Completed";
            _levelStatusImage.sprite = CompletedIcon;
        }

        public void SetLockedUI()
        {
            // _playButton.interactable = false;
            // _buttonText.text = "Locked";
            _levelStatusImage.sprite = LockedIcon;
        }

    }
}

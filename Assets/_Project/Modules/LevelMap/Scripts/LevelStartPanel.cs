using LabDiner.Restaurant.SO;
using LabDiner.Shared.UI;
using NUnit.Framework.Internal;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.LevelMap.UI
{
    public class LevelStartPanel : BasePanel
    {
        public Button StartButton => _startButton;

        [Header("Item References")]
        [SerializeField] private Image _levelIcon;
        [SerializeField] private TextMeshProUGUI _txtLevelName;
        [SerializeField] private TextMeshProUGUI _txtLevelDescription;
        [SerializeField] private Button _startButton;

        public void Setup(LevelConfigSO levelConfigSO)
        {
            _levelIcon.sprite = levelConfigSO.LevelIcon;
            _txtLevelName.text = "Level " + levelConfigSO.LevelIndex + ": " + levelConfigSO.LevelName;
            _txtLevelDescription.text = levelConfigSO.LevelDescription;
        }
    }
}

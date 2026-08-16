using LabDiner.Restaurant.SO;
using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.LevelMap.UI
{
    public class LevelCompletePanel : BasePanel
    {
        public Button ContinueButton => _continueButton;
        public RotationEffect RotationEffect => _rotationEffect;

        [Header("UI References")]
        [SerializeField] private Button _continueButton;
        [SerializeField] private Image _levelImage;
        [SerializeField] private TextMeshProUGUI _levelNameText;
        [SerializeField] private RotationEffect _rotationEffect;

        public void Init(LevelConfigSO levelConfigSO)
        {
            _levelImage.sprite = levelConfigSO.LevelIcon;
            _levelNameText.text = levelConfigSO.LevelName + " Complete!";
            // Khởi tạo dữ liệu hiển thị dựa trên levelConfigSO nếu cần
        }
    }
}

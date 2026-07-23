using System;
using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;
namespace LabDiner.GameSetting.UI
{
    public class ToggleButton : MonoBehaviour
    {
        public Action<bool> OnValueChanged;
        public Button Button => _toggleButton;

        void OnEnable()
        {
            _toggleButton.onClick.AddListener(Toggle);
        }

        void OnDisable()
        {
            _toggleButton.onClick.RemoveListener(Toggle);
        }

        private bool _isOn;
        [SerializeField] private Button _toggleButton;
        [SerializeField] private Image _toggleIcon;

        public void FetchData(bool isOn)
        {
            _isOn = isOn;
            UpdateVisualState();
        }

        private void Toggle()
        {
            _isOn = !_isOn;
            UpdateVisualState();
            OnValueChanged?.Invoke(_isOn);
        }

        private void UpdateVisualState()
        {
            // Cập nhật trạng thái hình ảnh hoặc màu sắc của nút dựa trên _isOn
            // Ví dụ: thay đổi màu nền hoặc biểu tượng
            if (_isOn)
            {
                _toggleButton.image.color = Color.white;
                _toggleIcon.color = Color.white;
            }
            else
            {
                _toggleButton.image.color = Color.gray;
                _toggleIcon.color = Color.gray;
            }
        }
    }
}

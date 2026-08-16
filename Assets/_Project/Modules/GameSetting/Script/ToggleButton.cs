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
        private Color _buttonColorOn;
        private Color _buttonColorOff;
        private Color _iconColorOn;
        private Color _iconColorOff;

        private bool _isInitColor = false;

        void OnEnable()
        {
            _toggleButton.onClick.AddListener(Toggle);
        }

        void OnDisable()
        {
            _toggleButton.onClick.RemoveListener(Toggle);
        }

        void InitColor()
        {
            _buttonColorOn = _toggleButton.image.color;
            _buttonColorOff = Color.gray;
            _iconColorOn = _toggleIcon.color;
            _iconColorOff = _toggleIcon.color * 0.5f; // Giảm độ sáng của màu icon khi tắt
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
            if (!_isInitColor)
            {
                InitColor();
                _isInitColor = true;
            }
            // Cập nhật trạng thái hình ảnh hoặc màu sắc của nút dựa trên _isOn
            // Ví dụ: thay đổi màu nền hoặc biểu tượng
            if (_isOn)
            {
                _toggleButton.image.color = _buttonColorOn;
                _toggleIcon.color = _iconColorOn;
            }
            else
            {
                _toggleButton.image.color = _buttonColorOff;
                _toggleIcon.color = _iconColorOff;
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI; // Cần cái này để dùng Selectable
using UnityEngine.EventSystems;
using System;

namespace LabDiner.UI
{
    // Kế thừa Selectable để có sẵn thuộc tính .interactable và hệ thống Transition màu sắc
    public class HoldableButton : Selectable, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {
        [Header("Turbo Settings")]
        [SerializeField] private float _initialDelay = 0.5f;    // Thời gian chờ trước khi bắt đầu tick liên tục
        [SerializeField] private float _startInterval = 0.2f;   // Khoảng thời gian giữa các tick đầu tiên, sẽ giảm dần theo thời gian giữ
        [SerializeField] private float _minInterval = 0.05f;    // Khoảng thời gian nhỏ nhất giữa các tick, để tránh tick quá nhanh khi giữ lâu
        [SerializeField] private float _acceleration = 0.1f;    // Tốc độ giảm khoảng thời gian giữa các tick (vd: 0.1 nghĩa là mỗi tick sẽ nhanh hơn 10% so với tick trước)

        public event Action OnTurboTick;

        private bool _isPressed;
        private float _nextTickTime;
        private float _currentInterval;

        public override void OnPointerDown(PointerEventData eventData)
        {
            // Kiểm tra thuộc tính interactable có sẵn của Selectable
            if (!interactable) return; 

            base.OnPointerDown(eventData); // Giữ lại logic mặc định của Unity (như đổi màu nút)

            _isPressed = true;
            _currentInterval = _startInterval;
            DoTick();
            _nextTickTime = Time.time + _initialDelay;
        }

        public override void OnPointerUp(PointerEventData eventData)
        {
            base.OnPointerUp(eventData);
            StopTurbo();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            StopTurbo();
        }

        private void Update()
        {
            // Nếu bị tắt interactable giữa chừng (vd: vừa hết tiền), phải dừng ngay
            if (!_isPressed || !interactable) 
            {
                if (_isPressed) StopTurbo();
                return;
            }

            if (Time.time >= _nextTickTime)
            {
                DoTick();
                _nextTickTime = Time.time + _currentInterval;
                _currentInterval = Mathf.Max(_minInterval, _currentInterval * (1f - _acceleration));
            }
        }

        private void DoTick() => OnTurboTick?.Invoke();
        private void StopTurbo() => _isPressed = false;
    }
}
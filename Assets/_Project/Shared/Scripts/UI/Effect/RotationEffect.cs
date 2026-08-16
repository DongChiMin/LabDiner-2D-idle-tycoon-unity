using System;
using System.Collections;
using UnityEngine;

namespace LabDiner.Shared.UI
{
    public class RotationEffect : BaseUIEffect
    {
        [Header("Rotation Settings")]
        [SerializeField] private Vector3 _rotationAxis = Vector3.forward;
        [SerializeField] private float _rotationSpeed = 360f; // Độ mỗi giây (degrees/second)
        [SerializeField] private bool _useUnscaledTime = true; // Chạy được cả khi game Pause (nếu cần)

        private Coroutine _currentRoutine;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        public override void Show(Action onComplete = null)
        {
            // Dừng hiệu ứng cũ nếu đang chạy dở
            StopCurrentRoutine();

            // Bắt đầu hiệu ứng xoay
            _currentRoutine = StartCoroutine(RotateRoutine(true, onComplete));
        }

        public override void Hide(Action onComplete = null)
        {
            // Dừng hiệu ứng cũ nếu đang chạy dở
            StopCurrentRoutine();

            // Nếu duration <= 0, lúc Hide có thể bạn muốn dừng hẳn, 
            // hoặc nếu muốn có hiệu ứng ẩn thì viết thêm ở đây. 
            // Hiện tại ta gọi ngay onComplete khi Hide được kích hoạt.
            onComplete?.Invoke();
        }

        private IEnumerator RotateRoutine(bool isShowing, Action onComplete)
        {
            float elapsed = 0f;
            // Nếu duration <= 0, điều kiện kết thúc theo thời gian sẽ bị bỏ qua (xoay mãi mãi)
            bool isInfinite = _duration <= 0f;

            while (isInfinite || elapsed < _duration)
            {
                float deltaTime = _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                
                // Xoay RectTransform
                Vector3 currentRotation = _rectTransform.localEulerAngles;
                currentRotation += _rotationAxis * (_rotationSpeed * deltaTime);
                _rectTransform.localEulerAngles = currentRotation;

                if (!isInfinite)
                {
                    elapsed += deltaTime;
                }

                yield return null;
            }

            _currentRoutine = null;
            onComplete?.Invoke();
        }

        private void StopCurrentRoutine()
        {
            if (_currentRoutine != null)
            {
                StopCoroutine(_currentRoutine);
                _currentRoutine = null;
            }
        }
    }
}
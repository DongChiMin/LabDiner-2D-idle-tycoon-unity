using System;
using DG.Tweening;
using LabDiner.Shared.Input;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class CoreStationButton : MonoBehaviour, IInteractable
    {
        public Action OnClick;
        [SerializeField] private Transform _visualTransform;

        [Header("Bounce Settings")]
        [SerializeField] private float _punchScale = 1.15f;
        [SerializeField] private float _duration = 0.15f;    // Thời gian chuyển động nhanh cho nhạy

        private Vector3 _originalScale;
        private Tween _currentTween;

        void Awake()
        {
            // Lưu lại kích thước gốc của nút
            _originalScale = _visualTransform.localScale;
        }

        void OnDisable()
        {
            // Kill tween và trả lại scale gốc khi bị ẩn đi
            _currentTween?.Kill();
            _visualTransform.localScale = _originalScale;
        }

        public void OnInteract()
        {
            OnClick?.Invoke();

            ScaleUp();
        }

        private void ScaleUp()
        {
            _currentTween?.Kill();

            // Hiệu ứng đà: Phóng to lên 1.15 lần trước để tạo lực "bắn"
            _currentTween = _visualTransform.DOScale(_originalScale * _punchScale, _duration)
                .SetEase(Ease.OutQuad);

            CancelInvoke(nameof(ResetScale));
            Invoke(nameof(ResetScale), _duration);
        }

        public void ResetScale()
        {
            _currentTween?.Kill();

            // Nảy về kích thước gốc (hoặc có thể hơi co nhẹ _pressedScale trước khi về gốc tùy ý)
            _currentTween = _visualTransform.DOScale(_originalScale, _duration * 1.5f)
                .SetEase(Ease.OutBack); // Dùng OutBack để tạo độ nảy đàn hồi khi về đích
        }

        public bool CanInteract()
        {
            return true;
        }
    }
}

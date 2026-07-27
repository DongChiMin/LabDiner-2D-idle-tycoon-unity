using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace LabDiner.Shared
{
    public class ClickBounceEffect : MonoBehaviour
    {
        [SerializeField] private Transform _visualTransform;

        [Header("Bounce Settings")]
        [SerializeField] private float _pressedScale = 0.9f; // Tỷ lệ thu nhỏ lại khi đè xuống (ví dụ 90%)
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

        // Gọi ngay khi người dùng nhấn chuột hoặc chạm vào nút
        public void OnMouseDown()
        {
            Debug.Log("OnMouseDown triggered");

            _currentTween?.Kill();

            // Thu nhỏ nút xuống ngay lập tức khi vừa bấm
            _currentTween = _visualTransform.DOScale(_originalScale * _pressedScale, _duration)
                .SetEase(Ease.OutQuad)
                .SetUpdate(true);
        }

        // Gọi khi người dùng nhả tay ra
        public void OnMouseUp()
        {
            _currentTween?.Kill();

            // Phình to nảy nhẹ vượt kích thước gốc một chút rồi hồi về cũ (tạo độ nảy lò xo)
            Sequence sequence = DOTween.Sequence();
            
            sequence.Append(_visualTransform.DOScale(_originalScale, _duration).SetEase(Ease.OutBack));
            
            sequence.SetUpdate(true);
            _currentTween = sequence;
        }
    }
}

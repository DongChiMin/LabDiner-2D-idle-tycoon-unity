using UnityEngine;
using DG.Tweening;
using System;

namespace LabDiner.Shared.UI
{
    public class VerticalSwingEffect : BaseUIEffect
    {
        [Header("Swing Settings")]
        [SerializeField] private float _swingDistance = 30f; // Khoảng cách bay lên/xuống (pixels)
        [SerializeField] private Ease _swingEase = Ease.InOutQuad; // Nên dùng InOutQuad cho mượt

        private Vector3 _startLocalScale;
        private Vector3 _startLocalPosition;
        private Tweener _swingTween;
        private bool _isHiding = false;

        void Awake()
        {
            _startLocalScale = transform.localScale;
        }

        public override void Show(Action onComplete = null)
        {
            // Reset trạng thái ẩn và xóa các tween cũ để tránh xung đột
            _isHiding = false;
            transform.DOKill();
            
            // Đưa mũi tên về vị trí gốc ban đầu
            _startLocalPosition = transform.localPosition;

            // Kích hoạt hiệu ứng di chuyển lên xuống vô hạn (-1 là lặp vô hạn, LoopType.Yoyo là đi tới đi lui)
            _swingTween = transform.DOLocalMoveY(_startLocalPosition.y - _swingDistance, _duration)
                                   .SetEase(_swingEase)
                                   .SetLoops(-1, LoopType.Yoyo)
                                   .SetUpdate(true)
                                   .SetLink(gameObject);

            // Vì đây là hiệu ứng lặp vô hạn, ta có thể invoke onComplete ngay khi nó bắt đầu chạy
            onComplete?.Invoke();
        }

        public override void Hide(Action onComplete = null)
        {
            if (_isHiding) return;
            _isHiding = true;

            // 1. Dừng ngay vòng lặp swing lại bằng cách kill riêng tween của nó
            if (_swingTween != null && _swingTween.IsActive())
            {
                _swingTween.Kill();
            }

            // 2. Chạy hiệu ứng thu nhỏ dần về 0 (hoặc bay nhẹ về vị trí gốc rồi biến mất)
            transform.DOScale(Vector3.zero, _duration)
                     .SetEase(Ease.InBack)
                     .SetUpdate(true)
                     .OnComplete(() =>
                     {
                         _isHiding = false;
                         // Trả lại scale mặc định để lần sau Show lên không bị mất tích
                         transform.localScale = _startLocalScale;
                         transform.localPosition = _startLocalPosition;
                         
                         onComplete?.Invoke();
                     }).SetLink(gameObject);
        }
    }
}
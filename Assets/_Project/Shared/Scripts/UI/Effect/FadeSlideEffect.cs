using UnityEngine;
using DG.Tweening;
using System;

namespace LabDiner.Shared.UI
{
    /// <summary>
    /// Hiệu ứng Fade + Slide cho UI Popup. Có thể dùng cho cả Panel lẫn Button.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeSlideEffect : BaseUIEffect
    {
        public enum SlideDirection { Left, Right, Top, Bottom }

        [Header("Settings")]
        [SerializeField] private float _slideDistance = 50f;
        [SerializeField] private SlideDirection _direction = SlideDirection.Bottom;
        
        [SerializeField] private Ease _showEase = Ease.OutCubic;
        [SerializeField] private Ease _hideEase = Ease.InCubic;

        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector2 _originalPosition;
        private bool _isInitialized;

        private void Awake() => EnsureInitialized();

        private void EnsureInitialized()
        {
            if (_isInitialized) return;
            _canvasGroup = GetComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _originalPosition = _rectTransform.anchoredPosition;
            _isInitialized = true;
        }

        public override void Show(Action onComplete = null)
        {
            EnsureInitialized();
            DOTween.Kill(_canvasGroup);
            DOTween.Kill(_rectTransform);

            _canvasGroup.alpha = 0f;
            _rectTransform.anchoredPosition = GetOffsetPosition();

            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, _duration).SetTarget(_canvasGroup).SetUpdate(true);
            DOTween.To(() => _rectTransform.anchoredPosition, x => _rectTransform.anchoredPosition = x, _originalPosition, _duration).SetTarget(_rectTransform).SetEase(_showEase).SetUpdate(true);
        }

        public override void Hide(Action onComplete = null)
        {
            DOTween.Kill(_canvasGroup);
            DOTween.Kill(_rectTransform);

            Vector2 targetPos = GetOffsetPosition();

            DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 0f, _duration).SetTarget(_canvasGroup).SetUpdate(true);
            DOTween.To(() => _rectTransform.anchoredPosition, x => _rectTransform.anchoredPosition = x, targetPos, _duration)
                .SetTarget(_rectTransform)
                .SetEase(_hideEase)
                .SetUpdate(true)
                .OnComplete(() => onComplete?.Invoke());
        }

        private Vector2 GetOffsetPosition()
        {
            Vector2 offset = _direction switch
            {
                SlideDirection.Left   => new Vector2(-_slideDistance, 0),
                SlideDirection.Right  => new Vector2(_slideDistance, 0),
                SlideDirection.Top    => new Vector2(0, _slideDistance),
                SlideDirection.Bottom => new Vector2(0, -_slideDistance),
                _ => Vector2.zero
            };
            return _originalPosition + offset;
        }
    }
}
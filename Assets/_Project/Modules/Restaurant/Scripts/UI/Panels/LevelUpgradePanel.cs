using System;
using System.Collections.Generic;
using DG.Tweening;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class LevelUpgradePanel : BasePanel
    {
        public Button CloseButton => _closeButton;
        
        [SerializeField] private Button _closeButton;
        [SerializeField] private FadeSlideEffect _contentEffect;
        [SerializeField] private ClickOutsideEffect _clickOutsideEffect;
        [SerializeField] private CanvasGroup _backgroundCanvas;

        void OnEnable()
        {
            _clickOutsideEffect.OnClickOutside += HandleClickOutside;
        }

        void OnDisable()
        {
            _clickOutsideEffect.OnClickOutside -= HandleClickOutside;
        }

        public override void Show()
        {
            _backgroundCanvas.gameObject.SetActive(true);
            _contentEffect.gameObject.SetActive(true);

            _backgroundCanvas.alpha = 0f;
            DOTween.To(() => _backgroundCanvas.alpha, x => _backgroundCanvas.alpha = x, 1f, _contentEffect.Duration).SetUpdate(true);

            _contentEffect.Show();
        }

        public override void Hide(Action onComplete = null)
        {
            DOTween.To(() => _backgroundCanvas.alpha, x => _backgroundCanvas.alpha = x, 0f, _contentEffect.Duration).SetUpdate(true);

            // Kêu Content ẩn đi, khi ẩn xong thì tắt GameObject cha
            _contentEffect.Hide(() =>
            {
                onComplete?.Invoke();
                _backgroundCanvas.gameObject.SetActive(false); // Tắt background
                _contentEffect.gameObject.SetActive(false); // Reset vị trí content
            });
        }

        private void HandleClickOutside()
        {
            Hide();
        }
    }
}

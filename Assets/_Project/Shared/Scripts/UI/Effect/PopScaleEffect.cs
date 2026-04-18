using UnityEngine;
using DG.Tweening;
using System;

namespace LabDiner.Shared.UI
{
    public class PopScaleEffect : BaseUIEffect
    {
        [Header("Settings")]
        [SerializeField] private Vector3 _targetScale = Vector3.one;
        
        [SerializeField] private Ease _showEase = Ease.OutBack; 
        [SerializeField] private Ease _hideEase = Ease.InBack;

        public override void Show(Action onComplete = null)
        {
            transform.DOKill();

            transform.localScale = Vector3.zero;
            
            transform.DOScale(_targetScale, _duration)
                     .SetEase(_showEase)
                     .SetUpdate(true)
                     .OnComplete(() =>
                    {
                        onComplete?.Invoke();
                    });
        }

        public override void Hide(Action onComplete = null)
        {
            transform.DOScale(Vector3.zero, _duration)
                     .SetEase(_hideEase)
                     .SetUpdate(true)
                     .OnComplete(() =>
                     {
                         onComplete?.Invoke();
                     });
        }
    }
}
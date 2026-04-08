using UnityEngine;
using DG.Tweening;
using System;

namespace LabDiner.Shared.UI
{
    public class PopScaleEffect : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _duration = 0.3f;
        [SerializeField] private Vector3 _targetScale = Vector3.one;
        
        // OutBack tạo hiệu ứng nảy nhẹ (彈) cực đẹp khi phóng to
        [SerializeField] private Ease _showEase = Ease.OutBack; 
        [SerializeField] private Ease _hideEase = Ease.InBack;

        private void OnEnable()
        {
            // Vừa bật lên là thu nhỏ về 0 ngay lập tức
            transform.localScale = Vector3.zero;
            
            // Phóng to lên
            transform.DOScale(_targetScale, _duration)
                     .SetEase(_showEase)
                     .SetUpdate(true); // Chạy được cả khi game đang Pause
        }

        // Thay vì gọi SetActive(false) từ bên ngoài, hãy gọi hàm này!
        public void Hide(Action onComplete = null)
        {
            transform.DOScale(Vector3.zero, _duration)
                     .SetEase(_hideEase)
                     .SetUpdate(true)
                     .OnComplete(() =>
                     {
                         onComplete?.Invoke();
                         gameObject.SetActive(false); // Hiệu ứng xong mới tắt Object
                     });
        }
    }
}
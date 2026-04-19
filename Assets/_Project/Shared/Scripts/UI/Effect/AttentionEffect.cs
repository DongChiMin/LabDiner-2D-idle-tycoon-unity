using UnityEngine;
using DG.Tweening;

namespace LabDiner.Shared.UI
{
    public class AttentionEffect : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _scaleStrength = 1.2f;
        [SerializeField] private float _rotateAngle = 10f;
        [SerializeField] private float _animationDuration = 0.6f;
        [SerializeField] private float _pauseDuration = 2.0f;

        private Sequence _mainSequence;

        private void OnEnable()
        {
            StartHighlightAnimation();
        }

        private void OnDisable()
        {
            StopAnimation();
        }

        private void StartHighlightAnimation()
        {
            // Reset trạng thái
            StopAnimation();
            
            // Đảm bảo không ghi đè scale nếu nó đang bị ẩn (scale = 0)
            // Nếu bạn gọi PopScaleEffect.Show() nó sẽ tự chạy lại OnEnable
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;

            _mainSequence = DOTween.Sequence();
            _mainSequence.SetId("Attention_" + gameObject.GetInstanceID());

            _mainSequence.Append(transform.DOScale(_scaleStrength, _animationDuration * 0.3f).SetEase(Ease.OutBack));
            
            _mainSequence.Append(transform.DOShakeRotation(_animationDuration * 0.7f, 
                new Vector3(0, 0, _rotateAngle), 10, 90, false));

            _mainSequence.Append(transform.DOScale(1f, _animationDuration * 0.3f).SetEase(Ease.InQuad));

            _mainSequence.AppendInterval(_pauseDuration);

            _mainSequence.SetLoops(-1);
            
            // Bổ sung: Đảm bảo Sequence này bị hủy nếu có một Tween Scale khác (như PopScaleEffect) tác động vào
            _mainSequence.SetLink(gameObject); 
        }

        private void StopAnimation()
        {
            // SỬA TẠI ĐÂY: Kill(true) sẽ hoàn tất trạng thái hiện tại hoặc Kill() đơn thuần để dừng đè dữ liệu
            if (_mainSequence != null)
            {
                _mainSequence.Kill();
                _mainSequence = null;
            }
        }
        
        // Thêm hàm này để PopScaleEffect có thể gọi nếu cần can thiệp thô bạo
        public void ManualStop()
        {
            StopAnimation();
        }
    }
}
using UnityEngine;
using DG.Tweening;

namespace LabDiner.Shared.UI
{
    public class ToggleAttentionEffect : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _scaleMultiplier = 1.15f; // Độ phóng đại
        [SerializeField] private float _duration = 0.4f;         // Thời gian co dãn
        [SerializeField] private float _pauseDuration = 1.2f;    // Thời gian nghỉ giữa các nhịp
        [SerializeField] private Ease _easeType = Ease.OutQuad;

        private Sequence _mainSequence;
        private Vector3 _originalScale;
        private bool _isActive = false;

        private void Awake()
        {
            // Lưu lại scale gốc để đảm bảo reset không bị lệch
            _originalScale = transform.localScale;
        }

        /// <summary>
        /// API để bật/tắt hiệu ứng từ bên ngoài
        /// </summary>
        public void ToggleAttention(bool bin)
        {
            if (_isActive == bin) return;
            _isActive = bin;

            if (_isActive) Play();
            else Stop();
        }

        private void Play()
        {
            KillCurrent();

            _mainSequence = DOTween.Sequence();

            // 1. Phóng to
            _mainSequence.Append(transform.DOScale(_originalScale * _scaleMultiplier, _duration).SetEase(_easeType));
            
            // 2. Thu nhỏ về trạng thái ban đầu
            _mainSequence.Append(transform.DOScale(_originalScale, _duration * 0.8f).SetEase(Ease.InQuad));

            // 3. NGHỈ (Interval) - Đây là điểm mấu chốt cậu cần
            _mainSequence.AppendInterval(_pauseDuration);

            // Cấu hình Sequence
            _mainSequence.SetLoops(-1); // Lặp vô hạn
            _mainSequence.SetUpdate(true); // Chạy kể cả khi Pause game
            _mainSequence.SetLink(gameObject); // Tự hủy theo GameObject
        }

        private void Stop()
        {
            KillCurrent();
            
            // Đưa về trạng thái mặc định mượt mà ngay khi tắt
            transform.DOScale(_originalScale, 0.2f).SetUpdate(true);
        }

        private void KillCurrent()
        {
            if (_mainSequence != null)
            {
                _mainSequence.Kill();
                _mainSequence = null;
            }
            // Kill các tween lẻ trên transform để tránh tranh chấp
            transform.DOKill();
        }

        private void OnDisable()
        {
            // Bảo hiểm: Dừng hoàn toàn khi UI bị đóng
            _isActive = false;
            KillCurrent();
        }
    }
}
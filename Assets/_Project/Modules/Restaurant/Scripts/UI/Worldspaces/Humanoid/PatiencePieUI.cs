using System.Collections;
using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class PatiencePieUI : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _highColor = Color.green;
        [SerializeField] private Color _mediumColor = Color.yellow;
        [SerializeField] private Color _lowColor = Color.red;
        [SerializeField] private float _flashThreshold = 0.25f;
        [SerializeField] private float _flashSpeed = 1f;

        private PopScaleEffect _popScaleEffect;

        void Awake()
        {
            _popScaleEffect = GetComponent<PopScaleEffect>();
        }

        public void UpdateVisual(float ratio)
        {
            _fillImage.fillAmount = ratio;

            // Đổi màu
            if (ratio > 0.5f)
                _fillImage.color = Color.Lerp(_mediumColor, _highColor, (ratio - 0.5f) * 2);
            else
                _fillImage.color = Color.Lerp(_lowColor, _mediumColor, ratio * 2);

            // Nhấp nháy
            Color c = _fillImage.color;
            if (ratio <= _flashThreshold)
            {
                c.a = 0.5f + Mathf.PingPong(Time.time * _flashSpeed, 0.5f);
            }
            else
            {
                c.a = 1f;
            }
            _fillImage.color = c;
        }

        public void SetActive(bool isActive)
        {
            if (isActive)
            {
                // Nếu nó đang bị tắt, bật lên sẽ tự kích hoạt OnEnable (Pop effect)
                if (!gameObject.activeSelf)
                {
                    gameObject.SetActive(true);
                }
                else
                {
                    // Nếu nó ĐANG bật (trường hợp khách tiếp theo thay thế ngay lập tức)
                    // Ta cần ép nó chạy lại hiệu ứng Pop bằng cách tắt đi bật lại ngay trong 1 nốt nhạc
                    // hoặc gọi thẳng logic từ PopScaleEffect nếu có thể.
                    gameObject.SetActive(false);
                    gameObject.SetActive(true);
                }
            }
            else
            {
                if (_popScaleEffect != null)
                {
                    _popScaleEffect.Hide();
                }
                else
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
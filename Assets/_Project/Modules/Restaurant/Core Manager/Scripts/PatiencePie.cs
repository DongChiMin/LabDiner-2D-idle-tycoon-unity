using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class PatiencePie : MonoBehaviour
    {
        [SerializeField] private Image _fillImage;
        [SerializeField] private Color _highColor = Color.green;
        [SerializeField] private Color _mediumColor = Color.yellow;
        [SerializeField] private Color _lowColor = Color.red;
        [SerializeField] private float _flashThreshold = 0.25f;
        [SerializeField] private float _flashSpeed = 1f;

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
            gameObject.SetActive(isActive);
        }
    }
}
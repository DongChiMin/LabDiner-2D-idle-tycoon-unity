using UnityEngine;
using UnityEngine.Events;
using LabDiner.Shared.Input;
using System;

namespace LabDiner.Shared.UI
{
    public class ClickOutsideEffect : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        
        [Header("Events")]
        public Action OnClickOutside; // Hoạt động y hệt Button.onClick

        private RectTransform _rectTransform;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
        }

        private void OnEnable()
        {
            InputReader.OnTap += HandleGlobalClick;
        }

        private void OnDisable()
        {
            InputReader.OnTap -= HandleGlobalClick;
        }

        private void HandleGlobalClick(Vector2 mousePos)
        {
            if (!gameObject.activeInHierarchy) return;

            Camera eventCamera = (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : Camera.main;

            if (!RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, mousePos, eventCamera))
            {
                // Cảm biến phát hiện click ra ngoài -> Hét lên cho ai quan tâm thì nghe!
                OnClickOutside?.Invoke();
            }
        }
    }
}
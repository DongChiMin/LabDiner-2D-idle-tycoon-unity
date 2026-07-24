using UnityEngine;
using LabDiner.Shared.Input;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace LabDiner.Shared.UI
{
    public class ClickOutsideEffect : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private LayerMask _blockRaycastLayer;

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

            //Nếu click vào gameObject có layer trong _blockRaycast thì bỏ qua
            GameObject firstUI = GetFirstUIUnderPointer(mousePos);
            if(firstUI != null && IsIgnoredLayer(firstUI.layer)) return;

            //Nếu click ra ngoài RectTransform của object này
            if (!RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, mousePos, eventCamera))
            {
                // Cảm biến phát hiện click ra ngoài -> Hét lên cho ai quan tâm thì nghe!
                OnClickOutside?.Invoke();
            }
        }

        public GameObject GetFirstUIUnderPointer(Vector2 screenPosition)
        {
            if (EventSystem.current == null) return null;

            // 1. Khai báo thông tin Pointer tại vị trí truyền vào (vd: Input.mousePosition)
            PointerEventData eventData = new PointerEventData(EventSystem.current)
            {
                position = screenPosition
            };

            // 2. Bắn Raycast tìm tất cả UI bên dưới
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // 3. Kết quả đầu tiên (index 0) chính là UI nằm cao nhất/đầu tiên trúng tia
            if (results.Count > 0)
            {
                return results[0].gameObject;
            }

            return null; // Không trúng UI nào
        }

        private bool IsIgnoredLayer(int layer)
        {
            return (_blockRaycastLayer.value & (1 << layer)) != 0;
        }       

        #if UNITY_EDITOR
        // Tự động gán LayerMask khi Add Component hoặc Reset Component trong Inspector
        private void Reset()
        {
            AutoAssignBlockRaycastLayer();
        }

        private void AutoAssignBlockRaycastLayer()
        {
            int layerIndex = LayerMask.NameToLayer("BlockRaycast");

            if (layerIndex != -1)
            {
                // Chuyển đổi từ số Layer Index (0-31) sang bitmask của LayerMask
                _blockRaycastLayer = 1 << layerIndex;
            }
            else
            {
                Debug.LogWarning("[ClickOutsideEffect] Chưa tạo Layer tên 'BlockRaycast' trong Tags & Layers!", this);
            }
        }
        #endif
    }
}
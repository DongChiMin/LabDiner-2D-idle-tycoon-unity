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
        [SerializeField] private List<GameObject> _ignoredObjects; // Danh sách các object sẽ được bỏ qua khi click ra ngoài
        
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

            //Nếu click ra ngoài RectTransform của object này && click không vào bất kì UI nào
            if (!RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, mousePos, eventCamera) && !IsPointerOverUI(mousePos))
            {
                // Cảm biến phát hiện click ra ngoài -> Hét lên cho ai quan tâm thì nghe!
                OnClickOutside?.Invoke();
            }
        }

        // Hàm phụ trợ để kiểm tra xem tọa độ mousePos hiện tại có đè lên UI nào không
        private bool IsPointerOverUI(Vector2 mousePos)
        {
            if (EventSystem.current == null) return false;

            PointerEventData eventData = new PointerEventData(EventSystem.current);
            eventData.position = mousePos;

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            // Xóa tất cả các UI element trùng với danh sách _ignoredObjects
            // Hoặc nằm trong cây phân cấp con của các _ignoredObjects đó
            results.RemoveAll(result => IsIgnored(result.gameObject));

            return results.Count > 0;
        }

        private bool IsIgnored(GameObject hitObject)
        {
            if (_ignoredObjects == null) return false;

            foreach (var ignoredObj in _ignoredObjects)
            {
                if (ignoredObj == null) continue;

                // Trả về true nếu click vào chính object đó HOẶC con/cháu của object đó
                if (hitObject == ignoredObj || hitObject.transform.IsChildOf(ignoredObj.transform))
                {
                    return true;
                }
            }
            return false;
        }       
    }
}
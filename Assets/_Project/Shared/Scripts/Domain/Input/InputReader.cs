using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using System;

namespace LabDiner.Shared.Input
{
    public class InputReader : MonoBehaviour
    {
        public static Action<Vector2> OnGlobalClick;
        public static Action<Vector2> OnPointerDown;
        public static Action<Vector2> OnPointerUp;
        public static Action<Vector2> OnTap;
        public static Action<Vector2> OnDrag;

        [Header("Settings")]
        [SerializeField] private LayerMask _interactableLayer; // Gán Layer "Interactable" trong Inspector
        [SerializeField] private float _maxRayDistance = 100f;
        [SerializeField] private float _dragThreshold = 10f;

        // Khai báo các Action của New Input System
        private InputAction _clickAction;
        private InputAction _positionAction;

        // Internal state để phân biệt giữa Click và Drag
        private Vector2 _startPos;
        private bool _isPointerDown;

        private void Awake()
        {
            // Tự động map phím: <Pointer> bao gồm cả Chuột và Cảm ứng Mobile!
            _clickAction = new InputAction("Click", binding: "<Pointer>/press");
            _positionAction = new InputAction("Position", binding: "<Pointer>/position");

            _clickAction.started += OnPointerDownInternal;
            _clickAction.canceled += OnPointerUpInternal;
            _clickAction.canceled += OnClickPerformed;
        }

        private void OnEnable()
        {
            _clickAction.Enable();
            _positionAction.Enable();
        }

        private void OnDisable()
        {
            _clickAction.Disable();
            _positionAction.Disable();
        }

        private void Update()
        {
            if (!_isPointerDown) return;

            Vector2 current = _positionAction.ReadValue<Vector2>();
            float dist = Vector2.Distance(_startPos, current);

            if (dist > _dragThreshold)
            {
                OnDrag?.Invoke(current);
            }
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            // 1. Lấy vị trí chuột/ngón tay
            Vector2 screenPos = _positionAction.ReadValue<Vector2>();

            // 2. Lấy PointerId (DeviceId) từ thiết bị vừa thực hiện action
            int deviceId = context.control.device.deviceId;

            // 2.3 PHÁT TIN: Báo cho các UI biết có cú click vừa xảy ra
            OnGlobalClick?.Invoke(screenPos);

            // 3. Chặn nếu bấm trúng UI
            if (IsPointerOverUI(deviceId, screenPos)) return;

            // 4. Bắn Raycast vào thế giới game
            HandleWorldInteraction(screenPos);
        }

        private void HandleWorldInteraction(Vector2 screenPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            RaycastHit2D hit = Physics2D.GetRayIntersection(ray, _maxRayDistance, _interactableLayer);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    if (interactable.CanInteract())
                    {
                        interactable.OnInteract();
                    }
                }
            }
        }

        private bool IsPointerOverUI(int pointerId, Vector2 screenPos)
        {
            var eventSystem = EventSystem.current;
            if (eventSystem == null) return false;

            // Lấy Module mới từ EventSystem
            var uiModule = eventSystem.currentInputModule as InputSystemUIInputModule;
            if (uiModule == null) return false;

            // Truy vấn kết quả Raycast UI của Pointer cụ thể tại frame này
            RaycastResult lastResult = uiModule.GetLastRaycastResult(pointerId);

            // Kiểm tra xem kết quả đó có tồn tại và có thuộc về một GameObject nào không
            return lastResult.isValid;
        }
        
        private void OnPointerDownInternal(InputAction.CallbackContext ctx)
        {
            _isPointerDown = true;
            _startPos = _positionAction.ReadValue<Vector2>();

            OnPointerDown?.Invoke(_startPos);
        }

        private void OnPointerUpInternal(InputAction.CallbackContext ctx)
        {
            if (!_isPointerDown) return;
            _isPointerDown = false;

            Vector2 endPos = _positionAction.ReadValue<Vector2>();
            float dist = Vector2.Distance(_startPos, endPos);

            OnPointerUp?.Invoke(endPos);

            if (dist <= _dragThreshold)
            {
                OnTap?.Invoke(endPos);
            }
            else
            {
                OnDrag?.Invoke(endPos);
            }
        }
    }
}
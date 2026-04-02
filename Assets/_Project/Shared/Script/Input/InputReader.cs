using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

namespace LabDiner.Shared.Input
{
    public class InputReader : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _interactableLayer; // Gán Layer "Interactable" trong Inspector
        [SerializeField] private float _maxRayDistance = 100f;

        // Khai báo các Action của New Input System
        private InputAction _clickAction;
        private InputAction _deltaAction;
        private InputAction _positionAction;

        private void Awake()
        {
            // Tự động map phím: <Pointer> bao gồm cả Chuột và Cảm ứng Mobile!
            _clickAction = new InputAction("Click", binding: "<Pointer>/press");
            _positionAction = new InputAction("Position", binding: "<Pointer>/position");

            // <Pointer>/delta: Trả về Vector2 (x, y) là độ dời của ngón tay/chuột
            _deltaAction = new InputAction("Delta", binding: "<Pointer>/delta");

            // Đăng ký sự kiện: Khi người chơi vừa buông tay/nhả chuột ra
            _clickAction.canceled += OnClickPerformed;
        }

        private void OnEnable()
        {
            _clickAction.Enable();
            _positionAction.Enable();
            _deltaAction.Enable();
        }

        private void OnDisable()
        {
            _clickAction.Disable();
            _positionAction.Disable();
            _deltaAction.Disable();
        }

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            // 1. Lấy vị trí chuột/ngón tay
            Vector2 screenPos = _positionAction.ReadValue<Vector2>();

            // 2. Lấy PointerId (DeviceId) từ thiết bị vừa thực hiện action
            int deviceId = context.control.device.deviceId;

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
    }
}
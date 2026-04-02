using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;

namespace LabDiner.Shared.Input
{
    public class InputReader : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask _interactableLayer; // Gán Layer "Interactable" trong Inspector
        [SerializeField] private float _maxRayDistance = 100f;

        // Khai báo các Action của New Input System
        private InputAction _clickAction;
        private InputAction _positionAction;

        private void Awake()
        {
            // Tự động map phím: <Pointer> bao gồm cả Chuột và Cảm ứng Mobile!
            _clickAction = new InputAction("Click", binding: "<Pointer>/press");
            _positionAction = new InputAction("Position", binding: "<Pointer>/position");

            // Đăng ký sự kiện: Khi người chơi vừa buông tay/nhả chuột ra
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

        private void OnClickPerformed(InputAction.CallbackContext context)
        {
            // 1. Lấy vị trí chuột/ngón tay
            Vector2 screenPos = _positionAction.ReadValue<Vector2>();

            // 2. Chặn nếu bấm trúng UI
            if (IsPointerOverUI()) return;

            // 3. Bắn Raycast vào thế giới game
            HandleWorldInteraction(screenPos);
        }

        private void HandleWorldInteraction(Vector2 screenPos)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance, _interactableLayer))
            {
                // Tìm Interface trên vật bị bắn trúng
                if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
                {
                    if (interactable.CanInteract())
                    {
                        interactable.OnInteract();
                    }
                }
            }
        }

        private bool IsPointerOverUI()
        {
            if (EventSystem.current == null) return false;

            // Trong New Input System, hàm này tự động check cả chuột và cảm ứng cực chuẩn
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
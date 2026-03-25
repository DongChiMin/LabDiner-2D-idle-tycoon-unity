using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LabDiner.Player
{
    public class GlobalInputReader : Singleton<GlobalInputReader>
    {
        private GameInput _input;

        // Các sự kiện thô (Raw Events)
        public event Action<Vector2> OnInputStarted;    // Truyền vào vị trí màn hình
        public event Action<Vector2> OnInputCanceled;   // Truyền vào vị trí màn hình
        public bool IsInputActive { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            _input = new GameInput();
        }

        private void OnEnable()
        {
            _input.Enable();
            _input.Player.Click.started += HandleClickStarted;
            _input.Player.Click.canceled += HandleClickCanceled;
        }

        private void OnDisable()
        {
            _input.Player.Click.started -= HandleClickStarted;
            _input.Player.Click.canceled -= HandleClickCanceled;
            _input.Disable();
        }

        private void HandleClickStarted(InputAction.CallbackContext context)
        {
            IsInputActive = true;
            OnInputStarted?.Invoke(GetPointerScreenPosition());
        }

        private void HandleClickCanceled(InputAction.CallbackContext context)
        {
            IsInputActive = false;
            OnInputCanceled?.Invoke(GetPointerScreenPosition());
        }

        // Helper để lấy vị trí chuột/tay chạm hiện tại
        public Vector2 GetPointerScreenPosition()
        {
            return _input.Player.Point.ReadValue<Vector2>();
        }

        // Chuyển đổi vị trí màn hình sang thế giới (Helper dùng chung)
        public Vector3 GetPointerWorldPosition()
        {
            Vector2 screenPos = GetPointerScreenPosition();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
            worldPos.z = 0;
            return worldPos;
        }
    }
}
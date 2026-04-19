using System;
using System.Collections.Generic;
using LabDiner.Shared.Input;
using UnityEngine;

namespace LabDiner.Shared
{
    public class UIManager : MonoBehaviour
    {
        public static Action<bool> OnUIStateChanged;
        public bool IsAnyPanelOpen => _historyStack.Count > 0;

        [Header("Events")]
        [SerializeField] private UIPanelEvent _onPanelRegister;
        [SerializeField] private UIPanelEvent _onPanelShow;
        [SerializeField] private UIPanelEvent _onPanelHide;

        [Header("DEBUG")]
        [SerializeField] private bool _isUIBlocked;
        [SerializeField] private List<BasePanel> _debugOpenPanels = new();
        [SerializeField] private List<BasePanel> _allPanelsInScene = new();

        private readonly Dictionary<Type, BasePanel> _panelCache = new();
        private readonly Stack<BasePanel> _historyStack = new(); 

        void OnEnable()
        {
            _onPanelRegister.Register(Register);
            _onPanelShow.Register(HandlePanelShow);
            _onPanelHide.Register(HandlePanelHide);

            InputReader.OnBackRequested += HandleBackRequest;
        }

        void OnDisable()
        {
            _onPanelRegister.Unregister(Register);
            _onPanelShow.Unregister(HandlePanelShow);
            _onPanelHide.Unregister(HandlePanelHide);

            InputReader.OnBackRequested -= HandleBackRequest;
        }

        #if UNITY_EDITOR
        void Update()
        {
            _isUIBlocked = IsAnyPanelOpen;

            _allPanelsInScene.Clear();
            foreach(var panel in _panelCache.Values)
            {
                _allPanelsInScene.Add(panel);
            }

            _debugOpenPanels.Clear();
            foreach (var panel in _historyStack)
            {
                _debugOpenPanels.Add(panel);
            }
        }
        #endif

        public void Register(BasePanel panel)
        {
            var type = panel.GetType();
            if (!_panelCache.ContainsKey(type))
            {
                _panelCache.Add(type, panel);
            }
        }

        public T GetPanel<T>() where T : BasePanel
        {
            if (_panelCache.TryGetValue(typeof(T), out var panel))
            {
                return (T)panel;
            }
            Debug.LogError($"Panel {typeof(T)} chưa có trong Scene!");
            return null;
        }

        public void Show<T>(Action<T> onSetup = null) where T : BasePanel
        {
            if (_panelCache.TryGetValue(typeof(T), out var panel))
            {
                // Nếu panel đang mở rồi thì không push thêm vào stack nữa
                if (_historyStack.Count > 0 && _historyStack.Peek() == panel) return;

                panel.Show();
                onSetup?.Invoke((T)panel);

                // Push vào lịch sử
                _historyStack.Push(panel);

                // Nếu là panel đầu tiên mở ra -> Báo khóa Camera
                if (_historyStack.Count == 1) OnUIStateChanged?.Invoke(true);
            }
            else
            {
                Debug.LogError($"Panel {typeof(T)} chưa có trong Scene!");
            }
        }

        public void CloseLast()
        {
            if (_historyStack.Count == 0) return;

            var lastPanel = _historyStack.Pop();
            lastPanel.Hide(() => {
                // Sau khi hide xong, nếu không còn UI nào -> Mở khóa Camera
                if (_historyStack.Count == 0) OnUIStateChanged?.Invoke(false);
            });
        }

        private void HandleBackRequest()
        {
            // Nếu có UI đang mở thì đóng nó
            if (IsAnyPanelOpen)
            {
                CloseLast();
            }
            else
            {
                // Nếu không có UI nào mở, có thể hiện Popup xác nhận thoát game
                Debug.Log("TODO: Hiện Popup thoát game hoặc Pause Menu");
            }
        }

        private void HandlePanelShow(BasePanel panel)
        {
            if (_historyStack.Count > 0 && _historyStack.Peek() == panel) return;

            // Đưa instance cụ thể của Panel đó vào stack
            _historyStack.Push(panel);

            if (_historyStack.Count == 1) OnUIStateChanged?.Invoke(true);
        }

        private void HandlePanelHide(BasePanel panel)
        {
            if (_historyStack.Count == 0) return;

            if (_historyStack.Peek() == panel)
            {
                _historyStack.Pop();

                if (_historyStack.Count == 0) OnUIStateChanged?.Invoke(false);
            }
            else
            {
                Debug.LogWarning($"Panel {panel.name} được hide nhưng không phải là panel trên cùng của stack. Có thể do gọi Hide trực tiếp thay vì qua UIManager.");
                // Nếu muốn, có thể thêm logic để tìm và loại bỏ panel này khỏi stack nếu nó tồn tại ở đâu đó trong đó.
            }
        }
    }
}

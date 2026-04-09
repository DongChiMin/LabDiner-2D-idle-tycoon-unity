using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared
{
    public class UIManager : Singleton<UIManager>
    {
        private readonly Dictionary<Type, BasePanel> _panelCache = new();

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
            var panel = GetPanel<T>();
            if (panel != null)
            {
                panel.gameObject.SetActive(true);
                onSetup?.Invoke(panel);
            }
        }
    }
}

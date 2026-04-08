using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared.Events
{
    public abstract class GameEvent<T> : ScriptableObject
    {
        private readonly List<ListenerData> _listeners = new List<ListenerData>();

        private struct ListenerData
        {
            public System.Action<T> Action;
            public int Priority;
        }

        public void Raise(T data)
        {
            // Vẫn giữ vòng lặp ngược "Count - 1" cực xịn của bạn
            // Vì list đã được sắp xếp tăng dần nên thằng cuối mảng (Ưu tiên CAO nhất) sẽ chạy ĐẦU TIÊN!
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                _listeners[i].Action.Invoke(data);
            }
        }

        /// <summary>
        /// Đăng ký listener với độ ưu tiên tùy chọn. Số càng LỚN thì độ ưu tiên càng CAO (được gọi trước khi Raise event).
        /// </summary>
        /// <param name="listener"></param>
        /// <param name="priority"></param>
        public void Register(System.Action<T> listener, int priority = 0)
        {
            _listeners.Add(new ListenerData { Action = listener, Priority = priority });
            
            // Sắp xếp tăng dần theo Priority (Ví dụ: [Thấp, Vừa, Cao])
            _listeners.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }

        public void Unregister(System.Action<T> listener)
        {
            for (int i = _listeners.Count - 1; i >= 0; i--)
            {
                // Tìm đúng Action để gỡ ra
                if (_listeners[i].Action == listener)
                {
                    _listeners.RemoveAt(i);
                    break;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestBehavior : MonoBehaviour
    {
        [SerializeField] private GuestContext _context;
        [SerializeField] private float _eatDuration = 3f;
        [SerializeField] private float _payDuration = 0f;

        [Header("[DEBUG]")]
        [SerializeField] private bool _isServed = false;
        [SerializeField] Order _order;
        [SerializeField] private List<TaskMapping> _taskMappings;
        [System.Serializable]
        private class TaskMapping
        {
            public CoreStation coreStation; // Key
            public int quantity; // Value
        }

        void OnEnable()
        {
            _isServed = false;
            _order = null;
        }

        public IEnumerator WaitInLine()
        {
            Debug.Log("TODO: giảm dần thanh patience của khách");
            // Ở đây có thể bật animation chờ đợi
            while (true)
            {
                yield return null; // Chờ cho đến khi được gọi tiếp tục
            }
        }

        public IEnumerator WaitForServe(DiningTable table)
        {
            if (_order == null) Debug.LogError("Order của khách này đang trống!");
            table.WaitingForServe(_order);
            while (!_isServed)
            {
                yield return null; // Chờ cho đến khi được gọi tiếp tục
            }
            yield return null;
        }

        public IEnumerator WaitForFood()
        {
            Debug.Log("Đang chờ đồ ăn...");
            // Ở đây có thể bật animation chờ đợi
            while (true)
            {
                yield return null; // Chờ cho đến khi được gọi tiếp tục
            }
        }

        public IEnumerator Eat()
        {
            Debug.Log("Đang ăn...");
            // Ở đây có thể bật animation ăn uống
            yield return new WaitForSeconds(_eatDuration);
            Debug.Log("Ăn xong rồi!");
        }

        public IEnumerator Pay()
        {
            Debug.Log("Đang trả tiền...");
            yield return new WaitForSeconds(_payDuration);
            Debug.Log("Trả tiền xong rồi!");
        }

        public void SetOrder(Order order)
        {
            _order = order;
            SyncDebugView(order);
        }

        public void SetServedStatus(bool status)
        {
            _isServed = status;
        }

        #region EDITOR_ONLY
        private void SyncDebugView(Order order)
        {
            // Chỉ chạy trong Editor để tránh tốn tài nguyên khi build game thật
#if UNITY_EDITOR
            _taskMappings = new List<TaskMapping>();
            foreach (var item in order.OrderDict)
            {
                CoreStation station = item.Key;
                int quantity = item.Value;

                _taskMappings.Add(new TaskMapping { coreStation = station, quantity = quantity });
            }
#endif
        }
        #endregion
    }
}
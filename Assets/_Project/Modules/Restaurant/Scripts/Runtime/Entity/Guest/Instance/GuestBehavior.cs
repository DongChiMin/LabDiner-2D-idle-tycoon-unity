using System.Collections;
using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.Humanoid
{
    public class GuestBehavior : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private TaskRuntimeSO _taskRuntimeSO;
        
        [Header("Events")]
        [SerializeField] private GuestEvent _onGuestWaitInLine;
        [SerializeField] private GuestEvent _onGuestHappy;
        [Header("Settings")]
        [SerializeField] private float _eatDuration = 3f;
        [SerializeField] private float _payDuration = 0f;

        [Header("[DEBUG]")]
        [SerializeField] private bool _isServed = false;
        [SerializeField] private bool _isFoodReceivedEnough = false;
        [SerializeField] private Order _order;
        [SerializeField] private List<TaskMapping> _taskMappings;

        [System.Serializable]
        private class TaskMapping
        {
            public CoreStation coreStation; // Key
            public int quantity; // Value
        }

        private GuestContext _ctx;
        private ServingTask _servingTask;

        void Awake()
        {
            _servingTask = new ServingTask(null, null);
        }

        void OnEnable()
        {
            _isServed = false;
            _isFoodReceivedEnough = false;
            _order = null;
        }

        void Start()
        {
            _ctx = GetComponent<GuestContext>();
        }

        public IEnumerator WaitInLine()
        {
            _onGuestWaitInLine.Raise(_ctx);
             while (true)
            {
                yield return null;
            }
        }

        public IEnumerator WaitForServe(DiningSeat seat)
        {
            if (_order == null) Debug.LogError("Order của khách này đang trống!");
            _servingTask.SetOrder(_order);
            _servingTask.SetLocation(seat.WorkPos);
            _taskRuntimeSO.Add(_servingTask);
            while (!_isServed)
            {
                yield return null;
            }
            //Sau khi được serve
            _ctx.OrderCanvas.gameObject.SetActive(true);
            yield return null;
        }

        public IEnumerator WaitForFood()
        {
            while (!_isFoodReceivedEnough)
            {
                yield return null; // Chờ cho đến khi được gọi tiếp tục
            }
            _ctx.OrderCanvas.gameObject.SetActive(false);
        }

        public IEnumerator Eat()
        {
            // _ctx.OrderCanvas.Setup(_eatDuration);
            // TODO: có thể điều chỉnh thời gian ăn dựa trên số lượng món ăn trong order hoặc các yếu tố khác
            yield return new WaitForSeconds(_eatDuration);
            // _ctx.OrderCanvas.gameObject.SetActive(false);
            _onGuestHappy.Raise(_ctx);
        }

        public IEnumerator Pay()
        {
            // TODO: có thể thêm logic thanh toán ở đây, ví dụ như hiển thị số tiền cần thanh toán dựa trên order, hoặc điều chỉnh thời gian thanh toán nếu muốn
            yield return new WaitForSeconds(_payDuration);
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

        public void SetFoodReceivedEnough(bool status)
        {
            _isFoodReceivedEnough = status;
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
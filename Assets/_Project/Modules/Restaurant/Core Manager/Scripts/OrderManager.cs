using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{

    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderEvent _onOrderServed;
        [SerializeField] private StaffTaskEvent _onNewCookingTask;
        private Queue<CookingTask> _cookingQueue = new Queue<CookingTask>();

        [Header("[DEBUG ONLY]")]
        [SerializeField] private List<CookingTask> _cookingQueueDebugView = new();

        void OnEnable()
        {
            _onOrderServed.Register(HandleOrderServed);
        }

        void OnDisable()
        {
            _onOrderServed.Unregister(HandleOrderServed);
        }


        //Tiếp nhận các order từ khách hàng, phân rã ra thành các cookingTask để phân phối từng task cho đầu bếp
        void HandleOrderServed(Order newOrder)
        {
            foreach (var item in newOrder._orderDict)
            {
                CoreStation station = item.Key;
                int quantity = item.Value;

                // 2. Với mỗi loại, lặp lại 'quantity' lần để tạo Task lẻ
                for (int i = 0; i < quantity; i++)
                {
                    IStaffTask singleTask = new CookingTask(newOrder, station);

                    // 3. Đẩy vào hàng đợi chung của bếp
                    _onNewCookingTask.Raise(singleTask);
                }
            }
            SyncDebugView();
        }

        public bool TryGetTask(out CookingTask task)
        {
            if (_cookingQueue.Count > 0)
            {
                task = _cookingQueue.Dequeue();
                SyncDebugView(); // Cập nhật lại sau khi lấy món ra
                return true;
            }
            task = null;
            return false;
        }

        private void SyncDebugView()
        {
            // Chỉ chạy trong Editor để tránh tốn tài nguyên khi build game thật
            #if UNITY_EDITOR
            _cookingQueueDebugView = new List<CookingTask>(_cookingQueue);
            #endif
        }
    }
}
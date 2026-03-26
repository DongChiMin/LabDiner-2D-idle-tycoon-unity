
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaiterManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private OrderEvent _onNewUnservedOrder;
        [SerializeField] private WaiterEvent _onWaiterAvailable;
        
        [Header("DEBUG")]
        [SerializeField] List<WaiterContext> waiters;
        [SerializeField] List<WaiterContext> availableWaiters;
        [SerializeField] List<Order> serveRequests;

        void Start()
        {
            availableWaiters = new List<WaiterContext>(waiters);
        }

        void OnEnable()
        {
            _onNewUnservedOrder.Register(HandleNewOrder);
            _onWaiterAvailable.Register(HandleWaiterAvailable);
        }

        void OnDisable()
        {
            _onNewUnservedOrder.Unregister(HandleNewOrder);
            _onWaiterAvailable.Unregister(HandleWaiterAvailable);
        }

        void HandleNewOrder(Order order)
        {
            if (availableWaiters.Count > 0)
            {
                AssignTaskToWaiter(availableWaiters[0], order);
                availableWaiters.RemoveAt(0);
                return;
            }
            else
            {
                serveRequests.Add(order);
                Debug.Log("Hết nhân viên rảnh để phục vụ đơn hàng mới!");
            }
        }   

        void HandleWaiterAvailable(WaiterContext waiter)
        {
            if (serveRequests.Count > 0)
            {
                Order nextOrder = serveRequests[0];
                serveRequests.RemoveAt(0);
                AssignTaskToWaiter(waiter, nextOrder);
            }
            else
            {
                availableWaiters.Add(waiter);
                Debug.Log("Không có đơn hàng nào đang chờ, nhân viên " + waiter.name + " đã sẵn sàng.");
            }
        }

        void AssignTaskToWaiter(WaiterContext waiter, Order order)
        {
            waiter.DoTask(order);
        }
    }
}
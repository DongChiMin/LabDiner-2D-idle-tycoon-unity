using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class OrderManager : MonoBehaviour
    {
        [SerializeField] private OrderEvent _onOrderServed;

        void OnEnable()
        {
            _onOrderServed.Register(HandleOrderServed);
        }

        void OnDisable()
        {
            _onOrderServed.Unregister(HandleOrderServed);
        }

        void HandleOrderServed(Order order)
        {
            Debug.Log("Đơn hàng đã được phục vụ: " + order._orderBy.name);
        }
    }
}
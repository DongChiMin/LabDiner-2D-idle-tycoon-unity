
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{

    public class DiningTable : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private OrderEvent _onNewUnservedOrder;

        [Header("DEBUG")]
        [SerializeField] private GuestContext _occupiedGuest;
        [SerializeField] private Order _order;

        public bool IsOccupied => _occupiedGuest != null;

        public void Occupy(GuestContext guest)
        {
            _occupiedGuest = guest;
        }

        public void WaitingForServe(Order order)
        {
            _order = order;
            _onNewUnservedOrder?.Raise(_order);
        }
    }
}
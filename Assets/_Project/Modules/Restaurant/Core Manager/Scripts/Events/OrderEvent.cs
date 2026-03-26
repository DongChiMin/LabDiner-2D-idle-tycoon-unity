using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewOrderEvent", menuName = "Events/Order Event")]
    public class OrderEvent : GameEvent<Order> { }
}
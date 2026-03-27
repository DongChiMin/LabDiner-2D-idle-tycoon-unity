using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnWaiterAvailable", menuName = "Events/Staff/Waiter Event")]
    public class WaiterEvent : GameEvent<WaiterContext> { }
}
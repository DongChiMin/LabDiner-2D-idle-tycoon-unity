using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnWaiterAvailable", menuName = "Events/Waiter Event")]
    public class WaiterEvent : GameEvent<WaiterContext> { }
}
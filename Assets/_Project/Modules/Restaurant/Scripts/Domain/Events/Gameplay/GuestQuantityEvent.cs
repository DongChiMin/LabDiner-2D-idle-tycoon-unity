using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnGuestQuantityChanged", menuName = "Events/Guest/Guest Quantity Event")]
    public class GuestQuantityEvent : GameEvent<int> { }
}
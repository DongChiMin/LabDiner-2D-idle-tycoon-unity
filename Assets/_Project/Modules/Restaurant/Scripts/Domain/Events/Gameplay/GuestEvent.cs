using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewGuestEvent", menuName = "Events/Guest Event")]
    public class GuestEvent : GameEvent<GuestContext> { }
}
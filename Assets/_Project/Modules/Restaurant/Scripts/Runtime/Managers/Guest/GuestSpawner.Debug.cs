#if UNITY_EDITOR
using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class GuestSpawner
    {
        [Header("[EDITOR ONLY DEBUG]")]
        [SerializeField] private List<GuestContext> _guests = new List<GuestContext>();

        partial void Debug_AddGuest(GuestContext guest)
        {
            _guests.Add(guest);
        }

        partial void Debug_RemoveGuest(GuestContext guest)
        {
            _guests.Remove(guest);
        }
    }
}
#endif
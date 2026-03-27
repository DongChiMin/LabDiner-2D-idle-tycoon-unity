using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewStaffEvent", menuName = "Events/Staff Event")]
    public class StaffEvent : GameEvent<IStaff> { }
}
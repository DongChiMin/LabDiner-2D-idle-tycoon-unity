using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewStaffTaskEvent", menuName = "Events/Staff Task Event")]
    public class StaffTaskEvent : GameEvent<IStaffTask> { }
}
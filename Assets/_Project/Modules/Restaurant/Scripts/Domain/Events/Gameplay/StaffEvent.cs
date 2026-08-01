using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Workflow;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "NewStaffEvent", menuName = "Events/GamePlay/Staff Event")]
    public class StaffEvent : GameEvent<Staff> { }
}
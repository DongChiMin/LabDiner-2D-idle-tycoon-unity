using LabDiner.Shared.Event;
using LabDiner.Restaurant.Environment;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "OnCoreStationLevelUpgraded", menuName = "Events/Table/CoreStation Event")]
    public class CoreStationEvent : GameEvent<CoreStation> { }
}
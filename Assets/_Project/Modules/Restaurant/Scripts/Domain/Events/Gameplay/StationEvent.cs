using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnStationAvailable", menuName = "Events/Table/Station Event")]
    public class StationEvent : GameEvent<Station> { }
}
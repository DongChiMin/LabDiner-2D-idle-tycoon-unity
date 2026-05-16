using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "OnStationUpgrade", menuName = "Events/Upgrades/Station Upgrade Event")]
    public class StationUpgradeEvent : GameEvent<StationUpgradeSO> { }
}
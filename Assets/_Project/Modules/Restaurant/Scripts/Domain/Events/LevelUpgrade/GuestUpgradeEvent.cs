using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "OnUpgradeGuest", menuName = "Events/Upgrades/Guest Upgrade Event")]
    public class GuestUpgradeEvent : GameEvent<GuestUpgradeSO> { }
}
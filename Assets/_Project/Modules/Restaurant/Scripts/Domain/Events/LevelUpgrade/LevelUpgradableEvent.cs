using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "OnLevelUpgradable", menuName = "Events/Upgrades/Level Upgradable Event")]
    public class LevelUpgradableEvent : GameEvent<bool> { }
}
using System.Collections.Generic;
using LabDiner.Shared.Events;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Shared
{
    [CreateAssetMenu(fileName = "OnGlobalUpgrade", menuName = "Events/Upgrades/Global Upgrade Event")]
    public class GlobalUpgradeEvent : GameEvent<GlobalUpgradeSO> { }
}
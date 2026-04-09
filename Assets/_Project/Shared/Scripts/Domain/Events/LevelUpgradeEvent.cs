using System.Collections.Generic;
using LabDiner.Shared.Events;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Shared
{
    [CreateAssetMenu(fileName = "OnLevelUpgrade", menuName = "Events/Level/Level Upgrade Event")]
    public class LevelUpgradeEvent : GameEvent<BaseUpgradeSO> { }
}
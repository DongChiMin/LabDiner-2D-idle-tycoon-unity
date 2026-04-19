using System.Collections.Generic;
using LabDiner.Shared.Events;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Shared
{
    [CreateAssetMenu(fileName = "OnDishUpgrade", menuName = "Events/Upgrades/Dish Upgrade Event")]
    public class DishUpgradeEvent : GameEvent<DishUpgradeSO> { }
}
using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnCoinAdded", menuName = "Events/Finance/Coin Event")]
    public class LevelCoinEvent : GameEvent<double> { }
}
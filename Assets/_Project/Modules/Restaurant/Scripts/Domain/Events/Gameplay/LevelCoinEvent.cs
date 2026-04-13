using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnCoinAdded", menuName = "Events/Currency/Coin Event")]
    public class LevelCoinEvent : GameEvent<double> { }
}
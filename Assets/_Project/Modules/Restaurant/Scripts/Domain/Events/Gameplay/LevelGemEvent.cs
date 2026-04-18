using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnGemAdded", menuName = "Events/Currency/Gem Event")]
    public class LevelGemEvent : GameEvent<int> { }
}
using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnChefAvailable", menuName = "Events/Chef Event")]
    public class ChefEvent : GameEvent<ChefContext> { }
}
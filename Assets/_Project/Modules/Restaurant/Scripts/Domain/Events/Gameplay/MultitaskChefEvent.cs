using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "OnMultitaskChefAvailable", menuName = "Events/Staff/Multitask Chef Event")]
    public class MultitaskChefEvent : GameEvent<MultitaskChefContext> { }
}
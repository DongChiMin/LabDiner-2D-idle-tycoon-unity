using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "NewDoubleEvent", menuName = "Events/Primitive/Double Event")]
    public class DoubleEvent : GameEvent<double> { }
}
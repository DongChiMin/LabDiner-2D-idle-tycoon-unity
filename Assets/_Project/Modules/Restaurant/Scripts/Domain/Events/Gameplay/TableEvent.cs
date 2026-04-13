using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewDiningTableEvent", menuName = "Events/Table/Dining Table Event")]
    public class TableEvent : GameEvent<DiningTable> { }
}
using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewTableEvent", menuName = "Events/Table Event")]
    public class TableEvent : GameEvent<DiningTable> { }
}
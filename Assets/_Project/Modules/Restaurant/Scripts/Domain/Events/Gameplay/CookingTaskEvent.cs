using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewCookingTaskEvent", menuName = "Events/Task/Cooking Task Event")]
    public class CookingTaskEvent : GameEvent<CookingTask> { }
}
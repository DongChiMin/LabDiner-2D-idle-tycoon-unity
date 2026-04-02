using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [CreateAssetMenu(fileName = "NewReputationEvent", menuName = "Events/Task/Reputation Event")]
    public class ReputationEvent : GameEvent<float> { }
}
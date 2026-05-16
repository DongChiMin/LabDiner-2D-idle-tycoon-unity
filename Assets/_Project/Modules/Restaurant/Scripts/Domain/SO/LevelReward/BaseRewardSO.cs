using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public abstract class BaseRewardSO : ScriptableObject
    {
        [Header("Reward Info")]
        public Sprite Icon;

        public abstract void ApplyReward(Vector3 startPos, double value);
    }
}
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "New Gem Reward", menuName = "Game/Rewards/Gem Reward")]
    public class GemRewardSO : BaseRewardSO
    {
        [Header("Reward Event")]
        public LevelGemFlyEvent OnGemFlyAdded;

        public override void ApplyReward(Vector3 startPos, double value)
        {
            // Gửi chính Asset này đi qua Event
            if (OnGemFlyAdded != null)
                OnGemFlyAdded.Raise(
                    new GemRewardData
                    {
                        startPos = startPos,
                        RewardValue = (int)value
                    });
        }
    }
}
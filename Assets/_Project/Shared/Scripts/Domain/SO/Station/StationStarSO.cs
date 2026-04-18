using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Shared.SO
{
    [System.Serializable]
    public struct RewardData
    {
        public BaseGameEvent Event;
        public double Amount;
        public Sprite Icon;
    }

    [CreateAssetMenu(fileName = "StationStar", menuName = "Game/Station/StationStar")]
    public class StationStarSO : ScriptableObject
    {
        public Sprite StarIcon; 
        public List<StationStarEffectSO> Effects;   //ví dụ cột mốc này vừa x2 profit, vừa tạo station mới
        public List<RewardData> rewards;

        public void GiveRewards()
        {
            foreach (var reward in rewards)
            {
                if (reward.Event is GameEvent<double> doubleEvent)
                {
                    doubleEvent.Raise(reward.Amount);
                }
                else if (reward.Event is GameEvent<int> intEvent)
                {
                    intEvent.Raise((int)reward.Amount);
                }
                else
                {
                    Debug.LogWarning($"Unsupported event type for reward: {reward.Event.GetType()}");
                }
            }
        }
    }
}
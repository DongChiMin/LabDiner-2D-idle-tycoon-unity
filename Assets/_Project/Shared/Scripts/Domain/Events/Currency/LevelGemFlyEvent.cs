
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Shared.Event
{
    public struct GemRewardData
    {
        public Vector3 startPos;   // Vị trí bắt đầu (thường là vị trí của đối tượng tạo ra gem)
        public int RewardValue;     // Giá trị phần thưởng (số lượng gem được thêm vào)
    }

    [CreateAssetMenu(fileName = "OnGemFlyAdded", menuName = "Events/Currency/Gem Fly Event")]
    public class LevelGemFlyEvent : GameEvent<GemRewardData>
    {
    }
}
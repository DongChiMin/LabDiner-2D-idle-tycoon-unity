
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Shared.Event
{
    public struct CoinRewardData
    {
        public Vector3 startPos;   // Vị trí bắt đầu (thường là vị trí của đối tượng tạo ra coin)
        public int RewardValue;     // Giá trị phần thưởng (số lượng coin được thêm vào)
    }

    [CreateAssetMenu(fileName = "OnCoinFlyAdded", menuName = "Events/Currency/Coin Fly Event")]
    public class LevelCoinFlyEvent : GameEvent<CoinRewardData>
    {
    }
}
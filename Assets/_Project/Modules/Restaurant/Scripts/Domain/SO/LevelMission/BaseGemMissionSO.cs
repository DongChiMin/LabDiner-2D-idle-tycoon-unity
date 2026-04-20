using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    // public enum MissionType
    // {
    //     UpgradeDish,      // Nâng cấp món a đến level b
    //     CollectTips,    // Nhặt a lần tiền tip
    //     HireChefs,      // Tổng số lượng đầu bếp đạt a
    //     SetupStation,  // Mở khóa trạm chính a
    //     CompleteOrder,     // Phục vụ món cho khách a lần
    //     BuyUpgrades,       // Mua a lần nâng cấp (có thể là bất kỳ nâng cấp nào, miễn là mua thành công)
    // }

    public abstract class BaseGemMissionSO : ScriptableObject
    {
        [Header("Mission Info")]
        public string Title;
        public Sprite MissionIcon;
        public float MissionValue;

        [Header("Reward Info")]
        public Sprite RewardIcon;
        public float RewardValue;
        public LevelGemFlyEvent OnGemFlyAdded;

        public void ApplyReward(Vector3 startPos)
        {
            // Gửi chính Asset này đi qua Event
            if (OnGemFlyAdded != null)
                OnGemFlyAdded.Raise(
                    new GemRewardData
                    {
                        startPos = startPos,
                        RewardValue = Mathf.RoundToInt(RewardValue)
                    });
        }
    }
}
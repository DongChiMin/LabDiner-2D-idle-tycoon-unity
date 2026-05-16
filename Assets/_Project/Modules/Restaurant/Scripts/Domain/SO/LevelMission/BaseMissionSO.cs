using System;
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

    public abstract class BaseMissionSO : ScriptableObject
    {
        public Action OnValueChanged;

        [Header("Mission Info")]
        public string Title;
        public Sprite MissionIcon;
        public float TargetValue;

        [Header("Reward")]
        public BaseRewardSO Reward;
        public double RewardValue;

        // Hàm abstract để mỗi loại mission tự định nghĩa cách lấy giá trị hiện tại
        public abstract float GetCurrentValue();

        // Kiểm tra xem đã hoàn thành chưa
        public virtual bool IsCompleted() => GetCurrentValue() >= TargetValue;

        /// <summary>
        /// Gọi phương thức này khi người chơi hoàn thành nhiệm vụ và muốn nhận phần thưởng. Nó sẽ kích hoạt hiệu ứng bay gem từ vị trí startPos đến HUD.
        /// </summary>
        /// <param name="startPos"></param>
        public void ApplyReward(Vector3 startPos)
        {
            // Gửi chính Asset này đi qua Event
            if (Reward != null)
                Reward.ApplyReward(startPos, RewardValue);
        }
    }
}
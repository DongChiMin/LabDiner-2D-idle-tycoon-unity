using System;
using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "Level Config", menuName = "Game/Level Config")]
    public class LevelConfigSO : ScriptableObject
    {
        [Header("Level Info")]
        public GameObject LevelPrefab;
        public int LevelIndex;
        public string LevelName;
        public List<BaseUpgradeSO> AvailableUpgrades;
        public List<BaseGemMissionSO> AvailableMissions;
        public BaseGemMissionSO FinalMission;
        public Action OnLevelComplete;

        [Header("Feature Settings")]
        public bool WaitingLine;
        public List<Staff> InitialStaffs;
        public double InitialMoney = 10;
        public int InitialGuestQuantity = 2;
        
        [Header("Core Station Settings")]
        public List<CoreStationSO> CoreStations;   // Danh sách các trạm chính có trong level, dùng để tính toán tiến độ level dựa trên level max của các trạm chính này

        [Header("Guest Order Settings")]
        public int MaxUniqueStations;           // Số lượng trạm chính khác nhau tối đa có trong đơn hàng của khách (ví dụ: 2 thì khách chỉ gọi món từ 2 trạm chính khác nhau, dù có thể gọi nhiều món từ mỗi trạm)
        public int MaxTotalQtyPerOrder;         // Số lượng món tối đa trong đơn hàng của khách (ví dụ: 5 thì khách chỉ gọi tối đa 5 món, dù có thể là 5 món từ cùng 1 trạm chính)

        [Header("--- Camera Settings ---")]
        public float minVerticalPos; // Điểm thấp nhất camera có thể xuống (thường là 0)
        public float maxVerticalPos; // Điểm cao nhất camera có thể lên (tùy độ dài nhà hàng)

        [Header("Summary")]
        [ReadOnly] public int MaxGuestQuantity => FetchGuestQuantity();
        public 

        void OnValidate()
        {
            FetchGuestQuantity();
        }

        private int FetchGuestQuantity()
        {
            int quantity = InitialGuestQuantity;
            foreach(BaseUpgradeSO upgrade in AvailableUpgrades)
            {
                if (upgrade is GuestUpgradeSO guestUpgrade)
                {
                    if (guestUpgrade.UpgradeType == GuestUpgradeType.Quantity)
                    {
                        quantity += Mathf.RoundToInt(guestUpgrade.UpgradeValue);
                    }
                }
            }
            return quantity;
        }
    }
}
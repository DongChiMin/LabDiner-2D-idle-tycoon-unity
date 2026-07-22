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
        public string ID;
        public GameObject LevelPrefab;
        public int LevelIndex;
        public string LevelName;
        public string LevelDescription;
        public Sprite LevelIcon;
        public List<BaseUpgradeSO> AvailableUpgrades;
        public List<BaseMissionSO> AvailableMissions;
        public Action OnLevelComplete;

        [Header("Feature Settings")]
        public bool WaitingLine;
        public List<Staff> InitialStaffs;
        public double InitialMoney = 10;
        public int InitialGuestQuantity = 2;

        [Header("Guest Order Settings")]
        public int MaxUniqueStations;           // Số lượng trạm chính khác nhau tối đa có trong đơn hàng của khách (ví dụ: 2 thì khách chỉ gọi món từ 2 trạm chính khác nhau, dù có thể gọi nhiều món từ mỗi trạm)
        public int MaxTotalQtyPerOrder;         // Số lượng món tối đa trong đơn hàng của khách (ví dụ: 5 thì khách chỉ gọi tối đa 5 món, dù có thể là 5 món từ cùng 1 trạm chính)

        [Header("--- Camera Settings ---")]
        public float minVerticalPos; // Điểm thấp nhất camera có thể xuống (thường là 0)
        public float maxVerticalPos; // Điểm cao nhất camera có thể lên (tùy độ dài nhà hàng)

        [Header("Summary")]
        [Tooltip("ReadOnly")]
        public int MaxGuestQuantity => FetchGuestQuantity(); 

        void OnValidate()
        {
            FetchGuestQuantity();

            //Đảm bảo value nhiệm vụ upgrade ko vượt quá số lượng upgrade có thể có trong level
            foreach(var mission in AvailableMissions)
            {
                if(mission is UpgradeMissionSO upgradeMission)
                {
                    //Kiểm tra nếu nhiệm vụ có target upgrade cụ thể, thì upgrade đó phải nằm trong danh sách AvailableUpgrades của level
                    if(upgradeMission.TargetUpgrade != null && !AvailableUpgrades.Contains(upgradeMission.TargetUpgrade))
                    {
                        Debug.LogWarning($"Mission {upgradeMission.name} has target upgrade {upgradeMission.TargetUpgrade.name} that is not in AvailableUpgrades list. Please add it to the list or clear the target.");
                    }

                    //Kiểm tra tổng số upgrade nhỏ hơn missionValue 
                    if(upgradeMission.TargetUpgrade == null && upgradeMission.TargetValue > AvailableUpgrades.Count)
                    {
                        Debug.LogWarning($"Mission {upgradeMission.name} has mission value {upgradeMission.TargetValue} that exceeds the total number of available upgrades {AvailableUpgrades.Count}. Please adjust the mission value.");
                    }
                }
            }
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
using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Manager;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum OrderMissionType
    {
        SellQuantity,      // Bán được số lượng món ăn từ core station a
        SellQuantityFromAllStations  // Bán được số lượng món ăn từ tất cả core
    }

    /// <summary>
    /// Nhiệm vụ yêu cầu nâng cấp level của một core Station cụ thể lên một mốc nhất định nào đó (ví dụ: nâng cấp Core Station Burger lên level 10)
    /// </summary>
    [CreateAssetMenu(fileName = "New Order Mission", menuName = "Game/Missions/Order Mission")]
    public class OrderMissionSO : BaseMissionSO
    {
        [Header("Target")]
        public CoreStationSO TargetCoreStation;
        public OrderMissionType MissionType;

        [Header("Static")]
        [SerializeField] private CoreStationEvent _onGuestReceiveFood;  //thông báo khách nhận đc món gì

        private Dictionary<CoreStation, int> _orderCount = new Dictionary<CoreStation, int>();

        void OnEnable()
        {
            if (_onGuestReceiveFood != null)
            {
                _onGuestReceiveFood.Register(HandleGuestReceiveFood);
            }
            OnMissionStart += HandleMissionStart;
        }

        void OnDisable()
        {
            if (_onGuestReceiveFood != null)
            {
                _onGuestReceiveFood.Unregister(HandleGuestReceiveFood);
            }
            OnMissionStart -= HandleMissionStart;
        }


        private void HandleValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public override float GetCurrentValue()
        {
            switch (MissionType)
            {
                case OrderMissionType.SellQuantity:
                    // Lấy số lượng món ăn đã bán từ core station tương ứng từ _orderCount
                    foreach (var coreStation in _orderCount.Keys)
                    {
                        if (coreStation.CoreStationSO == TargetCoreStation)
                        {
                            return _orderCount[coreStation];
                        }
                    }
                    return 0;
                case OrderMissionType.SellQuantityFromAllStations:
                    // Tính tổng số lượng món ăn đã bán từ tất cả các core station
                    return _orderCount.Values.Sum();
                default:
                    Debug.LogError("Unsupported Mission Type");
                    return 0;
            }

        }

        private void HandleGuestReceiveFood(CoreStation coreStation)
        {
            if (!_orderCount.ContainsKey(coreStation))
            {
                _orderCount[coreStation] = 0;
            }
            _orderCount[coreStation]++;
            HandleValueChanged();
        }

        private void HandleMissionStart()
        {
            // Reset order count when mission starts
            _orderCount.Clear();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            if(MissionType == OrderMissionType.SellQuantityFromAllStations)
            {
                TargetCoreStation = null; // Không cần target core station khi mission type là SellQuantityFromAllStations
            }
        }
#endif
    }
}

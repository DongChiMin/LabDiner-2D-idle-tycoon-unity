using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class CoreStationManager : MonoBehaviour, ILevelInitializable, ILevelRebuildable
    {
        public List<CoreStation> CoreStations => coreStations;

        [Header("Events")]
        [SerializeField] private LevelUpgradeEvent _onAllDishProfitUpgrade;
        [SerializeField] private DishUpgradeEvent _onDishProfitUpgrade;
        [SerializeField] private DishUpgradeEvent _onDishProcessTimeUpgrade;

        [Header("Core Stations")]
        [SerializeField] private CoreStationRuntimeSO coreStationRuntimeSO;

        [Header("[Runtime]")]
        [SerializeField] private List<CoreStation> coreStations;

        void OnEnable()
        {
            _onAllDishProfitUpgrade.Register(HandleAllDishProfitUpgrade);
            _onDishProfitUpgrade.Register(HandleDishProfitUpgrade);
            _onDishProcessTimeUpgrade.Register(HandleDishProcessTimeUpgrade);
        }

        void OnDisable()
        {
            _onAllDishProfitUpgrade.Unregister(HandleAllDishProfitUpgrade);
            _onDishProfitUpgrade.Unregister(HandleDishProfitUpgrade);
            _onDishProcessTimeUpgrade.Unregister(HandleDishProcessTimeUpgrade);
        }


        #region API

        public void Rebuild()
        {
            //Xóa list cũ, tạo mới list, thêm các trạm chính vào list
            coreStations.Clear();
            coreStations = new List<CoreStation>(gameObject.GetComponentsInChildren<CoreStation>());

            //Cập nhật vào RuntimeSO
            coreStationRuntimeSO.Clear();
            foreach(var station in coreStations)
            {
                coreStationRuntimeSO.AddCoreStation(station);
                station.OnMaxLevel += HandleCoreStationMaxLevel;
            }
        }

        public void Init(LevelConfigSO levelConfig)
        {
            List<CoreStationSO> coreStationSOs = levelConfig.CoreStations;
        }

        /// <summary>
        /// Kiểm tra xem có bất kỳ trạm chính nào đã được mở khóa hay không. Nếu chưa có trạm nào được mở khóa, nhà hàng sẽ không thể hoạt động vì không có trạm nào để phục vụ khách hàng.
        /// </summary>
        /// <returns></returns>
        public bool HasAnyUnlockedStation()
        {
            return coreStations.Exists(coreStation => coreStation.IsUnlocked);
        }

        /// <summary>
        /// Kiểm tra xem có bất kỳ trạm nào trong số các trạm chính đã được mở khóa có trạm con nào đang sẵn sàng để thực hiện nhiệm vụ hay không.
        /// </summary>
        /// <param name="station"></param>
        /// <returns></returns>
        public bool HasAnyAvailableStation(CoreStation station)
        {
            List<Station> stations = station.Stations;
            return stations.Exists(ws => ws.IsAvailable);
        }

        public Station PopAvailableStation(CoreStation station)
        {
            List<Station> stations = station.Stations;
            Station availableStation = stations.Find(ws => ws.IsAvailable);
            if (availableStation != null)
            {
                availableStation.SetStatus(false); // Đánh dấu trạm này đang bận
                return availableStation;
            }
            return null; // Không có trạm nào sẵn sàng
        }

        #endregion


        #region Private Methods

        /// <summary>
        /// Xử lý khi có một nâng cấp lợi nhuận áp dụng cho tất cả các món ăn.
        /// </summary>
        /// <param name="upgradeSO"></param>
        private void HandleAllDishProfitUpgrade(LevelUpgradeSO upgradeSO)
        {
            float profitBuffValue = upgradeSO.UpgradeValue;
            foreach (var station in coreStations)
            {
                station.UpgradeProfit(profitBuffValue);
            }
        }

        /// <summary>
        /// Xử lý khi có một món ăn cụ thể được nâng cấp lợi nhuận.
        /// <param name="upgradeSO"></param>
        private void HandleDishProfitUpgrade(DishUpgradeSO upgradeSO)
        {
            float profitBuffValue = upgradeSO.UpgradeValue;
            foreach (var station in coreStations)
            {
                if(station.VerifyData(upgradeSO.Dish))
                {
                    station.UpgradeProfit(profitBuffValue);
                }
            }
        }

        /// <summary>
        /// Xử lý khi có một món ăn cụ thể được nâng cấp thời gian chế biến.
        /// </summary>
        /// <param name="upgradeSO"></param>
        private void HandleDishProcessTimeUpgrade(DishUpgradeSO upgradeSO)
        {
            float processTimeBuffValue = upgradeSO.UpgradeValue;
            foreach (var station in coreStations)
            {
                if(station.VerifyData(upgradeSO.Dish))
                {
                    station.UpgradeProcessTime(processTimeBuffValue);
                }
            }
        }

        private void HandleCoreStationMaxLevel(int maxStar)
        {
            bool isAllStationMaxLevel = coreStations.TrueForAll(s => s.IsMaxLevel);
        }
        #endregion
    }
}
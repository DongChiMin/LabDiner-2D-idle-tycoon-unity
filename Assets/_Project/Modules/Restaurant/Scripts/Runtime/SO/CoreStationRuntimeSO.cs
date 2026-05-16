

using System;
using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "CoreStationRuntimeSet", menuName = "SO/Runtime/CoreStation")]
    public class CoreStationRuntimeSO : ScriptableObject
    {
        //Được gọi khi có trạm bất kỳ được cập nhật
        public Action OnValueChanged;

        // Danh sách CoreStation có trên level, được quản lý bởi CoreStationManager
        public List<CoreStation> CoreStations => coreStations;
        [SerializeField] private List<CoreStation> coreStations = new List<CoreStation>();

        public bool HasAnyUnlockedStation()
        {
            return coreStations.Exists(coreStation => coreStation.IsUnlocked);
        }

        public void AddCoreStation(CoreStation coreStation)
        {
            if (!coreStations.Contains(coreStation))
            {
                coreStations.Add(coreStation);
            }
        }
        public int GetCoreStationLevel(CoreStationSO coreStationSO)
        {
            var station = coreStations.Find(s => s.CoreStationSO == coreStationSO);
            return station != null ? station.CurrentLevel : 0;
        }

        /// <summary>
        /// Tạo một order ngẫu nhiên dựa trên các trạm đã mở khóa, với số lượng loại trạm tối đa và tổng số lượng món ăn tối đa được chỉ định.
        ///VD: maxUniqueStations = 1, maxTotalQty = 5 => thì chỉ được tạo món từ 1 trạm, max số lượng 5 
        /// </summary>
        /// <param name="maxUniqueStations"></param>
        /// <param name="maxTotalQty"></param>
        /// <returns></returns>
        public Dictionary<CoreStation, int> GenerateRandomOrder(int maxUniqueStations, int maxTotalQty)
        {
            Dictionary<CoreStation, int> order = new Dictionary<CoreStation, int>();

            List<CoreStation> unlockedStations = GetUnlockedStations();
            if (unlockedStations.Count == 0) return order;

            int totalQtyToGenerate = UnityEngine.Random.Range(1, maxTotalQty + 1);

            // 3. Xác định số lượng loại trạm (Unique Stations) tối đa cho đơn hàng này
            // Không được vượt quá số trạm hiện có và không vượt quá giới hạn tham số
            int allowedUniqueCount = UnityEngine.Random.Range(1, Mathf.Min(maxUniqueStations, unlockedStations.Count) + 1);

            // 4. Chọn ra danh sách các trạm cụ thể sẽ xuất hiện trong đơn hàng này
            List<CoreStation> selectedStationsForThisOrder = new List<CoreStation>();
            for (int i = 0; i < allowedUniqueCount; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, unlockedStations.Count);
                selectedStationsForThisOrder.Add(unlockedStations[randomIndex]);
                unlockedStations.RemoveAt(randomIndex); // Để không chọn trùng trạm trong bước chọn loại
            }

            // 5. Phân bổ tổng số lượng (totalQtyToGenerate) vào các trạm đã chọn
            for (int i = 0; i < totalQtyToGenerate; i++)
            {
                // Bốc ngẫu nhiên 1 trạm trong danh sách trạm đã được chọn ở bước 4
                CoreStation randomStation = selectedStationsForThisOrder[UnityEngine.Random.Range(0, selectedStationsForThisOrder.Count)];

                if (order.ContainsKey(randomStation))
                {
                    order[randomStation]++;
                }
                else
                {
                    order.Add(randomStation, 1);
                }
            }
            return order;
        }

        private List<CoreStation> GetUnlockedStations()
        {
            return coreStations.FindAll(station => station.IsUnlocked);
        }

        public void Clear() => coreStations.Clear();
    }
}
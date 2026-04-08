
using System;
using System.Collections.Generic;
using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CoreStationManager : MonoBehaviour
    {
        [SerializeField] private List<CoreStation> coreStations = new List<CoreStation>();
        
        #region API

        public void OnInit(LevelConfigSO levelConfig)
        {
            
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

        private List<CoreStation> GetUnlockedStations()
        {
            return coreStations.FindAll(station => station.IsUnlocked);
        }
    }
}
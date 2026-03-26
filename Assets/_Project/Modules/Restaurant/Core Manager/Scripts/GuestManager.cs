

using System.Collections;
using System.Collections.Generic;
using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GuestAI _guestPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private float _spawnInterval = 5f;

        [Header("[DEBUG]")]
        [SerializeField] private List<GuestContext> _guests;
        [SerializeField] private int _currentMaxGuests = 1;
        [Header("[DEBUG] Level Config")]
        [SerializeField] private int _levelMaxGuests = 10;
        [SerializeField] private int _maxUniqueStations = 2;
        [SerializeField] private int _maxTotalQty = 3;

        void Start()
        {
            // mỗi 5 giây thử spawn 1 lần
            StartCoroutine(SpawnLoop());
        }

        public void OnInit(LevelConfigSO levelConfigSO)
        {
            _levelMaxGuests = levelConfigSO.maxGuestCount;
            _maxUniqueStations = levelConfigSO.maxUniqueStations;
            _maxTotalQty = levelConfigSO.maxTotalQtyPerOrder;
        }

        IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                //1. Kiểm tra số lượng khách hiện tại có vượt quá max không
                if (_guests.Count >= _levelMaxGuests) continue;

                //2. Kiểm tra người chơi chưa mở khóa station nào -> không tạo khách mới
                if(!LevelManagerContext.Instance.coreStationManager.HasAnyUnlockedStation()) continue;

                //3. Spawn khách mới
                GuestContext guest = PoolContext.Instance.GuestPool.Get( _spawnPoint.position, Quaternion.identity);
                _guests.Add(guest);

                //4. Tạo danh sách món ăn khách muốn gọi
                Dictionary<CoreStation, int> order = LevelManagerContext.Instance.coreStationManager.GenerateRandomOrder(_maxUniqueStations, _maxTotalQty);

                //5.[Hành động] Kiểm tra hàng chờ bên ngoài có > 0 không, nếu có thì khách mới sẽ đi vào hàng chờ
                if(LevelManagerContext.Instance.waitingLineManager.HasWaitingGuest) {
                    Vector3 destination = LevelManagerContext.Instance.waitingLineManager.AddToWaitingLine(guest);
                    guest.Setup(order, destination, _exitPoint.position);
                    continue;
                }

                //6.[Hành động] Chọn bàn ăn available ngẫu nhiên để khách ngồi, nếu không có bàn nào available thì khách sẽ đi vào hàng chờ
                List<DiningTable> availableTables = LevelManagerContext.Instance.diningTableManager.GetAvailableTables();
                if (availableTables.Count > 0)
                {
                    DiningTable selectedTable = availableTables[Random.Range(0, availableTables.Count)];
                    LevelManagerContext.Instance.diningTableManager.OccupyTable(selectedTable, guest);
                    guest.Setup(order, selectedTable.transform.position, _exitPoint.position);
                    continue;
                }
                else
                {
                    Vector3 destination = LevelManagerContext.Instance.waitingLineManager.AddToWaitingLine(guest);
                    guest.Setup(order, destination, _exitPoint.position);
                    continue;
                }
            }
        }
    }
}
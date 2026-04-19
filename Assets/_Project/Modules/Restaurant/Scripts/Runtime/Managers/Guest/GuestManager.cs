

using System.Collections;
using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestManager : MonoBehaviour
    {
        [Header("Upgrade Events")]
        [SerializeField] private GlobalUpgradeEvent _onUpgradeGuestQuantity;

        [Header("Spawn Settings")]
        [SerializeField] private GuestAI _guestPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private float _spawnInterval = 5f;

        [Header("Event")]
        [SerializeField] private GuestEvent _onGuestLeft;
        [SerializeField] private GuestQuantityEvent _onGuestQuantityChanged;

        [Header("[DEBUG]")]
        [SerializeField] private List<GuestContext> _guests;
        [SerializeField] private int _currentMaxGuests = 1;
        [Header("[DEBUG] Level Config")]
        [SerializeField] private int _maxUniqueStations = 2;
        [SerializeField] private int _maxTotalQty = 3;

        void OnEnable()
        {
            _onGuestLeft.Register(RemoveGuest);
            _onUpgradeGuestQuantity.Register(HandleUpgradeGuestQuantity);
        }

        void OnDisable()
        {
            _onGuestLeft.Unregister(RemoveGuest);
            _onUpgradeGuestQuantity.Unregister(HandleUpgradeGuestQuantity);
        }

        void Start()
        {
            // mỗi 5 giây thử spawn 1 lần
            StartCoroutine(SpawnLoop());
            _onGuestQuantityChanged.Raise(_currentMaxGuests);
        }

        public void OnInit(LevelConfigSO levelConfigSO)
        {
            _maxUniqueStations = levelConfigSO.MaxUniqueStations;
            _maxTotalQty = levelConfigSO.MaxTotalQtyPerOrder;
        }

        public GuestContext SpawnGuest()
        {
            GuestContext guest = PoolContext.Instance.GuestPool.Get(_spawnPoint.position, Quaternion.identity);
            _guests.Add(guest);
            return guest;
        }

        public void RemoveGuest(GuestContext guest)
        {
            _guests.Remove(guest);
            PoolContext.Instance.GuestPool.ReturnToPool(guest);
        }

        IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                //0. Các biến cần sử dụng
                bool HasWaitingLine = LevelManagerContext.Instance.HasWaitingLine;
                bool hasAnyUnlockedStation = LevelManagerContext.Instance.CoreStationManager.HasAnyUnlockedStation();
                List<DiningSeat> availableSeats = LevelManagerContext.Instance.DiningTableManager.GetAvailableSeats();


                //1. Kiểm tra số lượng khách hiện tại có vượt quá max không
                if (_guests.Count >= _currentMaxGuests) continue;

                //2. Kiểm tra người chơi chưa mở khóa station nào -> không tạo khách mới
                if (!hasAnyUnlockedStation) continue;

                //3. Kiểm tra nếu không dùng WaitingLine và hết bàn trong restaurant: không tạo khách mới
                if (!HasWaitingLine && availableSeats.Count <= 0) continue;

                //3. Spawn khách mới
                GuestContext guest = SpawnGuest();

                //4. Tạo danh sách món ăn khách muốn gọi
                Dictionary<CoreStation, int> orderDict = LevelManagerContext.Instance.CoreStationManager.GenerateRandomOrder(_maxUniqueStations, _maxTotalQty);
                Order order = new Order(orderDict, guest, 0, false);

                //5.[Hành động] Kiểm tra hàng chờ bên ngoài có > 0 không, nếu có thì khách mới sẽ đi vào hàng chờ
                if (HasWaitingLine && LevelManagerContext.Instance.WaitingLineManager.HasWaitingGuest)
                {
                    Vector3 destination = LevelManagerContext.Instance.WaitingLineManager.AddToWaitingLine(guest);
                    guest.Setup(order, destination, _exitPoint.position);
                    continue;
                }

                //6.[Hành động] Nếu có bàn trống, khách sẽ ngồi vào bàn đó, set trạng thái bàn occupied, và bắt đầu hành trình của khách
                if (availableSeats.Count > 0)
                {
                    DiningSeat selectedSeat = availableSeats[Random.Range(0, availableSeats.Count)];
                    LevelManagerContext.Instance.DiningTableManager.OccupySeat(selectedSeat, guest);
                    guest.Setup(order, selectedSeat.transform.position, _exitPoint.position, selectedSeat);
                    continue;
                }
                //7.[Hành động] Nếu không có bàn trống nhưng có hàng chờ, khách sẽ đi vào hàng chờ
                else if (HasWaitingLine)
                {
                    Vector3 destination = LevelManagerContext.Instance.WaitingLineManager.AddToWaitingLine(guest);
                    guest.Setup(order, destination, _exitPoint.position);
                    continue;
                }
                //Nếu đúng logic thì phần này sẽ không bao giờ xảy ra
                else
                {
                    Debug.LogWarning("Không có bàn nào available và không có hàng chờ, khách sẽ không được spawn");
                    RemoveGuest(guest);
                }
            }
        }

        private void HandleUpgradeGuestQuantity(GlobalUpgradeSO upgradeSO)
        {
            _currentMaxGuests = _currentMaxGuests + Mathf.RoundToInt(upgradeSO.UpgradeValue);
            _onGuestQuantityChanged.Raise(_currentMaxGuests);
        }
    }
}
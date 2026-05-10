using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.Runtime;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class GuestSpawner : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private CoreStationRuntimeSO _coreStationRuntime;
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

        [Header("Dependency")]
        [SerializeField] private GuestManager _guestManager;

        [Header("[Runtime]")]
        [SerializeField] private int _currentMaxGuests = 1;
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

        public void Init(LevelConfigSO levelConfigSO)
        {
            _maxUniqueStations = levelConfigSO.MaxUniqueStations;
            _maxTotalQty = levelConfigSO.MaxTotalQtyPerOrder;
        }

        public GuestContext SpawnGuest()
        {
            GuestContext guest = PoolContext.Instance.GuestPool.Get(_spawnPoint.position, Quaternion.identity);
            Debug_AddGuest(guest);
            return guest;
        }

        public void RemoveGuest(GuestContext guest)
        {
            Debug_RemoveGuest(guest);
            PoolContext.Instance.GuestPool.ReturnToPool(guest);
        }

        IEnumerator SpawnLoop()
        {
            while (true)
            {
                yield return new WaitForSeconds(_spawnInterval);

                if (!_coreStationRuntime.HasAnyUnlockedStation())
                {
                    continue; // Nếu chưa có trạm nào được mở khóa, không spawn khách
                }

                GuestContext guest = SpawnGuest();
                _guestManager.Assign(guest, _exitPoint.position, _maxUniqueStations, _maxTotalQty);
            }
        }

        private void HandleUpgradeGuestQuantity(GlobalUpgradeSO upgradeSO)
        {
            _currentMaxGuests = _currentMaxGuests + Mathf.RoundToInt(upgradeSO.UpgradeValue);
            _onGuestQuantityChanged.Raise(_currentMaxGuests);
        }

        #region Debug
        partial void Debug_AddGuest(GuestContext guest);
        partial void Debug_RemoveGuest(GuestContext guest);
        #endregion
    }
}
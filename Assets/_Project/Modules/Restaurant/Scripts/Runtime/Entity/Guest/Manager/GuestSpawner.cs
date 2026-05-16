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
        [SerializeField] private GuestUpgradeEvent _onGuestUpgrade;

        [Header("Spawn Settings")]
        [SerializeField] private GuestAI _guestPrefab;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private float _spawnInterval = 5f;

        [Header("Event")]
        [SerializeField] private GuestEvent _onGuestLeft;
        [SerializeField] private IntEvent _onGuestQuantityChanged;

        [Header("Dependency")]
        [SerializeField] private GuestManager _guestManager;

        [Header("[Runtime]")]
        [SerializeField] private int _currentMaxGuests = 1;
        [SerializeField] private int _maxUniqueStations = 2;
        [SerializeField] private int _maxTotalQty = 3;

        private float _currentSpawntimeBuff = 0;

        void OnEnable()
        {
            _onGuestLeft.Register(RemoveGuest);
            _onGuestUpgrade.Register(HandleUpgradeGuest);
        }

        void OnDisable()
        {
            _onGuestLeft.Unregister(RemoveGuest);
            _onGuestUpgrade.Unregister(HandleUpgradeGuest);
        }

        public void Init(LevelConfigSO levelConfigSO)
        {
            _maxUniqueStations = levelConfigSO.MaxUniqueStations;
            _currentMaxGuests = levelConfigSO.InitialGuestQuantity;
            _maxTotalQty = levelConfigSO.MaxTotalQtyPerOrder;

            StartCoroutine(SpawnLoop());
            _onGuestQuantityChanged.Raise(_currentMaxGuests);
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

        private void HandleUpgradeGuest(GuestUpgradeSO upgradeSO)
        {
            if (upgradeSO.UpgradeType == GuestUpgradeType.Quantity)
            {
                _currentMaxGuests = _currentMaxGuests + Mathf.RoundToInt(upgradeSO.UpgradeValue);
                _onGuestQuantityChanged.Raise(_currentMaxGuests);
            }
            else if (upgradeSO.UpgradeType == GuestUpgradeType.SpawnTime)
            {
                _spawnInterval = _spawnInterval / (1 + _currentSpawntimeBuff);
            }
        }

        #region Debug
        partial void Debug_AddGuest(GuestContext guest);
        partial void Debug_RemoveGuest(GuestContext guest);
        #endregion
    }
}
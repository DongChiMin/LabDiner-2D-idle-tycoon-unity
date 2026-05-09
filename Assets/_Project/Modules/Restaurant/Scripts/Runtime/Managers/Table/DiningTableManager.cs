using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class DiningTableManager : MonoBehaviour, ILevelInitializable
    {
        [Header("Events")]
        [SerializeField] private GuestQuantityEvent _onGuestQuantityChanged;
        [SerializeField] private TableEvent _onTableFreed;
        [SerializeField] private TableEvent _onTableDirty;

        [Header("References")]
        [SerializeField] private DiningSeatRuntimeSetSO _diningSeatRuntimeSet;
        [SerializeField] private List<DiningTable> _tables = new List<DiningTable>();

        void OnEnable()
        {
            _onTableDirty.Register(SetSeatDirty);
            _onGuestQuantityChanged.Register(HandleGuestQuantityChanged);
        }

        void OnDisable()
        {
            _onTableDirty.Unregister(SetSeatDirty);
            _onGuestQuantityChanged.Unregister(HandleGuestQuantityChanged);
        }

        void Awake()
        {
            _diningSeatRuntimeSet.Clear();
            foreach(DiningTable table in _tables)
            {
                table.gameObject.SetActive(false);
            }
        }

        public void Init(LevelConfigSO config)
        {
            _levelConfig = config;
            Debug_ValidateData();
        }


        #region Private Methods
        private void SetSeatDirty(DiningSeat seat)
        {
            //TODO: có thể thêm hiệu ứng ghế bẩn ở đây
            seat.Occupy(null);

            //Sau khi dọn xong
            _onTableFreed.Raise(seat);
        }

        private void HandleGuestQuantityChanged(int quantity)
        {
            if(_spawnedSeats.Count < quantity)
            {
                SpawnNewTable();
            }
        }

        private void SpawnNewTable()
        {
            int index = _spawnedTables.Count;
            DiningTable spawnTable = _tables[index];
            spawnTable.gameObject.SetActive(true);

            foreach(DiningSeat seat in spawnTable.Seats)
            {
                _diningSeatRuntimeSet.Add(seat);
            }
            Debug_FetchData();
        }

        #endregion

        private partial void Debug_FetchData();
    }
}
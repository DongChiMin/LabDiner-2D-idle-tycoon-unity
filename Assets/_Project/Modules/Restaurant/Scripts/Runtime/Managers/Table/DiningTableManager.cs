
using System.Collections.Generic;
using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public struct Table
    {
        public GameObject TableInstance;
        public List<DiningTable> Seats;
    }

    public class DiningTableManager : MonoBehaviour, ILevelInitializable
    {
        [Header("Events")]
        [SerializeField] private GuestQuantityEvent _onGuestQuantityChanged;
        [SerializeField] private TableEvent _onTableFreed;
        [SerializeField] private TableEvent _onTableDirty;

        [Header("References")]
        [SerializeField] private List<Table> _tables = new List<Table>();

        [Header("DEBUG")]
        [SerializeField] private List<DiningTable> _spawnedSeats = new List<DiningTable>();
        [SerializeField] private List<Table> _spawnedTables = new List<Table>();
        private LevelConfigSO _levelConfig;

        void OnEnable()
        {
            _onTableDirty.Register(SetTableDirty);
            _onGuestQuantityChanged.Register(HandleGuestQuantityChanged);
        }

        void OnDisable()
        {
            _onTableDirty.Unregister(SetTableDirty);
            _onGuestQuantityChanged.Unregister(HandleGuestQuantityChanged);
        }

        public void Init(LevelConfigSO config)
        {
            _levelConfig = config;
            #if UNITY_EDITOR
            ValidateData();
            #endif
        }

        #region API
        public List<DiningTable> GetAvailableTables()
        {
            List<DiningTable> availableSeats = new List<DiningTable>();
            foreach (var seat in _spawnedSeats)
            {
                if (!seat.IsOccupied)
                    availableSeats.Add(seat);
            }
            return availableSeats;   
        }

        public void OccupyTable(DiningTable table, GuestContext guest)
        {
            table.Occupy(guest);
        }

        #endregion


        #region Private Methods
        private void SetTableDirty(DiningTable table)
        {
            Debug.Log("TODO: có thể set trạng thái bàn bẩn vào đây");
            table.Occupy(null);

            //Sau khi dọn xong
            _onTableFreed.Raise(table);
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
            Table spawnTable = _tables[index];
            _spawnedTables.Add(spawnTable);

            spawnTable.TableInstance.SetActive(true);
            foreach(DiningTable seat in spawnTable.Seats)
            {
                _spawnedSeats.Add(seat);
            }
        }

        #endregion

        #if UNITY_EDITOR
        private void ValidateData()
        {
            int totalSeat = 0;
            foreach(Table table in _tables)
            {
                totalSeat += table.Seats.Count;
            }
            if (_levelConfig.MaxGuestCount > totalSeat)
            {
                Debug.LogError($"Số lượng khách tối đa: {_levelConfig.MaxGuestCount} vượt quá số ghế có thể mở khóa: {totalSeat}. Cần điều chỉnh lại LevelConfigSO hoặc thêm nhiều bàn hơn!");
            }
        }

        void OnDrawGizmos(){
            if(_tables == null) return;

            foreach(Table table in _tables)
            {
                foreach(DiningTable seat in table.Seats)
                {
                    Gizmos.color = seat.IsOccupied ? Color.red : Color.green;
                    Gizmos.DrawWireSphere(seat.transform.position, 0.2f);
                }
            }
        }
        #endif
    }
}
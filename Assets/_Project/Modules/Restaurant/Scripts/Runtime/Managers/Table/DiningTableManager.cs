using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class DiningTableManager : MonoBehaviour, ILevelInitializable
    {
        [Header("Events")]
        [SerializeField] private GuestQuantityEvent _onGuestQuantityChanged;
        [SerializeField] private TableEvent _onTableFreed;
        [SerializeField] private TableEvent _onTableDirty;

        [Header("References")]
        [SerializeField] private List<DiningTable> _tables = new List<DiningTable>();

        [Header("DEBUG")]
        [SerializeField] private List<DiningSeat> _spawnedSeats = new List<DiningSeat>();
        [SerializeField] private List<DiningTable> _spawnedTables = new List<DiningTable>();
        private LevelConfigSO _levelConfig;

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

        public void Init(LevelConfigSO config)
        {
            _levelConfig = config;
            #if UNITY_EDITOR
            ValidateData();
            #endif
        }

        #region API
        public List<DiningSeat> GetAvailableSeats()
        {
            List<DiningSeat> availableSeats = new List<DiningSeat>();
            foreach (var seat in _spawnedSeats)
            {
                if (!seat.IsOccupied)
                    availableSeats.Add(seat);
            }
            return availableSeats;   
        }

        public void OccupySeat(DiningSeat seat, GuestContext guest)
        {
            seat.Occupy(guest);
        }

        #endregion


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
            _spawnedTables.Add(spawnTable);

            spawnTable.gameObject.SetActive(true);
            foreach(DiningSeat seat in spawnTable.Seats)
            {
                _spawnedSeats.Add(seat);
            }
        }

        #endregion

        #if UNITY_EDITOR
        private void ValidateData()
        {
            int totalSeat = 0;
            foreach(DiningTable table in _tables)
            {
                totalSeat += table.Seats.Count;
            }

            if (_levelConfig.maxGuestQuantity > totalSeat)
            {
                Debug.LogError($"Số lượng khách tối đa: {_levelConfig.maxGuestQuantity} vượt quá số ghế có thể mở khóa: {totalSeat}. Cần điều chỉnh lại LevelConfigSO hoặc thêm nhiều bàn hơn!");
            }
        }

        void OnDrawGizmos(){
            if(_tables == null) return;

            foreach(DiningTable table in _tables)
            {
                foreach(DiningSeat seat in table.Seats)
                {
                    Gizmos.color = seat.IsOccupied ? Color.red : Color.green;
                    Gizmos.DrawWireSphere(seat.transform.position, 0.2f);
                }
            }
        }
        #endif
    }
}
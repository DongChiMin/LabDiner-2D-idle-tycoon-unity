using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class DiningTableManager : MonoBehaviour, ILevelInitializable, ILevelRebuildable
    {
        [Header("Events")]
        [SerializeField] private IntEvent _onGuestQuantityChanged;
        [SerializeField] private DiningSeatEvent _onTableFreed;
        [SerializeField] private DiningSeatEvent _onTableDirty;

        [Header("References")]
        [SerializeField] private DiningSeatRuntimeSO _diningSeatRuntimeSet;

        [Header("[Runtime]")]
        [SerializeField] private List<DiningTable> _tables = new List<DiningTable>();
        private int _spawnedTableCount;

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

        public void Rebuild()
        {
            //Xóa list cũ, tạo mới list, thêm các bàn ăn vào list
            _tables.Clear();
            _tables = new List<DiningTable>(gameObject.GetComponentsInChildren<DiningTable>());
            _spawnedTableCount = 0;

            //Xóa runtime set, thêm các ghế ăn vào runtime set
            _diningSeatRuntimeSet.Clear();
            foreach(DiningTable table in _tables)
            {
                table.gameObject.SetActive(false);
                table.Init();
            }
        }

        public void Init(LevelConfigSO config)
        {
            Debug_ValidateData(config);
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
            int tries = 0;
            while (_diningSeatRuntimeSet.ActiveSeats.Count < quantity && tries < 10) // Thêm điều kiện dừng để tránh vòng lặp vô hạn
            {
                if (_spawnedTableCount >= _tables.Count)
                {
                    Debug.LogWarning("DiningTableManager: Không còn bàn nào để spawn thêm.");
                    break;
                }

                SpawnNewTable();
                tries++;
            }
        }

        private void SpawnNewTable()
        {
            int index = _spawnedTableCount;
            if (index < 0 || index >= _tables.Count || _tables[index] == null)
            {
                Debug.LogError($"DiningTableManager: SpawnNewTable failed because _tables[{index}] is null. Please check if the number of tables assigned in the inspector is sufficient for the maximum guest quantity.");
                return;
            }
            DiningTable spawnTable = _tables[index];
            spawnTable.gameObject.SetActive(true);
            _spawnedTableCount++;

            foreach (DiningSeat seat in spawnTable.Seats)
            {
                _diningSeatRuntimeSet.Add(seat);
            }
            Debug_FetchData();
        }

        #endregion

        partial void Debug_FetchData();
        partial void Debug_ValidateData(LevelConfigSO config);
    }
}
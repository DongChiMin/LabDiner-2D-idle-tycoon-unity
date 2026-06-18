#if UNITY_EDITOR
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
        [Header("DEBUG")]
        [SerializeField] private List<DiningSeat> _spawnedSeats = new List<DiningSeat>();
        [SerializeField] private List<DiningTable> _spawnedTables = new List<DiningTable>();
        private LevelConfigSO _levelConfig;

        partial void Debug_FetchData(){
            _spawnedSeats.Clear();
            _spawnedTables.Clear();
            foreach(DiningTable table in _tables)
            {
                _spawnedTables.Add(table);
                _spawnedSeats.AddRange(table.Seats);
            }
        }

        partial void Debug_ValidateData(LevelConfigSO config)
        {
            _levelConfig = config;
            int totalSeat = 0;
            foreach(DiningTable table in _tables)
            {
                totalSeat += table.Seats.Count;
            }

            if (_levelConfig.MaxGuestQuantity > totalSeat)
            {
                Debug.LogError($"Số lượng khách tối đa: {_levelConfig.MaxGuestQuantity} vượt quá số ghế có thể mở khóa: {totalSeat}. Cần điều chỉnh lại LevelConfigSO hoặc thêm nhiều bàn hơn!");
            }
        }
    }
}
#endif
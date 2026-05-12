

using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "CoreStationRuntimeSet", menuName = "SO/Runtime//CoreStation")]
    public class CoreStationRuntimeSO : ScriptableObject
    {
        // Danh sách CoreStation có trên level, được quản lý bởi CoreStationManager
        [SerializeField] private List<CoreStation> coreStations = new List<CoreStation>();

        public bool HasAnyUnlockedStation()
        {
            return coreStations.Exists(coreStation => coreStation.IsUnlocked);
        }

        public void AddCoreStation(CoreStation coreStation)
        {
            if (!coreStations.Contains(coreStation))
            {
                coreStations.Add(coreStation);
            }
        }

        public void Clear() => coreStations.Clear();
    }
}
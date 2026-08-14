using UnityEngine;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;

namespace LabDiner.LevelSystem.Domain
{
    [System.Serializable]
    public struct StaffTypePrefabPair 
    {
        public StaffType StaffType;
        public Staff Prefab;
    }

    [CreateAssetMenu(fileName = "StaffPrefabRepository", menuName = "SO/Staff/StaffPrefabRepository")]
    public class StaffPrefabRepository : ScriptableObject
    {
        public List<StaffTypePrefabPair> StaffPrefabs;

        public Staff GetStaffPrefab(StaffType staffType)
        {
            foreach (var pair in StaffPrefabs)
            {
                if (pair.StaffType == staffType)
                {
                    return pair.Prefab;
                }
            }
            Debug.LogWarning($"Staff prefab for type {staffType} not found in StaffRepository.");
            return null;
        }

        public List<Staff> GetAllStaffPrefabs()
        {
            List<Staff> allPrefabs = new List<Staff>();
            foreach (var pair in StaffPrefabs)
            {
                allPrefabs.Add(pair.Prefab);
            }
            return allPrefabs;
        }
    }
}


using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "PassTableRuntimeSet", menuName = "SO/Runtime/PassTable")]
    public class PassTableRuntimeSO : ScriptableObject
    {
        // Danh sách PassTable có trên level, được quản lý bởi PassTableManager
        [SerializeField] private List<PassTable> _passTables = new List<PassTable>();

        public void Add(PassTable table)
        {
            if (!_passTables.Contains(table)) _passTables.Add(table);
        }

        public void Remove(PassTable table)
        {
            if (_passTables.Contains(table)) _passTables.Remove(table);
        }

        public PassTable GetAvailablePassTable()
        {
            if (_passTables == null || _passTables.Count == 0)
            {
                return null;
            }
            int index = Random.Range(0, _passTables.Count);
            return _passTables[index];
        }

        public void Clear() => _passTables.Clear();
    }
}
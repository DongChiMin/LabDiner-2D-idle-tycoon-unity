using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class PassTableManager : MonoBehaviour, ILevelRebuildable
    {
        [SerializeField] private List<PassTable> _passTables;
        [SerializeField] private PassTableRuntimeSO _passTableRuntimeSet;
        
        #region API
        /// <summary>
        /// Lấy PassTable ngẫu nhiên
        /// </summary>
        /// <returns></returns>
        public PassTable GetAvailablePassTable()
        {
            if (_passTables == null || _passTables.Count == 0)
            {
                Debug.LogWarning("Không có PassTable nào được gán trong PassTableManager!");
                return null;
            }
            int index = Random.Range(0, _passTables.Count);
            return _passTables[index];
        }

        public void Rebuild()
        {
            //Xóa list cũ, tạo mới list, thêm các bàn ăn vào list
            _passTables.Clear();
            _passTables = new List<PassTable>(gameObject.GetComponentsInChildren<PassTable>());

            //Xóa runtime set, thêm các ghế ăn vào runtime set
            _passTableRuntimeSet.Clear();
            foreach(PassTable table in _passTables)
            {
                _passTableRuntimeSet.Add(table);
                table.Init();
            }
        }
        #endregion
    }
}
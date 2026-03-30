
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class PassTableManager : MonoBehaviour
    {
        [SerializeField] private List<PassTable> _passTables;
        
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
        #endregion
    }
}
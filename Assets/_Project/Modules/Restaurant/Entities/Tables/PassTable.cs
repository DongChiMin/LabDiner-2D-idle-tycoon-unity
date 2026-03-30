
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class PassTable : MonoBehaviour
    {
        public Transform WorkPos => _workPos;
        [SerializeField] Transform _workPos;
        [Header("[DEBUG]")]
        [SerializeField] private List<CookingTask> tasksOnPassTable = new();

        #region API
        public void PlaceTaskOnPassTable(CookingTask task)
        {
            tasksOnPassTable.Add(task);
            Debug.Log($"Task {task} placed on PassTable {name}. Current tasks on PassTable: {tasksOnPassTable.Count}");
        }

        
        #endregion
    }
}
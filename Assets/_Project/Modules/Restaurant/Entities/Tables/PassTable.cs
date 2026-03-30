
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class PassTable : MonoBehaviour
    {
        public Transform WorkPos_PutOn => _putOnPos;
        public Transform WorkPos_PickUp => _pickUpPos;
        [Header("Settings")]
        [SerializeField] Transform _putOnPos;
        [SerializeField] Transform _pickUpPos;
        [Header("[DEBUG]")]
        [SerializeField] private List<CookingTask> tasksOnPassTable = new();

        #region API
        public void PlaceTaskOnPassTable(CookingTask task)
        {
            tasksOnPassTable.Add(task);
            Debug.Log($"Task {task} placed on PassTable {name}. Current tasks on PassTable: {tasksOnPassTable.Count}");
        }

        public void PickUpDish(CookingTask task)
        {
            if (tasksOnPassTable.Contains(task))
            {
                tasksOnPassTable.Remove(task);
                Debug.Log($"TODO: show tiến trình pick up tại đây. Task {task} picked up from PassTable {name}. Remaining tasks on PassTable: {tasksOnPassTable.Count}");
            }
            else
            {
                Debug.LogWarning($"Không nên xảy ra tình huống này!!");
            }
        }
        #endregion
    }
}
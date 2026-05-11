using System.Collections.Generic;
using LabDiner.Restaurant.Workflow;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public class PassTable : MonoBehaviour
    {
        public Transform WorkPos_PutOn => _putOnPos;
        public Transform WorkPos_PickUp => _pickUpPos;
        [Header("Settings")]
        [SerializeField] Transform _putOnPos;
        [SerializeField] Transform _pickUpPos;
        [Header("Visual")]
        [SerializeField] private GameObject _dishVisual;
        [SerializeField] private SpriteRenderer _dishIcon;
        [SerializeField] private TextMeshProUGUI _priceText;
        [Header("[DEBUG]")]
        [SerializeField] private List<CookingTask> tasksOnPassTable = new();
        void OnEnable()
        {
            ToggleDishVisual(false);
        }

        #region API
        public void PlaceTaskOnPassTable(CookingTask task)
        {
            tasksOnPassTable.Add(task);
            UpdateDishVisual(task);
            ToggleDishVisual(true);
        }

        public void PickUpDish(CookingTask task)
        {
            if (tasksOnPassTable.Contains(task))
            {
                tasksOnPassTable.Remove(task);
                if(tasksOnPassTable.Count == 0)
                    ToggleDishVisual(false);
                else
                {
                    UpdateDishVisual(tasksOnPassTable[tasksOnPassTable.Count - 1]);
                    ToggleDishVisual(true);
                }
            }
            else
            {
                Debug.LogWarning($"Không nên xảy ra tình huống này!!");
            }
        }
        #endregion

        private void UpdateDishVisual(CookingTask task)
        {
            _dishIcon.sprite = task.CoreStation.DishIcon;
            _priceText.text = task.Profit.ToString();   
        }

        private void ToggleDishVisual(bool isOn)
        {
            _dishVisual.SetActive(isOn);
        }
    }
}
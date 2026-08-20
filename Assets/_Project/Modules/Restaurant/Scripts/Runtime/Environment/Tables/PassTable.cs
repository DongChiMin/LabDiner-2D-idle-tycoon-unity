using System.Collections.Generic;
using LabDiner.Restaurant.Workflow;
using LabDiner.Shared.Extension;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public class PassTable : MonoBehaviour, IGizmosDrawable
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

        #region API
        public void Init()
        {
            tasksOnPassTable.Clear();
            ToggleDishVisual(false);
        }

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
                if (tasksOnPassTable.Count == 0)
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
            _priceText.text = CurrencyFormatter.Format(task.Profit);
        }

        private void ToggleDishVisual(bool isOn)
        {
            _dishVisual.SetActive(isOn);
        }

        #region Gizmos

        [Header("Gizmos Settings")]
        public bool ShowGizmos { get => _showGizmos; set => _showGizmos = value; }
        [SerializeField] private bool _showGizmos = true;
        [SerializeField] private Vector3 _putOnPosSize = new Vector3(1.5f, 2.25f, 0.1f);
        [SerializeField] private Vector3 _pickUpPosSize = new Vector3(1.5f, 2.25f, 0.1f);
        void OnDrawGizmos()
        {
            if (!_showGizmos) return;

            // 1. Vẽ vị trí đặt đồ ăn lên (Chef Work Position) - Dạng hình chữ nhật
            Vector3 putOnCenter = _putOnPos.transform.position + _putOnPosSize.y * 0.5f * Vector3.up; // Center của vị trí đặt đồ ăn được offset lên một nửa chiều cao
            Vector3 putOnSize = _putOnPosSize;
            Gizmos.DrawWireCube(putOnCenter, putOnSize);

            string label = "PutOn-Chef";
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(_putOnPos.transform.position + Vector3.up * 0.5f, label);

            // 2. Vẽ vị trí lấy đồ ăn (Waiter Work Position) - Hình vuông "cao cao" như bạn muốn
            Gizmos.color = Color.yellow;
            Vector3 pickUpPos = _pickUpPos.position;
            Vector3 center = new Vector3(pickUpPos.x, pickUpPos.y + _pickUpPosSize.y * 0.5f, pickUpPos.z);
            Gizmos.DrawWireCube(center, _pickUpPosSize);

            // 4. Hiển thị tên ghế và trạng thái
            label = "PickUp-Waiter";
            UnityEditor.Handles.color = Color.white;
            UnityEditor.Handles.Label(_pickUpPos.transform.position + Vector3.up * 0.5f, label);
        }


        #endregion
    }
}

using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestLogic : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _debugOrderText;

        [Header("Debug")]
        [SerializeField] 

        private Dictionary<CoreStation, int> _remainingDishes = new Dictionary<CoreStation, int>();
        private GuestContext _ctx;

        private void Awake()
        {
            _ctx = GetComponent<GuestContext>();
        }

        void OnEnable()
        {
            _remainingDishes.Clear();
        }

        #region API

        public void SetOrder(Order order)
        {
            _remainingDishes = order.OrderDict;
            UpdateOrderDetailText();
        }

        public void ReceiveFood(CookingTask cookingTask)
        {
            CoreStation receivedDish = cookingTask.CoreStation;
            if(_remainingDishes.ContainsKey(receivedDish))
            {
                _remainingDishes[receivedDish]--;
                if(_remainingDishes[receivedDish] <= 0)
                {
                    _remainingDishes.Remove(receivedDish);
                }
                UpdateOrderDetailText();
            }
            else
            {
                Debug.LogWarning($"Received an unexpected dish: {receivedDish}");
            }
        }

        #endregion

        private void UpdateOrderDetailText()
        {
            string orderDetails = "";
            foreach (var item in _remainingDishes)
            {
                orderDetails += $"{item.Key.Name}: {item.Value}\n";
            }
            _debugOrderText.text = orderDetails;
        }
    }
}
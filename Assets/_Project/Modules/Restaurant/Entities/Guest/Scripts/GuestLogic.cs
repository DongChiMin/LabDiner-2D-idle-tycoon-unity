
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestLogic : MonoBehaviour
    {
        private GuestContext _ctx;
        private Dictionary<CoreStation, int> _remainingDishes = new Dictionary<CoreStation, int>();

        void Awake()
        {
            _ctx = GetComponent<GuestContext>();
        }

        #region API

        public void SetOrder(Order order)
        {
            _remainingDishes = order.OrderDict;
            _ctx.OrderCanvas.UpdateOrderDetailText(_remainingDishes);
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

                if(_remainingDishes.Count == 0)
                {
                    _ctx.CtxBehavior.SetFoodReceivedEnough(true);
                }
                _ctx.OrderCanvas.UpdateOrderDetailText(_remainingDishes);
            }
            else
            {
                Debug.LogWarning($"Received an unexpected dish: {receivedDish}");
            }
        }

        #endregion
        
    }
}
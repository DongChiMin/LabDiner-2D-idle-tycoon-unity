using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Model;
using UnityEngine;

namespace LabDiner.Restaurant.Humanoid
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
            _ctx.OrderCanvas.Setup(_remainingDishes);
        }

        public void ReceiveFood(LabDiner.Restaurant.Workflow.CookingTask cookingTask)
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
                _ctx.OrderCanvas.DecreaseQuantity(receivedDish);
            }
            else
            {
                Debug.LogWarning($"Received an unexpected dish: {receivedDish}");
            }
        }

        #endregion
        
    }
}
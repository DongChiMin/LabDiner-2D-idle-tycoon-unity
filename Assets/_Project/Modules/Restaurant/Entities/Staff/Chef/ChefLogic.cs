
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class ChefLogic : MonoBehaviour
    {
        [SerializeField] private GameObject _carryDishObject;
        [SerializeField] private TextMeshProUGUI _priceText;
        [SerializeField] private SpriteRenderer _dishIcon;

        private double currentPrice;

        void OnEnable()
        {
            ToggleCarryDish(false);
        }

        #region API

        public void UpdateCookingTaskPrice(CookingTask task)
        {
            CoreStation station = task.CoreStation;
            task.Price = station.CurrentPrice;
            Debug.Log("TODO: hoàn thiện công thức tính giá tiền");
            currentPrice = task.Price;
        }

        public void CarryDish(CookingTask task)
        {
            _dishIcon.sprite = task.CoreStation.DishIcon;
            currentPrice = task.Price;
            
            ToggleCarryDish(true);
        }

        public void FinishTask(CookingTask task)
        {
            ToggleCarryDish(false);
        }

        #endregion

        private void ToggleCarryDish(bool isOn)
        {
            _carryDishObject.SetActive(isOn);
            _priceText.text = currentPrice.ToString();
        }
    } 
}
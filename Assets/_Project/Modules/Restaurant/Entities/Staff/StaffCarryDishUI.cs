using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StaffCarryDishUI : MonoBehaviour
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
            task.Profit = station.CurrentProfit;
            Debug.Log("TODO: hoàn thiện công thức tính giá tiền");
            currentPrice = task.Profit;
        }

        public void CarryDish(CookingTask task)
        {
            _dishIcon.sprite = task.CoreStation.DishIcon;
            currentPrice = task.Profit;

            ToggleCarryDish(true);
        }

        public void Finish(CookingTask task)
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


using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class WaiterLogic : MonoBehaviour
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
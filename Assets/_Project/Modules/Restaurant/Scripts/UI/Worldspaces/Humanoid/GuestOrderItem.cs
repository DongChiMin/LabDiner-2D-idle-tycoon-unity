using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class GuestOrderItem : MonoBehaviour
    {
        [SerializeField] private Image _dishIcon;
        [SerializeField] private TextMeshProUGUI _quantityText;
        private CoreStation _associatedDish;
        private int _quantity;

        public void Setup(CoreStation associatedDish, int quantity)
        {
            _associatedDish = associatedDish;
            _dishIcon.sprite = associatedDish.DishIcon;
            _quantity = quantity;
            _quantityText.text = quantity.ToString();
        }

        /// <summary>
        /// Giảm số lượng món ăn và cập nhật UI. Trả về true nếu vẫn còn món ăn, false nếu đã hết.
        /// </summary>
        /// <returns></returns>
        public bool DecreaseQuantity()
        {
            if (int.TryParse(_quantityText.text, out int currentQuantity))
            {
                currentQuantity = Mathf.Max(0, currentQuantity - 1);
                _quantityText.text = currentQuantity.ToString();
            }
            else
            {
                Debug.LogWarning("Failed to parse quantity text.");
            }
            return _quantity > 0;
        }
        
    }
}

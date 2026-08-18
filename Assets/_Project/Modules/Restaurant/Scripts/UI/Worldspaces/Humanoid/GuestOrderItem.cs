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
            _quantity = Mathf.Max(0, _quantity - 1);
            _quantityText.text = _quantity.ToString();
            return _quantity > 0;
        }
        
    }
}

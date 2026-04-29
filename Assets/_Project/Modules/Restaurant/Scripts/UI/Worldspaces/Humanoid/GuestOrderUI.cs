using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public class GuestOrderUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _debugOrderText;
        [SerializeField] private GuestOrderItem _orderItemPrefab;
        [SerializeField] private Transform _orderItemContainer;

        private Dictionary<CoreStation, GuestOrderItem> _currentOrder = new Dictionary<CoreStation, GuestOrderItem>();

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void Setup(Dictionary<CoreStation, int> remainingDishes)
        {
            _currentOrder.Clear();

            // Xóa các item cũ
            foreach (Transform child in _orderItemContainer)
            {
                Destroy(child.gameObject);
            }

            // Tạo item mới cho mỗi món ăn trong order
            foreach (var item in remainingDishes)
            {
                CoreStation dish = item.Key;
                int quantity = item.Value;

                GuestOrderItem newItem = Instantiate(_orderItemPrefab, _orderItemContainer);
                newItem.Setup(dish, quantity);

                _currentOrder[dish] = newItem;
            }
        }

        public void DecreaseQuantity(CoreStation dish)
        {
            GuestOrderItem itemToDecrease = null;
            // Tìm item tương ứng với món ăn đã nhận
            foreach (var item in _currentOrder)
            {
                if (item.Key != null && item.Key == dish)
                {
                    itemToDecrease = item.Value;
                    break;
                }
            }

            // Nếu tìm thấy item, giảm số lượng và cập nhật UI
            if (itemToDecrease != null)
            {
                bool hasQuantityLeft = itemToDecrease.DecreaseQuantity();
                if (!hasQuantityLeft)
                {
                    itemToDecrease.gameObject.SetActive(false);
                }
            }
            else
            {
                Debug.LogWarning("Không tìm thấy item tương ứng với món ăn đã nhận!");
            }


        }
    }
}

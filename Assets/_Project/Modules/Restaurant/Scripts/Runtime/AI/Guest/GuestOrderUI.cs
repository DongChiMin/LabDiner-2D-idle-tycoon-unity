using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestOrderUI : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private TextMeshProUGUI _debugOrderText;

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void UpdateOrderDetailText(Dictionary<CoreStation, int> remainingDishes)
        {
            string orderDetails = "";
            foreach (var item in remainingDishes)
            {
                orderDetails += $"{item.Key.Name}: {item.Value}\n";
            }
            _debugOrderText.text = orderDetails;
        }
    }
}

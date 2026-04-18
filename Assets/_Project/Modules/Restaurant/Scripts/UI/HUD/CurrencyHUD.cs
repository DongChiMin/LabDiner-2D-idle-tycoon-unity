using LabDiner.Shared;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CurrencyHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _gemText;
        
        [SerializeField] private LevelCoinEvent _onCoinUpdated;

        void OnEnable()
        {
            _onCoinUpdated.Register(UpdateCoinUI);
        }

        void OnDisable()
        {
            _onCoinUpdated.Unregister(UpdateCoinUI);
        }

        private void UpdateCoinUI(double newCoinAmount)
        {
            string formattedString = CurrencyFormatter.Format(newCoinAmount);
            _coinText.text = formattedString;
        }
    }
}

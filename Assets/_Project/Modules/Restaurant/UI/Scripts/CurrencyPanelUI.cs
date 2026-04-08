using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CurrencyPanelUI : MonoBehaviour
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
            _coinText.text = newCoinAmount.ToString("0");
        }
    }
}

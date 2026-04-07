using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public struct CoreStationUIData
    {
        public int CurrentLevel;
        public string Name;

        public int StarQuantity;
        public float StarProgress;

        public double CurrentProfit;
        public double CurrentCost;
        public float CurrentProcessTime;
    }
    public class CoreStationUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _nameText;
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private List<Image> _starImages;
        [SerializeField] private Image _starProgressFill;
        [SerializeField] private TextMeshProUGUI _profitText;
        [SerializeField] private TextMeshProUGUI _processTimeText;
        [SerializeField] private TextMeshProUGUI _costText;
        
        #region API
        public void Setup(CoreStationUIData data)
        {
            _nameText.text = data.Name;
            _levelText.text = $"Lvl {data.CurrentLevel}";
            _profitText.text = $"{data.CurrentProfit:F0}";
            _processTimeText.text = $"{data.CurrentProcessTime:F1}";
            _costText.text = $"${data.CurrentCost:F0}";

            for (int i = 0; i < _starImages.Count; i++)
            {
                if (i < data.StarQuantity)
                {
                    _starImages[i].color = Color.blue; // Star đã đạt được
                }
                else
                {
                    _starImages[i].color = Color.gray;   // Star chưa đạt được
                }
            }
            _starProgressFill.fillAmount = data.StarProgress;
        }

        #endregion
    }
}

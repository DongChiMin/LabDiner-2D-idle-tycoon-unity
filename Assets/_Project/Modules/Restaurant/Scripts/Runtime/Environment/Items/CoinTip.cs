using System;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Enum;
using LabDiner.Shared.Extension;
using LabDiner.Shared.Input;
using LabDiner.Shared.SO;
using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    [System.Serializable]
    public class CoinTip : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshProUGUI _tipAmountText;
        [SerializeField] private DoubleRuntimeSO _coinData;
        [SerializeField] private DoubleEvent _onTipReceived;

        [Header("Effect")]
        [SerializeField] private VerticalSwingEffect _swingEffect;
        [SerializeField] private double _tipAmount;

        void OnEnable()
        {
            _swingEffect.Show();
            _tipAmountText.text = CurrencyFormatter.Format(_tipAmount);
        }

        void OnDisable()
        {
            _swingEffect.Hide();
        }

        public void OnInteract()
        {
            _coinData.Add(_tipAmount);
            
            PoolContext.Instance.CurrencyBurstPool.SpawnBurstEffect(CurrencyType.Coin, transform.position, _tipAmount, true);
            PoolContext.Instance.CoinTipPool.ReturnToPool(this);
            _onTipReceived?.Raise(_tipAmount);
        }

        public bool CanInteract()
        {
            return true;
        }

        public void SetTipAmount(double amount)
        {
            _tipAmount = amount;
            _tipAmountText.text = amount.ToString("F0");
        }
    }
}
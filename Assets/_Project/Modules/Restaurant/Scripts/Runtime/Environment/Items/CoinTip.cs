using System;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Pooling;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Input;
using LabDiner.Shared.SO;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    [System.Serializable]
    public class CoinTip : MonoBehaviour, IInteractable
    {
        [SerializeField] private TextMeshProUGUI _tipAmountText;
        [SerializeField] private DoubleRuntimeSO _coinData;
        private double _tipAmount;

        public void OnInteract()
        {
            _coinData.Add(_tipAmount);
            PoolContext.Instance.CoinTipPool.ReturnToPool(this);
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
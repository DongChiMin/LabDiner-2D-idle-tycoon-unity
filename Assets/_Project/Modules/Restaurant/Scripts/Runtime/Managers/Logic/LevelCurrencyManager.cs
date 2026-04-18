
using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelCurrencyManager : MonoBehaviour
    {
        public double CurrentCoin => _currentCoin;

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinAdded;
        [SerializeField] private LevelCoinEvent _onCoinSpent;
        [SerializeField] private LevelCoinEvent _onCoinUpdated;

        [Header("[DEBUG]")]

        [Tooltip("Có thể nhập đơn vị a, b, c để test nhanh số tiền lớn")]
        [SerializeField] private string _cheatCoin = "5.23a";
        [ReadOnly] [SerializeField] private double _currentCoin;

        void Start()
        {
            _onCoinUpdated.Raise(_currentCoin);
        }

        void OnEnable()
        {
            _onCoinAdded.Register(AddCoin);
            _onCoinSpent.Register(SpendCoin);
        }

        void OnDisable()
        {
            _onCoinAdded.Unregister(AddCoin);
            _onCoinSpent.Unregister(SpendCoin);
        }

        private void AddCoin(double amount)
        {
            _currentCoin += amount;
            _onCoinUpdated.Raise(_currentCoin);
        }

        private void SpendCoin(double amount)
        {
            _currentCoin -= amount;
            _onCoinUpdated.Raise(_currentCoin);
        }

        void OnValidate()
        {
            if(!string.IsNullOrEmpty(_cheatCoin))
            {
                double parsedValue = CurrencyFormatter.Format(_cheatCoin);
                _currentCoin = parsedValue;
            }
            _onCoinUpdated.Raise(_currentCoin);
        }
    }
}
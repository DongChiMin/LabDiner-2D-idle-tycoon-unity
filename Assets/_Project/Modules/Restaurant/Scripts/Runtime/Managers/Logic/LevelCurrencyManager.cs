using LabDiner.Restaurant.Pooling;
using LabDiner.Shared.Enum;
using LabDiner.Shared.Event;
using LabDiner.Shared.Extension;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.Manager
{
    public class LevelCurrencyManager : MonoBehaviour
    {
        public double CurrentCoin => _currentCoin;

        [Header("Coin Events")]
        [SerializeField] private LevelCoinEvent _onCoinAdded;
        [SerializeField] private LevelCoinEvent _onCoinSpent;
        [SerializeField] private LevelCoinEvent _onCoinUpdated;
        [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;

        [Header("Gem Events")]
        [SerializeField] private LevelGemEvent _onGemAdded;
        [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;
        [SerializeField] private LevelGemEvent _onGemSpent;
        [SerializeField] private LevelGemEvent _onGemUpdated;

        [Header("[DEBUG]")]

        [Tooltip("Có thể nhập đơn vị a, b, c để test nhanh số tiền lớn")]
        [SerializeField] private string _cheatCoin = "5.23a";
        [ReadOnly] [SerializeField] private double _currentCoin;
        [ReadOnly] [SerializeField] private int _currentGem;

        void Start()
        {
            _onCoinUpdated.Raise(_currentCoin);
        }

        void OnEnable()
        {
            _onCoinAdded.Register(AddCoin);
            _onCoinSpent.Register(SpendCoin);
            _onCoinFlyAdded.Register(HandleCoinFlyAdded);

            _onGemAdded.Register(AddGem);
            _onGemFlyAdded.Register(HandleGemFlyAdded);
        }

        void OnDisable()
        {
            _onCoinAdded.Unregister(AddCoin);
            _onCoinSpent.Unregister(SpendCoin);
            _onCoinFlyAdded.Unregister(HandleCoinFlyAdded);

            _onGemAdded.Unregister(AddGem);
            _onGemFlyAdded.Unregister(HandleGemFlyAdded);
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

        private void AddGem(int amount)
        {
            _currentGem += amount;
            _onGemUpdated.Raise(_currentGem);
        }

        private void HandleGemFlyAdded(GemRewardData data)
        {
            _currentGem += data.RewardValue;
        }

        private void HandleCoinFlyAdded(CoinRewardData data)
        {
            _currentCoin += data.RewardValue;
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
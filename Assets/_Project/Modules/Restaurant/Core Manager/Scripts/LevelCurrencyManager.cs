
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelCurrencyManager : MonoBehaviour
    {
        public double CurrentCoin => _currentCoin;

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinAdded;
        [SerializeField] private LevelCoinEvent _onCoinUpdated;

        [Header("[DEBUG]")]
        [SerializeField] private double _currentCoin;

        void OnEnable()
        {
            _onCoinAdded.Register(AddCoin);
        }

        void OnDisable()
        {
            _onCoinAdded.Unregister(AddCoin);
        }

        private void AddCoin(double amount)
        {
            _currentCoin += amount;
            _onCoinUpdated.Raise(_currentCoin);
        }
    }
}
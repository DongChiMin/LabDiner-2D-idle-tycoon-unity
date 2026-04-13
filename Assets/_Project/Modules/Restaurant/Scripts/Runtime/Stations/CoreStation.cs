
using System.Collections.Generic;
using LabDiner.Shared.Input;
using log4net.Core;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class CoreStation : MonoBehaviour, IInteractable
    {
        public bool IsUnlocked => _currentLevel >= 0;
        public List<Station> Stations => _stations;
        public string Name => _name;
        public Sprite DishIcon => _dishIcon;
        public double CurrentProfit => _currentProfit;

        [Header("Static Attributes ")]
        [SerializeField] private string _name = "New CoreStation";
        [SerializeField] private Sprite _dishIcon;
        [SerializeField] private int _maxStar = 5;
        [SerializeField] private int _levelPerStar = 2;
        [SerializeField] private int _baseUpgradeCost;  // Chi phí nâng cấp cơ bản cho trạm chính
        [SerializeField] private int _upgradeCostMultiplier;    // Hệ số nhân cho chi phí nâng cấp (ví dụ: 1.5 sẽ làm tăng chi phí mỗi lần nâng cấp)
        [SerializeField] private List<Station> _stations = new List<Station>();

        [Header("Dynamic Attributes [DEBUG]")]
        [SerializeField] private double _currentProfit;
        [SerializeField] private double _currentCost;
        [SerializeField] private float _currentProcessTime;
        [SerializeField] private int _currentStar = 0;
        [SerializeField] private int _currentLevel = -1;

        [Header("Logic")]
        [SerializeField] private CoreStationUI _CoreStationUI;
        [SerializeField] private CoreStationStarUI _CoreStationStarUI;

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinSpent;

        private int MaxLevel => _maxStar * _levelPerStar;

        void OnEnable()
        {
            _CoreStationUI.OnUpgradeButtonClicked += Upgrade;
        }

        void OnDisable()
        {
            _CoreStationUI.OnUpgradeButtonClicked -= Upgrade;
        }

        #region API

        public void Upgrade()
        {
            _onCoinSpent?.Raise(_currentCost);

            _currentLevel++;
            _currentCost = _currentLevel * 100;
            _currentProfit += 100;
            _currentProcessTime -= 0.1f;
            _currentStar = Mathf.Min(_currentLevel / _levelPerStar, _maxStar);   // Ví dụ: cứ mỗi _levelPerStar cấp độ sẽ được 1 sao, tối đa _maxStar sao

            if(_currentLevel >= MaxLevel)
            {
                _CoreStationUI.MaxLevelReached();
                _CoreStationStarUI.Setup(_maxStar, _maxStar);
                return;
            }
            
            CoreStationUIData data = new CoreStationUIData()
            {
                CurrentLevel = _currentLevel,
                Name = _name,

                StarQuantity = _currentStar,
                StarProgress = (_currentLevel == 1) ? 0 : (_currentLevel % _levelPerStar) / (float)_levelPerStar,
                CurrentProfit = _currentProfit,
                CurrentCost = _currentCost,
                CurrentProcessTime = _currentProcessTime,
            };
            _CoreStationStarUI.Setup(_currentStar, _maxStar);
            _CoreStationUI.Setup(data);
        }

        public void OnInteract()
        {
            Debug.Log("TODO: mở UI nâng cấp trạm chính tại đây");
            CoreStationUIData data = new CoreStationUIData()
            {
                CurrentLevel = _currentLevel,
                Name = _name,

                StarQuantity = _currentStar,
                StarProgress = (_currentLevel == 1) ? 0 : (_currentLevel % _levelPerStar) / (float)_levelPerStar,

                CurrentProfit = _currentProfit,
                CurrentCost = _currentCost,
                CurrentProcessTime = _currentProcessTime,
            };
            _CoreStationUI.Setup(data);
            _CoreStationStarUI.Setup(_currentStar, _maxStar);
            _CoreStationUI.Show();
            
        }

        public bool CanInteract()
        {
            return true;
        }

        #endregion
    }
}
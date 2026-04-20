using System;
using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.UI;
using LabDiner.Shared.Event;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    [System.Serializable]
    public class CoreStation : MonoBehaviour
    {
        // API
        public CoreStationSO CoreStationSO => _coreStationSO;
        public int CurrentLevel => _currentLevel;
        public bool IsUnlocked => _currentLevel >= 1;
        public float RawProcessTime => _currentProcessTime / (1 + _currentProcessTimeBuff);
        public List<Station> Stations => _stations;
        public string Name => _name;
        public Sprite DishIcon => _dishIcon;
        public double CurrentProfit => _currentProfit;

        // Events
        public Action OnDataChanged;
        public Action<int> OnMaxLevel;  // Truyền vào số sao tối đa khi đạt max level

        [Header("References")]
        [SerializeField] private CoreStationUIController _CoreStationUIController;
        [SerializeField] private StationSpawner _stationSpawner;
        [SerializeField] private PopScaleEffect _upgradeSprite;
        [SerializeField] private Transform gemRewardStartPos;   // Điểm xuất hiện hiệu ứng gem khi nhận thưởng từ star upgrade

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinSpent;
        [SerializeField] private LevelCoinEvent _onCoinUpdated;
        [SerializeField] private CoreStationEvent _onCoreStationLevelUpgraded;

        [Header("Data")]
        [SerializeField] private CoreStationSO _coreStationSO;

        [Header("[DEBUG] Static Attributes")]
        [SerializeField] private string _name = "New CoreStation";
        [SerializeField] private Sprite _dishIcon;
        [SerializeField] private int _maxStar = 5;
        [SerializeField] private int _levelPerStar = 2;

        [SerializeField] private double _baseProfit;  // Lợi nhuận cơ bản của trạm chính
        [SerializeField] private float _profitMultiplier;   

        [SerializeField] private double _baseUpgradeCost;  // Chi phí nâng cấp cơ bản cho trạm chính
        [SerializeField] private float _upgradeCostMultiplier;    // Hệ số nhân cho chi phí nâng cấp (ví dụ: 1.5 sẽ làm tăng chi phí mỗi lần nâng cấp)
        [SerializeField] private List<Station> _stations = new List<Station>();

        [Header("[DEBUG] Dynamic Attributes")]
        [SerializeField] private double _currentProfit;
        [SerializeField] private float _currentProfitBuff = 0;
        [SerializeField] private double _currentCost;
        [SerializeField] private float _currentProcessTime;
        [SerializeField] private float _currentProcessTimeBuff = 0;
        [SerializeField] private int _currentStar = 0;
        [SerializeField] private int _currentLevel = 0;

        void OnEnable()
        {
            _onCoinUpdated.Register(HandleCoinUpdated);
            foreach(var station in _stations)
            {
                station.OnClickStation += HandleStationClick;
            }
        }

        void OnDisable()
        {
            _onCoinUpdated.Unregister(HandleCoinUpdated);
            foreach(var station in _stations)
            {
                station.OnClickStation -= HandleStationClick;
            }
        }

        void Start()
        {
            Initialize();
            OnDataChanged?.Invoke();
            
            #if UNITY_EDITOR
            ValidateData();
            #endif
        }


        #region API

        /// <summary>
        /// Nâng cấp trạm chính:
        /// - Tăng cấp độ của trạm chính lên 1.
        /// - Tính toán lại chi phí nâng cấp tiếp theo dựa trên công thức: chi phí nâng cấp tiếp theo = chi phí nâng cấp cơ bản * (hệ số nhân ^ (cấp độ hiện tại * 2)). Ví dụ: nếu cấp độ hiện tại là 3, hệ số nhân là 1.5, và chi phí nâng cấp cơ bản là 100, thì chi phí nâng cấp tiếp theo sẽ là 100 * (1.5 ^ (3 * 2)) = 100 * (1.5 ^ 6) = 100 * 11.39 = 1139.
        /// - Tính toán lại lợi nhuận hiện tại dựa trên công thức: lợi nhuận hiện tại = lợi nhuận cơ bản * (hệ số nhân lợi nhuận ^ (cấp độ hiện tại - 1)). Ví dụ: nếu cấp độ hiện tại là 3, hệ số nhân lợi nhuận là 2, và lợi nhuận cơ bản là 10, thì lợi nhuận hiện tại sẽ là 10 * (2 ^ (3 - 1)) = 10 * (2 ^ 2) = 10 * 4 = 40.
        /// - Tính toán lại số sao hiện tại dựa trên công thức: số sao hiện tại = cấp độ hiện tại / cấp độ mỗi sao, và giới hạn tối đa bằng số sao tối đa. Ví dụ: nếu cấp độ hiện tại là 3, cấp độ mỗi sao là 2, và số sao tối đa là 5, thì số sao hiện tại sẽ là min(3 / 2, 5) = min(1.5, 5) = 1.
        /// - Nếu cấp độ hiện tại đạt đến cấp độ tối đa, hiển thị thông báo "Max Level Reached" trên UI và thiết lập UI sao với số sao tối đa.
        /// </summary>
        public void Upgrade()
        {
            //Sau khi tính toán giá trị mới thì mới trừ tiền để đảm bảo giá trị hiển thị trên UI là chính xác nhất
            _onCoinSpent?.Raise(_currentCost);

            _currentLevel++;

            // Tính toán lại profit tiếp theo
            double rawProfit = _baseProfit * (_currentProfitBuff + 1) * Mathf.Pow(_profitMultiplier, _currentLevel - 1);
            _currentProfit = Math.Floor(rawProfit);

            // Tính toán lại chi phí nâng cấp tiếp theo
            double rawCost = _baseUpgradeCost * Mathf.Pow(_upgradeCostMultiplier, _currentLevel * 2);
            _currentCost = Math.Floor(rawCost);

            //Kiểm tra đã qua sao mới chưa
            int newStar = _currentLevel / _levelPerStar;
            bool isNewStar = newStar > _currentStar;
            _currentStar = Mathf.Min(newStar, _maxStar);

            // Kiểm tra nếu là level 1: spawn station
            if(_currentLevel == 1)
            {
                CreateNewStation(1);
            }
            
            //Nếu đã qua sao mới
            // - chạy reward (thưởng gem, ...)
            // - chạy effect (effect x2 profit, effect tạo station mới, ...)
            if(isNewStar && _currentStar <= _maxStar)
            {
                ProcessStarUpgrade(newStar);
                // Sau khi có Buff mới từ Star, tính lại Profit một lần nữa để UI nhận số mới nhất
                _currentProfit = Math.Floor(rawProfit * (1 + _currentProfitBuff));
            }

            // Kiểm tra nếu đạt cấp độ tối đa
            int maxLevel = _maxStar * _levelPerStar;
            if(_currentLevel >= maxLevel)
            {
                OnMaxLevel?.Invoke(_maxStar);
                _onCoreStationLevelUpgraded.Raise(this);
                return;
            }

            // Sau khi cập nhật cost, cập nhật lại coin để xem có toggle attention upgrade hay không
            _onCoinSpent.Raise(0);
            _onCoreStationLevelUpgraded.Raise(this);

            OnDataChanged?.Invoke();
        }

        public CoreStationUIData GetUIData()
        {
            // Lấy số coin hiện tại của người chơi để so sánh với chi phí nâng cấp
            double currentCoin = LevelManagerContext.Instance.LevelCurrencyManager.CurrentCoin;

            // Tính toán phần thưởng sao dựa trên số sao hiện tại và dữ liệu từ CoreStationSO
            StationStarSO currentStarData = _coreStationSO.StationStars[Mathf.Min(_currentStar, _coreStationSO.StationStars.Count - 1)];
            List<RewardData> rewardDatas = currentStarData.rewards;
            List<Sprite> starRewardIcons = new List<Sprite>();
            foreach(var reward in rewardDatas)
            {
                starRewardIcons.Add(reward.Icon);
            }

            // Tạo struct dữ liệu để truyền vào UI
            CoreStationUIData data = new CoreStationUIData()
            {
                CurrentLevel = _currentLevel,
                Name = _name,

                StarQuantity = _currentStar,
                MaxStar = _maxStar, 
                StarProgress = (_currentLevel == 1) ? 0 : (_currentLevel % _levelPerStar) / (float)_levelPerStar,
                StarRewardIcons = starRewardIcons,

                CurrentProfit = _currentProfit,
                CurrentCost = _currentCost,
                CurrentProcessTime = _currentProcessTime,
                CanUpgrade = currentCoin >= _currentCost
            };
            return data;
        }

        public bool VerifyData(DishSO dishSO)
        {
            return _coreStationSO.Dish == dishSO;
        }

        public void UpgradeProfit(float value)
        {
            _currentProfitBuff += value;
            double rawProfit = _baseProfit * Mathf.Pow(_profitMultiplier, _currentLevel - 1);
            _currentProfit = _currentProfit = Math.Floor(rawProfit * (1+ _currentProfitBuff));
            OnDataChanged?.Invoke();
        }

        public void UpgradeProcessTime(float value)
        {
            _currentProcessTimeBuff += value;
            _currentProcessTime = _coreStationSO.BaseProcessTime / (1 + _currentProcessTimeBuff);
            OnDataChanged?.Invoke();
        }

        #endregion

        #region Private Methods 
        private void Initialize()
        {
            if(_coreStationSO == null)
            {
                Debug.LogError("CoreStationSO is not assigned in the inspector.");
                return;
            }

            _name = _coreStationSO.Dish.name;
            _dishIcon = _coreStationSO.Dish.Icon;
            _maxStar = _coreStationSO.StationStars.Count;
            _levelPerStar = _coreStationSO.LevelPerStar;

            _baseProfit = _coreStationSO.BaseProfit;
            _profitMultiplier = _coreStationSO.ProfitMultiplier;
            _baseUpgradeCost = _coreStationSO.BaseUpgradeCost;
            _upgradeCostMultiplier = _coreStationSO.UpgradeCostMultiplier;


            _currentProfit = _baseProfit;
            _currentProfitBuff = 0;
            _currentProcessTimeBuff = 0;
            _currentCost = _baseUpgradeCost;
            _currentProcessTime = _coreStationSO.BaseProcessTime;
            _currentStar = 0;
            _currentLevel = 0;
        }

        private void CreateNewStation(float quantity)
        {
            for(int i = 0; i < quantity; i++)
            {
                Station newStation = _stationSpawner.RequestSpawn();
                _stations.Add(newStation);
                newStation.OnClickStation += HandleStationClick;
            }
        }

        private void HandleCoinUpdated(double currentCoin)
        {
            bool canUpgrade = currentCoin >= _currentCost;
            if(canUpgrade && !_upgradeSprite.gameObject.activeSelf)
            {
                _upgradeSprite.gameObject.SetActive(true);
                _upgradeSprite.Show();
            }
            else if(!canUpgrade && _upgradeSprite.gameObject.activeSelf)
            {
                _upgradeSprite.Hide(() => _upgradeSprite.gameObject.SetActive(false));
            }
        }

        private void HandleStationClick()
         {
             if (!_CoreStationUIController.CanInteract())
             {
                 return;
             }

             _CoreStationUIController.OnInteract();
         }

         private void ProcessStarUpgrade(int newStar)
         {
            StationStarSO starData = _coreStationSO.StationStars[Mathf.Min(newStar - 1, _coreStationSO.StationStars.Count - 1)];
            Vector3 screenPoint = Camera.main.WorldToScreenPoint(gemRewardStartPos.position);
            screenPoint.z = 0;
            Vector3 startPos = screenPoint;
            starData.GiveRewards(startPos);

            foreach(var buff in starData.Buffs)
            {
                switch(buff.EffectType)
                {
                    case StationStarBuffType.MultiplyProfit:
                        _currentProfitBuff += buff.Value; 
                        _CoreStationUIController.ShowUpgradeEffect($"Profit x{buff.Value:F1}");
                        break;
                    case StationStarBuffType.CreateNewStation:
                        CreateNewStation(buff.Value);
                        _CoreStationUIController.ShowUpgradeEffect($"New Station(s)!");
                        break;
                }
            }
         }
        #endregion


        #region EDITOR ONLY

        private CoreStationSO _lastCoreStationSO;

        void OnValidate()
        {
            if (_coreStationSO != _lastCoreStationSO)
            {
                if (_coreStationSO != null)
                {
                    Initialize();
                }
                
                // Cập nhật lại biến tạm để không chạy lại lần sau
                _lastCoreStationSO = _coreStationSO;
            }
            OnDataChanged?.Invoke();
        }

        void ValidateData()
        {
            int spawnPointCount = _stationSpawner.SpawnPoints.Count;
            int maxStationQuantity = 1;
            List<StationStarSO> stars = _coreStationSO.StationStars;
            foreach(var star in stars)
            {
                foreach(var buff in star.Buffs)
                {
                    if(buff.EffectType == StationStarBuffType.CreateNewStation)
                    {
                        maxStationQuantity += (int)buff.Value;
                    }
                }
            }

            if(maxStationQuantity > spawnPointCount)
            {
                Debug.LogError($"CoreStation '{_name}' cần {maxStationQuantity} điểm spawn, nhưng chỉ có {spawnPointCount} điểm spawn khả dụng trong StationSpawner.");
            }
        }

        #endregion        
    }
}
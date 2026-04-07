
using System.Collections.Generic;
using LabDiner.Shared.Input;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class CoreStation : MonoBehaviour, IInteractable
    {
        public bool isUnlocked => _currentLevel >= 0;
        public List<Station> Stations => _stations;
        public string Name => _name;
        public Sprite DishIcon => _dishIcon;
        public double CurrentProfit => _currentProfit;

        [Header("Static Attributes ")]
        [SerializeField] private string _name = "New CoreStation";
        [SerializeField] private Sprite _dishIcon;
        [SerializeField] private int _baseUpgradeCost;  // Chi phí nâng cấp cơ bản cho trạm chính
        [SerializeField] private int _upgradeCostMultiplier;    // Hệ số nhân cho chi phí nâng cấp (ví dụ: 1.5 sẽ làm tăng chi phí mỗi lần nâng cấp)
        [SerializeField] private List<Station> _stations = new List<Station>();

        [Header("Dynamic Attributes")]
        [SerializeField] private double _currentProfit;
        [SerializeField] private double _currentCost;
        [SerializeField] private float _currentProcessTime;
        [SerializeField] private int _currentStar = 0;
        [SerializeField] private int _currentLevel = -1;

        [Header("Logic")]
        [SerializeField] private CoreStationUI _CoreStationUI;

        #region API

        public void OnInteract()
        {
            CoreStationUIData data = new CoreStationUIData()
            {
                CurrentLevel = _currentLevel,
                Name = _name,

                StarQuantity = _currentStar,
                StarProgress = 0.3f,   // TODO: tính toán phần trăm tiến độ sao dựa trên doanh thu hiện tại và mục tiêu doanh thu của từng sao

                CurrentProfit = _currentProfit,
                CurrentCost = _currentCost,
                CurrentProcessTime = _currentProcessTime,
            };
            _CoreStationUI.Setup(data);
            _CoreStationUI.gameObject.SetActive(true);
        }

        public bool CanInteract()
        {
            return true;
        }

        #endregion
    }
}
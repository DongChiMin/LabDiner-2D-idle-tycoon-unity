
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class CoreStation : MonoBehaviour
    {
        public bool isUnlocked => _currentLevel >= 0;
        public List<Station> Stations => _stations;
        public string Name => _name;
        public Sprite DishIcon => _dishIcon;
        public double CurrentPrice => currentPrice;

        [Header("Economic Settings")]
        [SerializeField] private int _baseUpgradeCost;  // Chi phí nâng cấp cơ bản cho trạm chính
        [SerializeField] private int _upgradeCostMultiplier;    // Hệ số nhân cho chi phí nâng cấp (ví dụ: 1.5 sẽ làm tăng chi phí mỗi lần nâng cấp)

        [Header("Object Settings")]
        [SerializeField] private string _name = "New CoreStation";
        [SerializeField] private Sprite _dishIcon;
        [SerializeField] private double currentPrice;
        [SerializeField] private int _currentLevel = -1;
        [SerializeField] private List<Station> _stations = new List<Station>();

        
    }
}

using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class CoreStation : MonoBehaviour
    {
        [Header("Economic Settings")]
        [SerializeField] private int _baseUpgradeCost;  // Chi phí nâng cấp cơ bản cho trạm chính
        [SerializeField] private int _upgradeCostMultiplier;    // Hệ số nhân cho chi phí nâng cấp (ví dụ: 1.5 sẽ làm tăng chi phí mỗi lần nâng cấp)

        [Header("Object Settings")]
        [SerializeField] private int currentLevel = -1;

        public bool isUnlocked => currentLevel >= 0;
    }
}
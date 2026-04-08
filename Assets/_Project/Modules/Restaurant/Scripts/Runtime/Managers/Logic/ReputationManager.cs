
using System;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
public class ReputationMilestone
{
    public string Name;
    public int Threshold;
    public float BonusCookingSpeed; // Ví dụ: +0.1f (tăng 10%)
    public float BonusTipChance;    // Ví dụ: +0.05f (tăng 5% tỉ lệ tip)
}

    public class ReputationManager : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private GuestEvent _onGuestHappy;
        [SerializeField] private GuestEvent _onGuestLeaveAngry;
        [SerializeField] private ReputationEvent _onReputationChanged;

        [Header("Settings")]
        [SerializeField] private List<ReputationMilestone> _milestones;
        [SerializeField] private int _maxReputation = 100;
        [SerializeField] private int _happyReputationGain = 10;
        [SerializeField] private int _angryReputationLoss = 15;

        [Header("[DEBUG]")]
        [SerializeField] private int _currentReputation = 0;

        void OnEnable()
        {
            _onGuestHappy.Register(HandleGuestHappy);
            _onGuestLeaveAngry.Register(HandleGuestAngry);
        }

        void OnDisable()
        {
            _onGuestHappy.Unregister(HandleGuestHappy);
            _onGuestLeaveAngry.Unregister(HandleGuestAngry);
        }

        #region API

        public void AddReputation(int amount)
        {
            _currentReputation = Mathf.Max(0, _currentReputation + amount);
            _onReputationChanged?.Raise(_currentReputation / (float)_maxReputation);
            RecalculateActiveBuffs();
        }

        #endregion

        private void HandleGuestHappy(GuestContext guest)
        {
            AddReputation(_happyReputationGain);
        }

        private void HandleGuestAngry(GuestContext guest)
        {
            AddReputation(-_angryReputationLoss);
        }

        private void RecalculateActiveBuffs()
        {
            float newSpeedBonus = 0f;
            float newTipBonus = 0f;

            // Duyệt qua tất cả các mốc, cái nào vượt qua thì cộng dồn vào
            foreach (var milestone in _milestones)
            {
                if (_currentReputation >= milestone.Threshold)
                {
                    newSpeedBonus += milestone.BonusCookingSpeed;
                    newTipBonus += milestone.BonusTipChance;
                }
            }
        }
    }
}
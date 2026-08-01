using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.Workflow;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public enum TipMissionType
    {
        TipCount,      // Số lần nhận tip
        TipAmount,        // Tổng số tiền tip
    }

    /// <summary>
    /// Nhiệm vụ yêu cầu nâng cấp level của một core Station cụ thể lên một mốc nhất định nào đó (ví dụ: nâng cấp Core Station Burger lên level 10)
    /// </summary>
    [CreateAssetMenu(fileName = "New Tip Mission", menuName = "Game/Missions/Tip Mission")]
    public class TipMissionSO : BaseMissionSO
    {
        [Header("Target")]
        public TipMissionType MissionType;

        [Header("Static")]
        [SerializeField] private DoubleEvent _onTipReceived;
        private double _currentValue = 0;
        private int _currentCount = 0;

        void OnEnable()
        {
            if (_onTipReceived != null)
            {
                _onTipReceived.Register(HandleTipReceived);
            }
            OnMissionStart += HandleMissionStart;
        }

        void OnDisable()
        {
            if (_onTipReceived != null)
            {
                _onTipReceived.Unregister(HandleTipReceived);
            }
            OnMissionStart -= HandleMissionStart;
        }

        private void HandleValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public override float GetCurrentValue()
        {
            switch(MissionType)
            {
                case TipMissionType.TipCount:
                    return _currentCount;
                case TipMissionType.TipAmount:
                    return (float)_currentValue;
                default:
                    return 0;
            }
        }

        private void HandleTipReceived(double tipAmount)
        {
            _currentValue += tipAmount;
            _currentCount++;
            HandleValueChanged();
        }

        private void HandleMissionStart()
        {
            _currentValue = 0;
            _currentCount = 0;
        }
    }
}

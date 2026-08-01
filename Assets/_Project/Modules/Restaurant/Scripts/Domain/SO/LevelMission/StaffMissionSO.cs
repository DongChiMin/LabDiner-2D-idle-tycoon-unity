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
    /// <summary>
    /// Nhiệm vụ yêu cầu nâng cấp level của một core Station cụ thể lên một mốc nhất định nào đó (ví dụ: nâng cấp Core Station Burger lên level 10)
    /// </summary>
    [CreateAssetMenu(fileName = "New Staff Mission", menuName = "Game/Missions/Staff Mission")]
    public class StaffMissionSO : BaseMissionSO
    {
        [Header("Target")]
        public StaffType TargetStaff;

        [Header("Static")]
        [SerializeField] private StaffEvent _onStaffSpawn;
        [SerializeField] private StaffEvent _onStaffListClear;

        private Dictionary<StaffType, int> _staffCount = new Dictionary<StaffType, int>();

        void OnEnable()
        {
            if (_onStaffSpawn != null)
            {
                _onStaffSpawn.Register(HandleStaffSpawn);
            }
            if (_onStaffListClear != null)
            {
                _onStaffListClear.Register(HandleStaffListClear);
            }
        }

        void OnDisable()
        {
            if (_onStaffSpawn != null)
            {
                _onStaffSpawn.Unregister(HandleStaffSpawn);
            }
            if (_onStaffListClear != null)
            {
                _onStaffListClear.Unregister(HandleStaffListClear);
            }
        }

        private void HandleValueChanged()
        {
            OnValueChanged?.Invoke();
        }

        public override float GetCurrentValue()
        {
            if(TargetStaff == StaffType.All)
            {
                return _staffCount.Values.Sum();
            }
            if (_staffCount.ContainsKey(TargetStaff))
            {
                return _staffCount[TargetStaff];
            }
            return 0;
        }

        private void HandleStaffSpawn(Staff staff)
        {
            if (!_staffCount.ContainsKey(staff.StaffType))
            {
                _staffCount[staff.StaffType] = 0;
            }
            _staffCount[staff.StaffType]++;
            HandleValueChanged();
        }

        private void HandleStaffListClear(Staff staff)
        {
            _staffCount.Clear();
        }
    }
}

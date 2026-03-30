
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaiterContext : MonoBehaviour, IStaff
    {
        [Header("Settings")]
        [SerializeField] private OrderEvent _onOrderServed;
        [SerializeField] private Transform _restPosition;
        public Transform RestPosition => _restPosition;

        [Header("Components")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private WaiterBehavior _behavior;
        [SerializeField] private WaiterAI _ai;
        public StaffMover CtxMover => _mover;
        public WaiterBehavior CtxBehavior => _behavior;
        public WaiterAI CtxAI => _ai;
        private bool isAvailable = true;
        public bool IsAvailable => isAvailable;

        public void DoTask(IStaffTask task)
        {
            isAvailable = false;
            _ai.DoServe(task as Order);
        }

        public void OnTaskCompleted(IStaffTask task)
        {
            isAvailable = true;
            _onOrderServed.Raise(task as Order);
        }
    }
}
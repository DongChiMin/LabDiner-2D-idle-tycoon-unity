
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaiterContext : MonoBehaviour, IStaff
    {
        public StaffMover CtxMover => _mover;
        public WaiterBehavior CtxBehavior => _behavior;
        public WaiterAI CtxAI => _ai;

        [Header("Settings")]
        [SerializeField] private OrderEvent _onOrderServed;
        [SerializeField] private WaiterEvent _onWaiterAvailable;
        [SerializeField] private Transform _restPosition;
        public Transform RestPosition => _restPosition;

        [Header("Components")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private WaiterBehavior _behavior;
        [SerializeField] private WaiterAI _ai;

        [Header("[Debug]")]
        [SerializeField] private bool _isAvailable = true;
        public bool IsAvailable {
            get => _isAvailable; 
            set => _isAvailable = value;
        }

        public void DoTask(IStaffTask task)
        {
            IsAvailable = false;
            switch (task)
            {
                case Order order:
                    _ai.StartServe(order);
                    break;
                case CookingTask cookingTask:
                    _ai.StartShip(cookingTask);
                    break;
                default:
                    Debug.LogWarning("Waiter received an unsupported task: " + task);
                    IsAvailable = true; // Trả lại trạng thái sẵn sàng nếu không thể xử lý task
                    break;
            }
        }

        public void OnTaskCompleted(IStaffTask task)
        {
            IsAvailable = true;
            _onWaiterAvailable.Raise(this);
            switch (task)
            {
                case Order order:
                    _onOrderServed.Raise(order);
                    break;
                case CookingTask cookingTask:
                    break;
                default:
                    Debug.LogWarning("Waiter completed an unsupported task: " + task);
                    break;
            }
        }
    }
}
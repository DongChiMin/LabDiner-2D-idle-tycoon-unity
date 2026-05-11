using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.UI;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Humanoid
{
    public class WaiterContext : MonoBehaviour, IStaff
    {
        public Restaurant.Workflow.StaffMover CtxMover => _mover;
        public WaiterBehavior CtxBehavior => _behavior;
        public WaiterAI CtxAI => _ai;
        public StaffCarryDishUI CarryDishLogic => _carryDishLogic;
        public StaffProgressPieUI ProgressPieLogic => _progressPieLogic;
        public bool IsAvailable
        {
            get => _isAvailable;
            set => _isAvailable = value;
        }
        public Transform RestPosition
        {
            get => _currentRestPos;
            set => _currentRestPos = value;
        }


        [Header("Settings")]
        [SerializeField] private OrderEvent _onOrderServed;
        [SerializeField] private WaiterEvent _onWaiterAvailable;
        [SerializeField] private LevelCoinEvent _onCoinAdded;
        [SerializeField] private Transform _restPosition;

        [Header("Components")]
        [SerializeField] private Restaurant.Workflow.StaffMover _mover;
        [SerializeField] private WaiterBehavior _behavior;
        [SerializeField] private WaiterAI _ai;

        [Header("Visual Logics")]
        [SerializeField] private StaffCarryDishUI _carryDishLogic;
        [SerializeField] private StaffProgressPieUI _progressPieLogic;

        [Header("[Debug]")]
        [SerializeField] private Transform _currentRestPos;
        [SerializeField] private bool _isAvailable = true;

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
                    _onCoinAdded.Raise(cookingTask.Profit);
                    break;
                default:
                    Debug.LogWarning("Waiter completed an unsupported task: " + task);
                    break;
            }
        }

        public void UpgradeMoveSpeed(float speedBuffValue)
        {
            _mover.UpgradeMoveSpeed(speedBuffValue);
        }
    }
}
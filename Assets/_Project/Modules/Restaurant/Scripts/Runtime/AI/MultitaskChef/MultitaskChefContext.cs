using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class MultitaskChefContext : MonoBehaviour, IStaff
    {         
        public StaffMover CtxMover => _mover;
        public MultitaskChefBehavior CtxBehavior => _behavior;
        public MultitaskChefAI CtxAI => _ai;
        public StaffCarryDishUI CarryDishUI => _carryDishUI;
        public StaffProgressPieUI ProgressPieUI => _progressPieUI;
        public bool IsAvailable {
            get => _isAvailable; 
            set => _isAvailable = value;
        }
        public Transform RestPosition => _restPosition;

        [Header("Settings")]
        [SerializeField] private CookingTaskEvent _onCookingTaskComplete;
        [SerializeField] private OrderEvent _onOrderServed;
        [SerializeField] private MultitaskChefEvent _onMultitaskChefAvailable;
        [SerializeField] private LevelCoinEvent _onCoinAdded;
        [SerializeField] private Transform _restPosition;
        
        [Header("Components")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private MultitaskChefBehavior _behavior;
        [SerializeField] private MultitaskChefAI _ai;

        [Header("Visual Logics")]
        [SerializeField] private StaffCarryDishUI _carryDishUI;
        [SerializeField] private StaffProgressPieUI _progressPieUI;

        [Header("[Debug]")]
        [SerializeField] private bool _isAvailable = true;
        

        public void DoTask(IStaffTask task)
        {
            IsAvailable = false;
            switch(task)
            {
                case CookingTask cookingTask:
                    _ai.StartCookAndShip(cookingTask);
                    break;
                case Order order:
                    _ai.StartServe(order);
                    return;
                default:
                    Debug.LogError("Unsupported task type for MultitaskChef: " + task.GetType());
                    break;
            }
        }

        //Hoàn thành sau khi chef bê đồ ăn tới passTable
        public void OnTaskCompleted(IStaffTask task)
        {
            IsAvailable = true;
            _onMultitaskChefAvailable.Raise(this);
            switch(task)
            {
                case CookingTask cookingTask:
                    _onCookingTaskComplete.Raise(cookingTask);
                    break;
                case Order order:
                    _onOrderServed.Raise(order);
                    _onCoinAdded.Raise(order.Profit);
                    break;
                default:
                    Debug.LogError("Unsupported task type for MultitaskChef: " + task.GetType());
                    break;
            }
        }
    }
}

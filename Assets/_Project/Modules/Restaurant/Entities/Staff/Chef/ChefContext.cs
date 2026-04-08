using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class ChefContext : MonoBehaviour, IStaff
    { 
        public StaffMover CtxMover => _mover;
        public ChefBehavior CtxBehavior => _behavior;
        public ChefAI CtxAI => _ai;
        public StaffCarryDishUI CarryDishUI => _carryDishUI;
        public StaffProgressPieUI ProgressPieUI => _progressPieUI;

        [Header("Settings")]
        [SerializeField] private CookingTaskEvent _onCookingTaskComplete;
        [SerializeField] private ChefEvent _onChefAvailable;
        [SerializeField] private Transform _restPosition;
        public Transform RestPosition => _restPosition;

        [Header("Components")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private ChefBehavior _behavior;
        [SerializeField] private ChefAI _ai;

        [Header("Visual Logics")]
        [SerializeField] private StaffCarryDishUI _carryDishUI;
        [SerializeField] private StaffProgressPieUI _progressPieUI;

        [Header("[Debug]")]
        [SerializeField] private bool _isAvailable = true;
        public bool IsAvailable {
            get => _isAvailable; 
            set => _isAvailable = value;
        }

        public void DoTask(IStaffTask task)
        {
            IsAvailable = false;
            _ai.StartTask(task as CookingTask);
        }

        //Hoàn thành sau khi chef bê đồ ăn tới passTable
        public void OnTaskCompleted(IStaffTask task)
        {
            IsAvailable = true;
            _onChefAvailable.Raise(this);
            _onCookingTaskComplete.Raise(task as CookingTask);
        }
    }
}

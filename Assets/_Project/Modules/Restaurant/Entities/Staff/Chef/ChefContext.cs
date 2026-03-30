using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class ChefContext : MonoBehaviour, IStaff
    { 
        [Header("Settings")]
        [SerializeField] private CookingTaskEvent _onCookingTaskComplete;
        [SerializeField] private Transform _restPosition;
        public Transform RestPosition => _restPosition;

        [Header("Components")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private ChefBehavior _behavior;
        [SerializeField] private ChefAI _ai;
        public StaffMover CtxMover => _mover;
        public ChefBehavior CtxBehavior => _behavior;
        public ChefAI CtxAI => _ai;
        public bool IsAvailable { get ; set ; } = true;

        public void DoTask(IStaffTask task)
        {
            IsAvailable = false;
            _ai.StartTask(task as CookingTask);
        }

        //Hoàn thành sau khi chef bê đồ ăn tới passTable
        public void OnTaskCompleted(IStaffTask task)
        {
            IsAvailable = true;
            _onCookingTaskComplete.Raise(task as CookingTask);
        }
    }
}

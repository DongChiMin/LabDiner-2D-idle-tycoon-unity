using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class ChefContext : MonoBehaviour, IStaff
    { 
        [Header("Settings")]
        [SerializeField] private CookingTaskEvent _onOrderServed;
        [SerializeField] private Transform _restPosition;
        public Transform RestPosition => _restPosition;

        [Header("Components")]
        [SerializeField] private StaffMover _mover;
        [SerializeField] private ChefBehavior _behavior;
        [SerializeField] private ChefAI _ai;
        public StaffMover CtxMover => _mover;
        public ChefBehavior CtxBehavior => _behavior;
        public ChefAI CtxAI => _ai;
        private bool isAvailable = true;
        public bool IsAvailable => isAvailable;
        
        public void DoTask(IStaffTask task)
        {
             isAvailable = false;
            _ai.StartTask(task as CookingTask);
        }

        public void OnTaskCompleted(IStaffTask task)
        {
            isAvailable = true;
            _onOrderServed.Raise(task as CookingTask);
        }
    }
}

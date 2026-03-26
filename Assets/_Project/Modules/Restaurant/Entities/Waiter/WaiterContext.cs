
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaiterContext : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Transform _restPosition;
        public Transform RestPosition => _restPosition;

        [Header("Components")]
        [SerializeField] private WaiterMover _mover;
        [SerializeField] private WaiterBehavior _behavior;
        [SerializeField] private WaiterAI _ai;
        public WaiterMover CtxMover => _mover;
        public WaiterBehavior CtxBehavior => _behavior;
        public WaiterAI CtxAI => _ai;

        private bool isAvailable = true;

        public bool IsAvailable => isAvailable;

        public void DoTask(Order order)
        {
            // Implement task logic here
            isAvailable = false;
            _ai.DoServe(order);
        }
    }
}
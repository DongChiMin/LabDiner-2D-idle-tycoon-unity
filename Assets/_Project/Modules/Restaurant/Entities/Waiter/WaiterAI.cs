using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class WaiterAI : MonoBehaviour
    {
        [SerializeField] WaiterContext _context;
        [SerializeField] private WaiterEvent _onWaiterAvailable;
        private WaiterMover _mover;
        private WaiterBehavior _behavior;

        [Header("[DEBUG]")]
        [SerializeField] private Order servingOrder;

        void Awake()
        {
            _mover = _context.CtxMover;
            _behavior = _context.CtxBehavior;
        }

        void OnEnable()
        {
            servingOrder = null;
        }

        // // Đọc luồng logic ở đây như một câu chuyện
        public IEnumerator DoServeTask(Order order)
        {
            servingOrder = order;
            // 1. Đi đến bàn
            yield return _mover.MoveTo(servingOrder._orderBy.transform.position);

            //2. Phục vụ
            yield return _behavior.Serve(servingOrder);

            //3. Quay về vị trí ban đầu (hoặc có thể đi phục vụ bàn khác nếu có order mới)
            yield return Rest();

        }

        IEnumerator Rest()
        {
            servingOrder = null;
            _onWaiterAvailable.Raise(_context);
            yield return _mover.MoveTo(_context.RestPosition.position);
        }

        public void DoServe(Order order)
        {
            StopAllCoroutines();
            StartCoroutine(DoServeTask(order));
        }

        void LateUpdate() => _mover.SetZToZero();
    }
}

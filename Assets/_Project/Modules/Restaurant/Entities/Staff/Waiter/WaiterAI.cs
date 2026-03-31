using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class WaiterAI : MonoBehaviour
    {
        [SerializeField] private WaiterEvent _onWaiterAvailable;

        // Cache các component cần thiết để tránh phải GetComponent nhiều lần
        private WaiterContext _context;

        [Header("[DEBUG]")]
        [SerializeField] private Order servingOrder;

        void Awake()
        {
            _context = GetComponent<WaiterContext>();
        }

        void OnEnable()
        {
            servingOrder = null;
        }

        // // Đọc luồng logic ở đây như một câu chuyện
        IEnumerator DoServe(Order order)
        {
            servingOrder = order;
            // 1. Đi đến bàn
            yield return _context.CtxMover.MoveTo(servingOrder.OrderBy.DiningTable.WorkPos.position);

            //2. Phục vụ
            yield return _context.CtxBehavior.Serve(servingOrder);

            //3. Thông báo hoàn thành

            //3. Quay về vị trí ban đầu (hoặc có thể đi phục vụ bàn khác nếu có order mới)
            yield return Rest(order);
        }

        IEnumerator DoShip(CookingTask cookingTask)
        {
            //1. Đi đến PassTable
            yield return _context.CtxMover.MoveTo(cookingTask.PassTableTarget.WorkPos_PickUp.position);

            //2. Lấy món từ PassTable
            yield return _context.CtxBehavior.PickUpFromPassTable(cookingTask);

            //3. Đi đến bàn
            yield return _context.CtxMover.MoveTo(cookingTask.Order.OrderBy.DiningTable.WorkPos.position);

            //4. Phục vụ
            yield return _context.CtxBehavior.GiveFoodToGuest(cookingTask);

            //5. Quay về vị trí ban đầu (hoặc có thể đi phục vụ bàn khác nếu có order mới)
            yield return Rest(cookingTask);
        }


        IEnumerator Rest(IStaffTask completedTask)
        {
            _context.OnTaskCompleted(completedTask);
            servingOrder = null;
            _onWaiterAvailable.Raise(_context);
            yield return _context.CtxMover.MoveTo(_context.RestPosition.position);
        }

        public void StartServe(Order order)
        {
            StopAllCoroutines();
            StartCoroutine(DoServe(order));
        }

        public void StartShip(CookingTask cookingTask)
        {
            StopAllCoroutines();
            StartCoroutine(DoShip(cookingTask));
        }

        void LateUpdate() => _context.CtxMover.SetZToZero();
    }
}

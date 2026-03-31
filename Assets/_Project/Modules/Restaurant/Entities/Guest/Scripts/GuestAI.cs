using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class GuestAI : MonoBehaviour
    {
        [SerializeField] private GuestEvent _onGuestLeft;
        [SerializeField] private TableEvent _onTableDirty;
        [SerializeField] GuestContext _context;

        //Các biến lưu trữ MainRoutine
        private DiningTable _targetTable;
        private Vector3 _exitPos;
        void OnEnable()
        {
            _targetTable = null;
            _exitPos = Vector3.zero;
        }

        // // Đọc luồng logic ở đây như một câu chuyện
        public IEnumerator MainRoutine(Vector3 destination, Vector3 exitPos, DiningTable table = null)
        {
            _targetTable = table;
            _exitPos = exitPos;

            // 1. Đi đến bàn hoặc waitingLine
            yield return MoveTo(destination);

            //2.1. Nếu là đi đến waitingLine
            if(_targetTable == null)
            {
                yield return _context.CtxBehavior.WaitInLine();
            }

            // 2.2. Nếu đã được chỉ định bàn ngay từ đầu, họ sẽ chờ phục vụ đến nhận order
            yield return _context.CtxBehavior.WaitForServe(_targetTable);

            // 3. Đợi mang đồ ăn đến
            yield return _context.CtxBehavior.WaitForFood();

            // 4. Thực hiện hành động ăn
            yield return _context.CtxBehavior.Eat();

            // 5. Trả tiền (Dễ dàng thêm bước mới vào giữa)
            yield return _context.CtxBehavior.Pay();

            // 6. Giải phóng bàn (chuyển bàn đó thành bàn bẩn, chưa ngồi được)
            _onTableDirty.Raise(_targetTable);

            // 7. Đi ra cửa
            yield return MoveTo(exitPos);

            // 8. Biến mất
            _onGuestLeft.Raise(_context);
        }

        IEnumerator MoveTo(Vector3 destination)
        {
            // Không cần check moveCoroutine thủ công nữa, cứ chạy trực tiếp
            yield return StartCoroutine(_context.CtxMover.MoveTo(destination));
        }

        public void FromWaitingLineToDiningTable(DiningTable table)
        {
            StopAllCoroutines();
            _targetTable = table;
            StartCoroutine(MainRoutine(table.transform.position, _exitPos, table));
        }

        void LateUpdate() => _context.CtxMover.SetZToZero();
    }
}

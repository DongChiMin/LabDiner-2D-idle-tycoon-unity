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
        private GuestMover _mover;
        private GuestBehavior _behavior;

        //Các biến lưu chữ MainRoutine
        private DiningTable _targetTable;
        private Vector3 _exitPos;
        private DiningTable _table;

        void Awake()
        {
            _mover = _context.CtxMover;
            _behavior = _context.CtxBehavior;
        }

        void OnEnable()
        {
            _targetTable = null;
            _exitPos = Vector3.zero;
            _table = null;
        }

        // // Đọc luồng logic ở đây như một câu chuyện
        public IEnumerator MainRoutine(Vector3 destination, Vector3 exitPos, DiningTable table = null)
        {
            _targetTable = table;
            _exitPos = exitPos;
            _table = table;

            // 1. Đi đến bàn hoặc waitingLine
            yield return MoveTo(destination);

            //2.1. Nếu là đi đến waitingLine
            if(_targetTable == null)
            {
                yield return _behavior.WaitInLine();
            }

            // 2.2. Nếu đã được chỉ định bàn ngay từ đầu, họ sẽ chờ phục vụ đến nhận order
            yield return _behavior.WaitForServe(_targetTable);
            
            

            // 3. Đợi mang đồ ăn đến
            yield return _behavior.WaitForFood();

            // 4. Thực hiện hành động ăn
            yield return _behavior.Eat();

            // 5. Trả tiền (Dễ dàng thêm bước mới vào giữa)
            yield return _behavior.Pay();

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
            yield return StartCoroutine(_mover.MoveTo(destination));
        }

        public void FromWaitingLineToDiningTable(DiningTable table)
        {
            StopAllCoroutines();
            _targetTable = table;
            StartCoroutine(MainRoutine(table.transform.position, _exitPos, table));
        }

        void LateUpdate() => _mover.SetZToZero();
    }
}

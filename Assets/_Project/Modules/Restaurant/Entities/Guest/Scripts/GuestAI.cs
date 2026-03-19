using System.Collections;
using UnityEngine;
using UnityEngine.AI;

namespace LabDiner.Restaurant
{
    public class GuestAI : MonoBehaviour
    {
        [SerializeField] GuestContext _context;
        private GuestMover _mover;
        private GuestBehavior _behavior;
        private int _tableIndex;

        void Awake()
        {
            _mover = _context.CtxMover;
            _behavior = _context.CtxBehavior;
        }


        public void Setup(Transform table, int index, Transform exit)
        {
            _tableIndex = index;
            StartCoroutine(MainRoutine(table.position, exit.position));
        }

        // Đọc luồng logic ở đây như một câu chuyện
        IEnumerator MainRoutine(Vector3 tablePos, Vector3 exitPos)
        {
            // 1. Đi đến bàn
            Debug.Log("Moving to table: " + tablePos + " - Exit: " + exitPos + " - Mover: " + _mover);
            yield return _mover.MoveTo(tablePos);

            // 2. Thực hiện hành động ăn
            yield return _behavior.Eat();

            // 3. Trả tiền (Dễ dàng thêm bước mới vào giữa)
            yield return _behavior.Pay();

            // 4. Giải phóng bàn
            SeatingManager.Instance.ReleaseTable(_tableIndex);

            // 5. Đi ra cửa
            yield return _mover.MoveTo(exitPos);

            // 6. Biến mất
            PoolContext.Instance.GuestPool.ReturnToPool(GetComponent<GuestContext>());
        }

        void LateUpdate() => _mover.SetZToZero();
    }
}

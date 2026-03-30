
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaitingLineManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Vector3 _offsetDirection = new Vector3(-1.5f, 0, 0);
        
        [Header("Events")]
        [SerializeField] private TableEvent _onTableFreed;

        [Header("[DEBUG]")]
        [SerializeField] private List<GuestContext> _waitingGuests;

        void OnEnable()
        {
            _onTableFreed.Register(PopNextGuest);
        }

        void OnDisable()
        {
            _onTableFreed.Unregister(PopNextGuest);
        }

        public bool HasWaitingGuest => _waitingGuests.Count > 0;

        public Vector3 AddToWaitingLine(GuestContext guest) {
            _waitingGuests.Add(guest);

            // Tính toán vị trí dựa trên Index
            return CalculatePosition( _waitingGuests.Count - 1 );
        }

        private Vector3 CalculatePosition(int index)
        {
            // Công thức: Start + (0, 1, 2... * Offset)
            return _startPoint.position + (_offsetDirection * index);
        }

        public void PopNextGuest(DiningTable table)
        {
            if (_waitingGuests.Count == 0) return;

            GuestContext guest = _waitingGuests[0];
            _waitingGuests.RemoveAt(0);

            // Bắt tất cả người còn lại tiến lên
            RearrangeQueue();

            LevelManagerContext.Instance.DiningTableManager.OccupyTable(table, guest);
            guest.FromWaitingLineToDiningTable(table);
        }

        private void RearrangeQueue()
        {
            for (int i = 0; i < _waitingGuests.Count; i++)
            {
                Vector3 newPos = CalculatePosition(i);
                StartCoroutine(_waitingGuests[i].CtxMover.MoveTo(newPos));
            }
        }
    }
}
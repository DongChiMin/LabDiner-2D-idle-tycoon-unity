using System.Collections;
using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class WaitingLineManager : MonoBehaviour
    {
        public bool HasWaitingGuest => _waitingGuests.Count > 0;

        [SerializeField] private DiningSeatRuntimeSetSO _diningSeatRuntimeSet;

        [Header("Spawn Settings")]
        [SerializeField] private Transform _startPoint;
        [SerializeField] private Vector3 _offsetDirection = new Vector3(-1.5f, 0, 0);
        
        [Header("Events")]
        [SerializeField] private TableEvent _onTableFreed;
        [SerializeField] private GuestEvent _onGuestWaitInLine;

        [Header("Patience Settings")]
        [SerializeField] private PatiencePieUI _patienceUI; // Reference tới script mới
        [SerializeField] private float _maxPatienceTime = 30f;

        [Header("[DEBUG]")]
        [SerializeField] private List<GuestContext> _waitingGuests;

        Coroutine _patienceCoroutine;

        void OnEnable()
        {
            _onTableFreed.Register(PopNextGuest);
            _onGuestWaitInLine.Register(HandleStartPatience);
            StopPatience();
        }

        void OnDisable()
        {
            _onTableFreed.Unregister(PopNextGuest);
            _onGuestWaitInLine.Unregister(HandleStartPatience);
            StopPatience();
        }

        #region API

        public Vector3 AddToWaitingLine(GuestContext guest) {
            _waitingGuests.Add(guest);

            // Tính toán vị trí dựa trên Index
            return CalculatePosition( _waitingGuests.Count - 1 );
        }

        public void PopNextGuest(DiningSeat seat)
        {
            if (_waitingGuests.Count == 0) return;

            GuestContext guest = _waitingGuests[0];
            _waitingGuests.RemoveAt(0);

            // Bắt tất cả người còn lại tiến lên
            RearrangeQueue();

            _diningSeatRuntimeSet.SetOccupy(seat, guest);
            guest.FromWaitingLineToDiningSeat(seat);

            StopPatience();
            if (_waitingGuests.Count > 0)
            {
                StartPatience();
            }
        }

        #endregion

        private void HandleStartPatience(GuestContext guest)
        {
            if (_waitingGuests.Count == 1 || _patienceUI.gameObject.activeSelf == false)
            {
                StartPatience();
            }
        }

        private Vector3 CalculatePosition(int index)
        {
            // Công thức: Start + (0, 1, 2... * Offset)
            return _startPoint.position + (_offsetDirection * index);
        }

        private void RearrangeQueue()
        {
            for (int i = 0; i < _waitingGuests.Count; i++)
            {
                Vector3 newPos = CalculatePosition(i);
                StartCoroutine(_waitingGuests[i].CtxMover.MoveTo(newPos));
            }
        }

        #region Patience System (No Update)

        private void StartPatience()
        {
            _patienceCoroutine = StartCoroutine(PatienceRoutine());
        }

        private void StopPatience()
        {
            if (_patienceCoroutine != null)
            {
                StopCoroutine(_patienceCoroutine);
                _patienceCoroutine = null;
            }
            _patienceUI.UpdateVisual(0f);
            _patienceUI.SetActive(false);
        }

        private IEnumerator PatienceRoutine()
        {
            float timer = _maxPatienceTime;
            _patienceUI.SetActive(true);

            while (timer > 0)
            {
                timer -= Time.deltaTime;
                float ratio = timer / _maxPatienceTime;
                
                // Đẩy việc hiển thị cho script chuyên biệt
                _patienceUI.UpdateVisual(ratio);
                
                yield return null;
            }

            // Xử lý khách bỏ về
            HandleGuestAngryLeave();
        }

        private void HandleGuestAngryLeave()
        {
            if (_waitingGuests.Count == 0) return;

            StopPatience();

            GuestContext angryGuest = _waitingGuests[0];
            _waitingGuests.RemoveAt(0);

            RearrangeQueue();

            angryGuest.LeaveAngry(); 

            if (_waitingGuests.Count > 0) StartPatience();
        }

        #endregion
    }
}
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Humanoid
{
    /// <summary>
    /// Là nơi chứa các component và các API liên quan đến khách hàng
    /// </summary>
    public class GuestContext : MonoBehaviour
    {
        public GuestAI CtxAI => _guestAI;
        public GuestBehavior CtxBehavior => _guestBehavior;
        public GuestMover CtxMover => _guestMover;
        public GuestLogic CtxLogic => _guestLogic;
        public DiningSeat DiningSeat => _diningSeat;
        public GuestOrderUI OrderCanvas => _guestOrderCanvas;

        [Header("Events")]
        [SerializeField] private GuestEvent _onGuestLeaveAngry;

        [Header("Context")]
        [SerializeField] private GuestAI _guestAI;
        [SerializeField] private GuestBehavior _guestBehavior;
        [SerializeField] private GuestMover _guestMover;
        [SerializeField] private GuestLogic _guestLogic;
        [Header("UI Canvas")]
        [SerializeField] private GuestOrderUI _guestOrderCanvas;

        [Header("[Debug]")]
        [SerializeField] private DiningSeat _diningSeat;

        #region API
        public void Setup(Order order, Vector3 destination, Vector3 exitPos, DiningSeat seat = null)
        {
            _guestBehavior.SetOrder(order);
            _guestLogic.SetOrder(order);
            _diningSeat = seat;
            StartCoroutine(_guestAI.MainRoutine(destination, exitPos, seat));
        }

        public void SetServedStatus(bool status) {
            _guestBehavior.SetServedStatus(status);
        }

        public void FromWaitingLineToDiningSeat( DiningSeat seat) {
            _diningSeat = seat;
            _guestAI.FromWaitingLineToDiningSeat(seat);
        }

        public void ReceiveFood(Restaurant.Workflow.CookingTask cookingTask)
        {
            _guestLogic.ReceiveFood(cookingTask);
        }

        public void LeaveAngry()
        {
            _guestAI.LeaveAngry();
            _onGuestLeaveAngry.Raise(this);
        }
        #endregion
    }
}
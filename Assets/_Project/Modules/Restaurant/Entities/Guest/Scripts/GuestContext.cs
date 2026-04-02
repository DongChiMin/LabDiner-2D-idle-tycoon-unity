
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
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
        public DiningTable DiningTable => _diningTable;
        public GuestOrderCanvas OrderCanvas => _guestOrderCanvas;

        [Header("Context")]
        [SerializeField] private GuestAI _guestAI;
        [SerializeField] private GuestBehavior _guestBehavior;
        [SerializeField] private GuestMover _guestMover;
        [SerializeField] private GuestLogic _guestLogic;
        [Header("UI Canvas")]
        [SerializeField] private GuestOrderCanvas _guestOrderCanvas;

        [Header("[Debug]")]
        [SerializeField] private DiningTable _diningTable;

        #region API
        public void Setup(Order order, Vector3 destination, Vector3 exitPos, DiningTable table = null)
        {
            _guestBehavior.SetOrder(order);
            _guestLogic.SetOrder(order);
            _diningTable = table;
            StartCoroutine(_guestAI.MainRoutine(destination, exitPos, table));
        }

        public void SetServedStatus(bool status) {
            _guestBehavior.SetServedStatus(status);
        }

        public void FromWaitingLineToDiningTable( DiningTable table) {
            _diningTable = table;
            _guestAI.FromWaitingLineToDiningTable(table);
        }

        public void ReceiveFood(CookingTask cookingTask)
        {
            _guestLogic.ReceiveFood(cookingTask);
        }

        public void LeaveAngry()
        {
            _guestAI.LeaveAngry();
        }
        #endregion
    }
}
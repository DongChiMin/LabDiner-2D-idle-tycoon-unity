
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class GuestContext : MonoBehaviour
    {
        [SerializeField] private GuestAI _guestAI;
        [SerializeField] private GuestBehavior _guestBehavior;
        [SerializeField] private GuestMover _guestMover;
        public GuestAI CtxAI => _guestAI;
        public GuestBehavior CtxBehavior => _guestBehavior;
        public GuestMover CtxMover => _guestMover;

        public void Setup(Order order, Vector3 destination, Vector3 exitPos, DiningTable table = null)
        {
            _guestBehavior.SetOrder(order);
            StartCoroutine(_guestAI.MainRoutine(destination, exitPos, table));
        }

        public void SetServedStatus(bool status) {
            _guestBehavior.SetServedStatus(status);
        }

        public void FromWaitingLineToDiningTable( DiningTable table) {
            _guestAI.FromWaitingLineToDiningTable(table);
        }
    }
}
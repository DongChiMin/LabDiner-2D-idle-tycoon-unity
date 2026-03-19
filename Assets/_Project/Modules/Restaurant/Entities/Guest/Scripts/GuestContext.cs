
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
    }
}
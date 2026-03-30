
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaiterBehavior : MonoBehaviour
    {
        [SerializeField] private WaiterContext _context;
        [SerializeField] private float _serveDuration = 3f;

        public IEnumerator Serve(Order order)
        {
            GuestContext guest = order.OrderBy;

            Debug.Log("TODO: show tiến trình serving tại đây");
            yield return new WaitForSeconds(_serveDuration);
            guest.SetServedStatus(true);
            _context.OnTaskCompleted(order);
        }

    }
}
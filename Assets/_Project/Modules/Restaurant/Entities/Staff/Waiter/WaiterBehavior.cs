
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class WaiterBehavior : MonoBehaviour
    {
        [Header("Serve")]
        [SerializeField] private WaiterContext _context;
        [SerializeField] private float _serveDuration = 3f;
        [Header("Shipp")]
        [SerializeField] private float _pickUpDuration = 0f;
        [SerializeField] private float _giveFoodDuration = 0f;

        public IEnumerator Serve(Order order)
        {
            GuestContext guest = order.OrderBy;

            yield return new WaitForSeconds(_serveDuration);
            guest.SetServedStatus(true);
        }

        public IEnumerator PickUpFromPassTable(CookingTask cookingTask)
        {
            Debug.Log("TODO: show tiến trình pick up tại đây");
            PassTable passTable = cookingTask.PassTableTarget;
            passTable.PickUpDish(cookingTask);
            yield return new WaitForSeconds(_pickUpDuration); // Giả lập thời gian lấy món
        }

        public IEnumerator GiveFoodToGuest(CookingTask cookingTask)
        {
            GuestContext guest = cookingTask.Order.OrderBy;
            guest.ReceiveFood(cookingTask);
            Debug.Log("TODO: show tiến trình phục vụ tại đây");
            yield return new WaitForSeconds(_giveFoodDuration);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class MultitaskChefBehavior : MonoBehaviour
    {
        [Header("Cook Settings")]
        [SerializeField] private float cookMultiplier = 1f;
        [Header("Serve Settings")]
        [SerializeField] private float _serveDuration = 3f;
        [SerializeField] private float _giveFoodDuration = 0f;

        public IEnumerator Cook(CookingTask task)
        {
            CoreStation coreStation = task.CoreStation;
            Debug.Log("TODO: hoàn thiện công thức thời gian nấu");
            // Bật hiệu ứng khói, lửa, âm thanh xèo xèo...
            yield return new WaitForSeconds(3 * (1/ cookMultiplier));
            task.StationTarget.SetStatus(true);
        }

        public IEnumerator Serve(Order order)
        {
            GuestContext guest = order.OrderBy;

            yield return new WaitForSeconds(_serveDuration);
            guest.SetServedStatus(true);
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
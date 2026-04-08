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

        private MultitaskChefContext _context;

        void Start()
        {
            _context = GetComponent<MultitaskChefContext>();
        }

        public IEnumerator Cook(CookingTask task)
        {
            CoreStation coreStation = task.CoreStation;
            Debug.Log("TODO: hoàn thiện công thức thời gian nấu");
            float cookTime = 3 * (1/ cookMultiplier);
            _context.ProgressPieUI.StartProgressPie(cookTime);
            yield return new WaitForSeconds(cookTime);
            task.StationTarget.SetStatus(true);
            _context.CarryDishUI.UpdateCookingTaskPrice(task);
            _context.CarryDishUI.CarryDish(task);
        }

        public IEnumerator Serve(Order order)
        {
            GuestContext guest = order.OrderBy;

            _context.ProgressPieUI.StartProgressPie(_serveDuration);
            yield return new WaitForSeconds(_serveDuration);
            guest.SetServedStatus(true);
        }

        public IEnumerator GiveFoodToGuest(CookingTask cookingTask)
        {
            GuestContext guest = cookingTask.Order.OrderBy;
            guest.ReceiveFood(cookingTask);
            Debug.Log("TODO: show tiến trình phục vụ tại đây");
            yield return new WaitForSeconds(_giveFoodDuration);
            _context.CarryDishUI.Finish(cookingTask);
        }
    }
}
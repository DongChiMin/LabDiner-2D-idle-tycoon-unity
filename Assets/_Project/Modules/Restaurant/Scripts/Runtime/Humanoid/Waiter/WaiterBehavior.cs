
using System.Collections;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Model;
using UnityEngine;

namespace LabDiner.Restaurant.Humanoid
{
    public class WaiterBehavior : MonoBehaviour
    {
        [Header("Serve")]
        [SerializeField] private float _serveDuration = 3f;
        [Header("Ship")]
        [SerializeField] private float _pickUpDuration = 0f;
        [SerializeField] private float _giveFoodDuration = 0f;

        private WaiterContext _ctx;

        private void Awake()
        {
            _ctx = GetComponent<WaiterContext>();
        }

        public IEnumerator Serve(Order order)
        {
            GuestContext guest = order.OrderBy;
            _ctx.ProgressPieLogic.StartProgressPie(_serveDuration);
            yield return new WaitForSeconds(_serveDuration);
            guest.SetServedStatus(true);
        }

        public IEnumerator PickUpFromPassTable(CookingTask cookingTask)
        {
            yield break;
            // PassTable passTable = cookingTask.PassTableTarget;
            // passTable.PickUpDish(cookingTask);
            // _ctx.ProgressPieLogic.StartProgressPie(_pickUpDuration);
            // yield return new WaitForSeconds(_pickUpDuration); // Giả lập thời gian lấy món
            // _ctx.CarryDishLogic.CarryDish(cookingTask);
        }

        public IEnumerator GiveFoodToGuest(CookingTask cookingTask)
        {
            yield break;
            // GuestContext guest = cookingTask.Order.OrderBy;
            // guest.ReceiveFood(cookingTask);
            
            // //TODO: điều chỉnh thời gian phục vụ nếu cần thiết, hiện tại để 0 để món ăn được giao ngay lập tức
            // _ctx.ProgressPieLogic.StartProgressPie(_giveFoodDuration);
            // yield return new WaitForSeconds(_giveFoodDuration);
            // _ctx.CarryDishLogic.Finish(cookingTask);
        }
    }
}
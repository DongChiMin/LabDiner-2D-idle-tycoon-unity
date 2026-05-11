using System.Collections;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Model;
using UnityEngine;

namespace LabDiner.Restaurant.Humanoid
{
    public class ChefBehavior : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float cookMultiplier = 1f;
        [SerializeField] private float placeOnPassTableDuration = 0f;

        private ChefContext _context;

        void Start()
        {
            _context = GetComponent<ChefContext>();    
        }

        public IEnumerator Cook(CookingTask task)
        {
            yield return null;
            // //IMPORTANT: Công thức nấu ăn: Thời gian nấu = Thời gian cơ bản của món ăn / (1 + cookMultiplier)
            // CoreStation coreStation = task.CoreStation;
            // float cookTime = coreStation.RawProcessTime / (1 + cookMultiplier);

            // _context.ProgressPieUI.StartProgressPie(cookTime);
            // yield return new WaitForSeconds(cookTime);
            // task.StationTarget.SetStatus(true);
            // _context.CarryDishUI.UpdateCookingTaskPrice(task);
            // _context.CarryDishUI.CarryDish(task);
        }

        public IEnumerator PlaceOnPassTable(CookingTask task)
        {
            yield return null;
            // task.PassTableTarget.PlaceTaskOnPassTable(task);
            // // Di chuyển món ăn đến PassTable, bật hiệu ứng đặt món...
            // _context.ProgressPieUI.StartProgressPie(placeOnPassTableDuration);
            // yield return new WaitForSeconds(placeOnPassTableDuration);
            // _context.CarryDishUI.Finish(task);
        }
    }
}
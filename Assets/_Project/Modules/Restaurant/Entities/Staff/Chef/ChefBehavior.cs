using System.Collections;
using UnityEngine;

namespace LabDiner.Restaurant
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
            CoreStation coreStation = task.CoreStation;
            Debug.Log("TODO: hoàn thiện công thức thời gian nấu");
            // Bật hiệu ứng khói, lửa, âm thanh xèo xèo...
            yield return new WaitForSeconds(3 * (1/ cookMultiplier));
            task.StationTarget.SetStatus(true);
        }

        public IEnumerator PlaceOnPassTable(CookingTask task)
        {
            task.PassTableTarget.PlaceTaskOnPassTable(task);
            _context.OnTaskCompleted(task);
            // Di chuyển món ăn đến PassTable, bật hiệu ứng đặt món...
            yield return new WaitForSeconds(placeOnPassTableDuration);
        }
    }
}
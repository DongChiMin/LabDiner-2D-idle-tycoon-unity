
using System.Collections;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class ChefAI : MonoBehaviour
    {
        private ChefMover _mover;
        // Tạm thời chưa cần ChefActions, ta dùng Debug.Log để test luồng

        public Transform prepStation;
        public Transform cookStation;
        public Transform counterStation;

        void Start()
        {
            _mover = GetComponent<ChefMover>();
            StartCoroutine(WorkRoutine());
        }

        IEnumerator WorkRoutine()
        {
            Debug.Log("Chef đã sẵn sàng!");
            while (true)
            {
                // // 1. Đứng đợi cho đến khi có đơn
                // yield return new WaitUntil(() => OrderManager.Instance.HasPendingOrder());

                // Order currentOrder = OrderManager.Instance.GetNextOrder();

                // // 2. Đi lấy nguyên liệu
                // yield return _mover.MoveTo(prepStation.position);
                // Debug.Log("Đang lấy nguyên liệu cho " + currentOrder.DishName);
                // yield return new WaitForSeconds(1f);

                // // 3. Đi nấu
                // yield return _mover.MoveTo(cookStation.position);
                // Debug.Log("Đang nấu " + currentOrder.DishName);
                // yield return new WaitForSeconds(currentOrder.CookTime);

                // // 4. Trả món
                // yield return _mover.MoveTo(counterStation.position);
                // Debug.Log("Đã xong món " + currentOrder.DishName + " cho bàn " + currentOrder.TableIndex);
            }
        }
    }
}
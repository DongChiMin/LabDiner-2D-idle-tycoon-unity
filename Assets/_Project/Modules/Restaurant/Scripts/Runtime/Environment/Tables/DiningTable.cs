using System.Collections.Generic;
using LabDiner.Restaurant.Model;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public class DiningTable : MonoBehaviour
    {
        public List<DiningSeat> Seats;

#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (Seats == null) return;

            foreach (DiningSeat seat in Seats)
            {
                if (seat == null)
                {
                    // Lưu ý: Hạn chế dùng Debug.Log trong OnDrawGizmos vì nó chạy liên tục mỗi frame
                    // Chỉ nên check nhanh để tránh crash
                    continue;
                }

                // 1. Vẽ vị trí ghế (Seat Position)
                Gizmos.color = seat.IsOccupied ? Color.red : Color.green;
                Gizmos.DrawWireSphere(seat.transform.position, 0.2f);

                // 2. Vẽ vị trí làm việc (Work Position) - Hình vuông "cao cao" như bạn muốn
                Gizmos.color = Color.yellow;
                Vector3 workPos = seat.WorkPos.position;
                // Vẽ khối đứng: rộng 0.4, cao 0.8
                Gizmos.DrawWireCube(workPos + Vector3.up * 0.4f, new Vector3(0.4f, 0.8f, 0.1f));

                // 3. Vẽ đường nối giữa Ghế và WorkPos để dễ phân biệt
                Gizmos.color = new Color(1f, 1f, 1f, 0.5f); // Màu trắng mờ
                Gizmos.DrawLine(seat.transform.position, workPos);

                // 4. Hiển thị tên ghế và trạng thái
                string label = $"{seat.name}";
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(seat.transform.position + Vector3.up * 0.5f, label);
            }
        }
#endif
    }
}
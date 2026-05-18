using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public partial class DiningSeat : MonoBehaviour
    {
        public bool IsOccupied => _occupiedGuest != null;
        public Transform WorkPos => _workPos;

        [Header("Settings")]
        [SerializeField] private Transform _workPos;

        [Header("[Runtime]")]
        [SerializeField] private GuestContext _occupiedGuest;

        [Header("Gizmos Settings")]
        [SerializeField] private float _workCenterY = 0f;
        [SerializeField] private Vector3 _workPosSize = new Vector3(1.5f, 2.25f, 0.1f);

        public void Occupy(GuestContext guest)
        {
            _occupiedGuest = guest;
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
                // 1. Vẽ vị trí ghế (Seat Position)
                Gizmos.color = IsOccupied ? Color.red : Color.green;
                Gizmos.DrawWireSphere(transform.position, 0.2f);

                // 2. Vẽ vị trí làm việc (Work Position) - Hình vuông "cao cao" như bạn muốn
                Gizmos.color = Color.yellow;
                Vector3 workPos = WorkPos.position;
                Vector3 center = workPos + Vector3.up * _workCenterY;
                Gizmos.DrawWireCube(center, _workPosSize);

                // 3. Vẽ đường nối giữa Ghế và WorkPos để dễ phân biệt
                Gizmos.color = new Color(1f, 1f, 1f, 0.5f); // Màu trắng mờ
                Gizmos.DrawLine(transform.position, workPos);

                // 4. Hiển thị tên ghế và trạng thái
                string label = $"{name}";
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(transform.position + Vector3.up * 0.5f, label);
            }
        
        #endif
    }
}
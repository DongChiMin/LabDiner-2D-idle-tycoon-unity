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
        public Transform SitPos => _sitPos;
        public Transform TipPos => _tipPos;

        [Header("Settings")]
        [SerializeField] private Transform _workPos;
        [SerializeField] private Transform _sitPos;
        [SerializeField] private Transform _tipPos;

        [Header("[Runtime]")]
        [SerializeField] private GuestContext _occupiedGuest;

        [Header("Gizmos Settings")]
        [SerializeField] private Vector3 _workPosSize = new Vector3(1.5f, 2.25f, 0.1f);
        [SerializeField] private Vector3 _sitPosSize = new Vector3(1.5f, 2.25f, 0.1f);
        [SerializeField] private Vector3 _tipPosSize = new Vector3(0.5f, 1f, 0.5f);

        public void Occupy(GuestContext guest)
        {
            _occupiedGuest = guest;
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
                // 1. Vẽ vị trí ghế (Seat Position) - Dạng hình chữ nhật
                Gizmos.color = IsOccupied ? Color.red : Color.green;
                Vector3 seatCenter = _sitPos.transform.position + _sitPosSize.y * 0.5f * Vector3.up; // Center của ghế được offset lên một nửa chiều cao
                Vector3 seatSize = _sitPosSize;
                Gizmos.DrawWireCube(seatCenter, seatSize);

                // 2. Vẽ vị trí làm việc (Work Position) - Hình vuông "cao cao" như bạn muốn
                Gizmos.color = Color.yellow;
                Vector3 workPos = WorkPos.position;
                Vector3 center = new Vector3(workPos.x, workPos.y + _workPosSize.y * 0.5f, workPos.z);
                Gizmos.DrawWireCube(center, _workPosSize);
                
                // 3. Vẽ vị trí tip (Tip Position) - Hình vuông nhỏ
                Gizmos.color = Color.cyan;
                Vector3 tipPos = TipPos.position;
                Vector3 tipCenter = new Vector3(tipPos.x, tipPos.y + _tipPosSize.y * 0.5f, tipPos.z);
                Vector3 tipSize = _tipPosSize;
                Gizmos.DrawWireCube(tipCenter, tipSize);

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
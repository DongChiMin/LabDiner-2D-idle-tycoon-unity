#if UNITY_EDITOR
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public partial class StaffSpawner : MonoBehaviour, IStaffUnboxer
    {
        [Header("[DEBUG]")]
        [SerializeField] private bool _showGizmos = true;
        [SerializeField] private Color _gizmoColor = Color.cyan;
        [SerializeField] private float _workGizmoCenterY = 0f;
        [SerializeField] private Vector3 _restPointDimensions = new Vector3(0.6f, 1.2f, 0.1f); // Hình vuông cao cao

        protected virtual void OnDrawGizmos()
        {
            if (!_showGizmos || _restPositions == null) return;

            Gizmos.color = _gizmoColor;

            for (int i = 0; i < _restPositions.Count; i++)
            {
                if (_restPositions[i] == null) continue;

                Vector3 pos = _restPositions[i].position;

                // Vẽ hình hộp đứng (đại diện cho vị trí nhân viên đứng nghỉ)
                // Center được offset lên một nửa chiều cao để hình nằm trên mặt sàn
                Vector3 center = pos + Vector3.up * _workGizmoCenterY;;
                Gizmos.DrawWireCube(center, _restPointDimensions);

                // Reset lại màu chính cho vòng lặp sau
                Gizmos.color = _gizmoColor;

                // Ghi số thứ tự điểm nghỉ (tùy chọn)
                UnityEditor.Handles.Label(pos + Vector3.up * (_restPointDimensions.y + 0.2f), $"Rest {i}");
            }
        }
    }
}
#endif
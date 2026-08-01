using UnityEngine;

namespace LabDiner.Restaurant
{
    [ExecuteAlways] // Cho phép vẽ Gizmos ngay cả khi KHÔNG BẤM PLAY
    public class DebugScreenPlay : MonoBehaviour, IGizmosDrawable
    {
        [SerializeField] private bool _showGizmos = true;
        public bool ShowGizmos { get => _showGizmos; set => _showGizmos = value; }

       [Header("Target Device Aspect Ratio (Tỉ lệ màn hình)")]
        [SerializeField] private float _aspectWidth = 9f;
        [SerializeField] private float _aspectHeight = 16f;

        [Header("Camera Settings Simulation")]
        [Tooltip("Orthographic Size của Camera cậu định dùng cho Game (ví dụ: 5, 7, 10...)")]
        [SerializeField] private float _simulatedOrthoSize = 5f;

        [Header("Gizmos Display")]
        [SerializeField] private Color _frameColor = Color.cyan;
        [SerializeField] private Color _safeZoneColor = Color.yellow;
        [SerializeField] private Vector2 _centerOffset = Vector2.zero; // Dời tâm khung nếu Level không nằm ở gốc (0,0)

        [Header("SafeZone Settings")]
        [SerializeField] private float _safeZoneHeightRatio = 0.7f; // Chiều cao vùng an toàn so với khung giả lập (ví dụ: 0.85 = 85% chiều cao khung)
        [SerializeField] private float _safeZoneWidthRatio = 1f; // Chiều rộng vùng an toàn so với khung giả lập (ví dụ: 1 = 100% chiều rộng khung)

        private void OnDrawGizmos()
        {
            if(!_showGizmos) return;

            // Calculate screen bounds based on simulated Camera Orthographic Size
            float targetAspect = _aspectWidth / _aspectHeight;

            // Height = OrthoSize * 2
            float frameHeight = _simulatedOrthoSize * 2f;
            float frameWidth = frameHeight * targetAspect;

            // Vị trí tâm của khung sẽ lấy theo Transform của Prefab + Offset
            Vector3 center = transform.position + new Vector3(_centerOffset.x, _centerOffset.y, 0f);

            // 1. Vẽ khung màn hình giả lập
            Gizmos.color = _frameColor;
            Gizmos.DrawWireCube(center, new Vector3(frameWidth, frameHeight, 0.1f));

            // 2. Vẽ Safe Zone (vùng an toàn tránh tai thỏ/thanh điều hướng)
            Gizmos.color = _safeZoneColor;
            Vector3 safeZoneSize = new Vector3(frameWidth * _safeZoneWidthRatio, frameHeight * _safeZoneHeightRatio, 0.1f);
            Gizmos.DrawWireCube(center, safeZoneSize);

            // 3. Vẽ tâm (Center Cross) để biết điểm Focus của Camera
            Gizmos.color = Color.red;
            Gizmos.DrawRay(center + Vector3.left * 0.5f, Vector3.right);
            Gizmos.DrawRay(center + Vector3.down * 0.5f, Vector3.up);
        }
    }

}


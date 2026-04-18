using LabDiner.Shared.SO;
using LabDiner.Shared;
using LabDiner.Shared.Input;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CameraController : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float _dragSensitivity = 0.01f;
        [SerializeField] private float _smoothSpeed = 10f;

        [Header("Debug")]
        [SerializeField] private float _minY = -5;
        [SerializeField] private float _maxY = 5;
        
        private float _targetY;
        private Vector2 _lastDragPos;
        private bool _isDragging;
        private bool _isLockedByUI;

        public void Init(LevelConfigSO config)
        {
            _minY = config.minVerticalPos;
            _maxY = config.maxVerticalPos;
            
            // Đặt vị trí mục tiêu ban đầu là vị trí hiện tại
            _targetY = transform.position.y;
        }

        private void OnEnable()
        {
            // Đăng ký các sự kiện từ InputReader
            InputReader.OnPointerDown += HandlePointerDown;
            InputReader.OnDrag += HandleDrag;
            InputReader.OnPointerUp += HandlePointerUp;
            UIManager.OnUIStateChanged += SetLock;
        }

        private void OnDisable()
        {
            // Hủy đăng ký để tránh memory leak
            InputReader.OnPointerDown -= HandlePointerDown;
            InputReader.OnDrag -= HandleDrag;
            InputReader.OnPointerUp -= HandlePointerUp;
            UIManager.OnUIStateChanged -= SetLock;
        }

        private void SetLock(bool isLocked)
        {
            _isLockedByUI = isLocked;
            if (isLocked) _isDragging = false; // Ngắt drag ngay lập tức nếu đang vuốt mà UI hiện lên
        }

        private void HandlePointerDown(Vector2 pos)
        {
            if (_isLockedByUI) return;

            _lastDragPos = pos;
            _isDragging = true;
        }

        private void HandlePointerUp(Vector2 pos)
        {
            _isDragging = false;
        }

        private void HandleDrag(Vector2 currentPos)
        {
            if (!_isDragging || _isLockedByUI) return;

            // Tính toán độ dời so với frame trước
            float deltaY = currentPos.y - _lastDragPos.y;
            
            // Cập nhật vị trí mục tiêu (Trừ vì vuốt lên thì cam đi xuống)
            _targetY -= deltaY * _dragSensitivity;

            // Giới hạn trong Bounds của level
            _targetY = Mathf.Clamp(_targetY, _minY, _maxY);

            _lastDragPos = currentPos;
        }

        private void LateUpdate()
        {
            // Di chuyển camera mượt mà tới vị trí mục tiêu
            Vector3 pos = transform.position;
            float smoothedY = Mathf.Lerp(pos.y, _targetY, Time.deltaTime * _smoothSpeed);
            transform.position = new Vector3(pos.x, smoothedY, pos.z);
        }

        private void OnDrawGizmos()
        {
            Camera cam = GetComponent<Camera>();
            if (cam == null) cam = Camera.main;
            if (cam == null || !cam.orthographic) return;

            // 1. Tính toán kích thước khung hình
            float camHeight = cam.orthographicSize * 2;
            float camWidth = camHeight * cam.aspect;

            // 2. Vẽ vùng giới hạn mà tâm Camera có thể di chuyển (Đường line cũ của cậu)
            Gizmos.color = Color.green;
            Vector3 centerMin = new Vector3(transform.position.x, _minY, 0);
            Vector3 centerMax = new Vector3(transform.position.x, _maxY, 0);
            Gizmos.DrawLine(centerMin, centerMax);

            // 3. Vẽ viền KHÔNG GIAN thực tế mà người chơi nhìn thấy
            // Viền dưới cùng (Khi cam ở minY)
            Gizmos.color = Color.red;
            float finalBottomEdge = _minY - cam.orthographicSize;
            Gizmos.DrawLine(new Vector3(-camWidth/2, finalBottomEdge, 0), new Vector3(camWidth/2, finalBottomEdge, 0));

            // Viền trên cùng (Khi cam ở maxY)
            Gizmos.color = Color.red;
            float finalTopEdge = _maxY + cam.orthographicSize;
            Gizmos.DrawLine(new Vector3(-camWidth/2, finalTopEdge, 0), new Vector3(camWidth/2, finalTopEdge, 0));

            // 4. Vẽ 2 cái hộp đại diện cho khung hình tại 2 điểm cực hạn
            Gizmos.color = new Color(1, 1, 0, 0.3f); // Màu vàng trong suốt
            Gizmos.DrawWireCube(centerMin, new Vector3(camWidth, camHeight, 0.1f));
            Gizmos.DrawWireCube(centerMax, new Vector3(camWidth, camHeight, 0.1f));
        }
    }
}
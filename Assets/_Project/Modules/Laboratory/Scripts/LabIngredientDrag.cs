using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Laboratory
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TargetJoint2D))]
    public class LabIngredientDrag : MonoBehaviour, IDraggable
    {
        [SerializeField] private IngredientSO data;
        [SerializeField] private Rigidbody2D _rb;
        [SerializeField] private TargetJoint2D _joint;

        [Header("Dragging Settings")]
        [SerializeField] private float _RandomRotationEndDragging = 2f;
        [SerializeField] private float _DragDamping = 10f;
        [SerializeField] private float _DragLinearDamping = 1f;

        public Transform Transform => transform;
        public Rigidbody2D Rigidbody => _rb;

        private void Awake()
        {
            // Tắt joint lúc đầu
            _joint.enabled = false;
        }

        public void OnDragStart(Vector3 worldPosition)
        {
            _joint.enabled = true;

            // Đặt Anchor của Joint chính là điểm người chơi click vào (tính theo tọa độ Local của vật)
            // Điều này giúp vật xoay quanh đúng điểm đó thay vì xoay quanh tâm.
            _joint.anchor = transform.InverseTransformPoint(worldPosition);
            _joint.target = worldPosition;

            // Tăng lực cản để vật không bị xoay quá "loạn" khi kéo nhanh
            _rb.angularDamping = _DragDamping;
            _rb.linearDamping = _DragLinearDamping;

            // Thêm chút xoay ngẫu nhiên khi lấy cho sinh động
            _rb.AddTorque(Random.Range(-_RandomRotationEndDragging*5f, _RandomRotationEndDragging*5f), ForceMode2D.Impulse);
        }

        public void OnDragContinue(Vector3 worldPosition)
        {
            // Cập nhật vị trí đích của Joint theo chuột
            _joint.target = worldPosition;
        }

        public void OnDragEnd()
        {
            _joint.enabled = false;

            // Trả lại các chỉ số vật lý bình thường để rơi tự nhiên
            _rb.angularDamping = 0.5f;
            _rb.linearDamping = 0.05f;

            // Thêm chút xoay ngẫu nhiên khi buông tay cho sinh động
            _rb.AddTorque(Random.Range(-_RandomRotationEndDragging, _RandomRotationEndDragging), ForceMode2D.Impulse);
        }
    }
}
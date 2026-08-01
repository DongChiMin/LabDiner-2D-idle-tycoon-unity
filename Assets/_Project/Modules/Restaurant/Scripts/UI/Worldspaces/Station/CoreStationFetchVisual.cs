using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using LabDiner.Shared.Input;
using LabDiner.Shared.SO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    
    enum ArrowAlignment 
    {
        Left,
        Center,
        Right
    }

    public class CoreStationFetchVisual : MonoBehaviour, IGizmosDrawable
    {
        [SerializeField] private bool _showGizmos = true;
        public bool ShowGizmos { get => _showGizmos; set => _showGizmos = value; }

        [Header("Settings")]
        [SerializeField] private ArrowAlignment _arrowAlignment;
        [SerializeField] private CoreStationSO _stationData;

        [Header("Data References")]
        [SerializeField] private SpriteRenderer _buttonIcon;
        [SerializeField] private Image _unlockStationIcon;
        [SerializeField] private TextMeshProUGUI _upgradeStationText;
        [SerializeField] private TextMeshProUGUI _unlockStationName;

        [Header("Layout References")]
        [SerializeField] private RectTransform _panelRect;
        [SerializeField] private float _panelOffset = 1.5f;
        [SerializeField] private List<HorizontalLayoutGroup> _layoutGroups = new List<HorizontalLayoutGroup>();


        private ArrowAlignment _currentArrowAlignment;
        private CoreStationSO _currentStationData;

        public void FetchVisual(CoreStationUIData data)
        {
#if UNITY_EDITOR
            // Hủy các lệnh gọi delay cũ trước đó (nếu có) để tránh bị spam gọi trùng khi bạn gõ phím nhanh trên Inspector
            UnityEditor.EditorApplication.delayCall -= () => ExecuteFetch(data);

            // Đăng ký lệnh gọi mới
            UnityEditor.EditorApplication.delayCall += () =>
            {
                // Kiểm tra null đề phòng trường hợp bạn vừa xóa Object trong lúc Editor đang delay
                if (this != null)
                {
                    ExecuteFetch(data);
                }
            };
#endif

        }

        private void ExecuteFetch(CoreStationUIData data)
        {

            _buttonIcon.sprite = data.CoreStationSO.Dish.DishIcon;
            _unlockStationIcon.sprite = data.CoreStationSO.Dish.StationIcon;
            _unlockStationName.text = data.CoreStationSO.Dish.Name;
            _upgradeStationText.text = data.CoreStationSO.Dish.Name;
        }

        void OnValidate()
        {
            if(_stationData == null)
            {
                CoreStation coreStation = GetComponent<CoreStation>();
                if(coreStation != null)
                {
                    _stationData = coreStation.CoreStationSO;
                }
            }

            if(_currentArrowAlignment != _arrowAlignment)
            {
                _currentArrowAlignment = _arrowAlignment;
                foreach(var layout in _layoutGroups)
                {
                    switch(_currentArrowAlignment)
                    {
                        case ArrowAlignment.Left:
                            layout.childAlignment = TextAnchor.MiddleRight;
                            Vector3 pos = _panelRect.anchoredPosition;
                            pos.x = -1 * _panelOffset; // Đảo ngược giá trị x để nó nằm bên trái
                            _panelRect.anchoredPosition = pos;
                            break;
                        case ArrowAlignment.Center:
                            layout.childAlignment = TextAnchor.MiddleCenter;
                            pos = _panelRect.anchoredPosition;
                            pos.x = 0;
                            _panelRect.anchoredPosition = pos;
                            break;
                        case ArrowAlignment.Right:
                            layout.childAlignment = TextAnchor.MiddleLeft;
                            pos = _panelRect.anchoredPosition;
                            pos.x = _panelOffset;
                            _panelRect.anchoredPosition = pos;
                            break;
                    }
                }
            }

            if(_currentStationData != _stationData)
            {
                _currentStationData = _stationData;
                CoreStation coreStation = GetComponent<CoreStation>();
                if(coreStation != null)
                {
                    coreStation.SetData(_stationData);
                    FetchVisual(coreStation.GetUIData());
                }
            }
        }

        [Header("Gizmos Settings")]
        [SerializeField] private float _gizmoLineLength = 1f;
        [SerializeField] private float _gizmoArrowSize = 0.2f;
        void OnDrawGizmos()
        {
            if (!_showGizmos) return;

            Vector3 startPos = transform.position;
            Vector3 direction = Vector3.zero;
            Color gizmoColor = Color.white;
            string alignmentLabel = "CENTER";

            // Xác định vector hướng, màu sắc và nội dung chữ dựa trên Alignment
            switch (_arrowAlignment)
            {
                case ArrowAlignment.Left:
                    direction = Vector3.left;
                    gizmoColor = Color.red;
                    alignmentLabel = "◀ LEFT";
                    break;
                case ArrowAlignment.Center:
                    direction = Vector3.up;
                    gizmoColor = Color.yellow;
                    alignmentLabel = "● CENTER";
                    break;
                case ArrowAlignment.Right:
                    direction = Vector3.right;
                    gizmoColor = Color.green;
                    alignmentLabel = "RIGHT ▶";
                    break;
            }

            Gizmos.color = gizmoColor;

            // Tính vị trí để đặt chữ Label (nằm ở đầu mũi tên hoặc phía trên tâm một chút)
            Vector3 labelPos = startPos + direction * _gizmoLineLength;

            if (_arrowAlignment == ArrowAlignment.Center)
            {
                // Nếu ở giữa, vẽ một vòng tròn/khối tâm nhỏ để biểu thị "đứng im tại chỗ"
                Gizmos.DrawWireSphere(startPos, _gizmoArrowSize);
                Gizmos.DrawLine(startPos, startPos + direction * (_gizmoLineLength * 0.5f));
                labelPos = startPos + direction * (_gizmoLineLength * 0.6f); // Dịch chữ gần tâm hơn một chút cho đẹp
            }
            else
            {
                // Vẽ đường thẳng chính
                Gizmos.DrawLine(startPos, labelPos);

                // Vẽ 2 nhánh mũi tên phụ phụ thuộc vào hướng chính
                Vector3 rightWing = Quaternion.LookRotation(Vector3.forward, direction) * new Vector3(0.5f, -1f, 0f);
                Vector3 leftWing = Quaternion.LookRotation(Vector3.forward, direction) * new Vector3(-0.5f, -1f, 0f);

                Gizmos.DrawLine(labelPos, labelPos + rightWing.normalized * _gizmoArrowSize);
                Gizmos.DrawLine(labelPos, labelPos + leftWing.normalized * _gizmoArrowSize);
                
                // Đẩy vị trí text ra xa đầu mũi tên một tẹo để tránh đè lên nét vẽ
                labelPos += direction * 0.15f; 
            }

#if UNITY_EDITOR
            // Thiết lập font style cho chữ trong Scene View
            GUIStyle labelStyle = new GUIStyle();
            labelStyle.normal.textColor = gizmoColor;
            labelStyle.fontSize = 12;
            labelStyle.fontStyle = FontStyle.Bold;
            labelStyle.alignment = TextAnchor.MiddleCenter; // Căn chữ nằm chính giữa tọa độ được chỉ định

            // Vẽ chữ nhãn lên Scene View
            UnityEditor.Handles.Label(labelPos, "UI: " + alignmentLabel, labelStyle);
#endif
        }
    
    }
}
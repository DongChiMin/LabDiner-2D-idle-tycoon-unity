using UnityEngine;
using LabDiner.Shared.Input;
namespace LabDiner.Shared.UI
{
    public class DismissableUI : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas; // Kéo Canvas chứa UI này vào đây
        private RectTransform _rectTransform;
        private PopScaleEffect _popEffect;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _popEffect = GetComponent<PopScaleEffect>();
        }

        private void OnEnable()
        {
            // Đăng ký lắng nghe tiếng hét từ InputReader
            InputReader.OnGlobalClick += HandleGlobalClick;
        }

        private void OnDisable()
        {
            InputReader.OnGlobalClick -= HandleGlobalClick;
        }

        private void HandleGlobalClick(Vector2 mousePos)
        {
            // Nếu UI đang ẩn thì thôi khỏi check
            if (!gameObject.activeInHierarchy) return;

            // KIỂM TRA: Vị trí click có nằm TRONG cái UI này không?
            // Lưu ý: Dùng Camera.main nếu là World Space, dùng null nếu là Overlay
            Camera eventCamera = (_canvas.renderMode == RenderMode.ScreenSpaceOverlay) ? null : Camera.main;

            if (!RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, mousePos, eventCamera))
            {
                // Nếu click RA NGOÀI -> Tắt UI
                if(_popEffect != null)
                {
                    _popEffect.Hide();
                    return;
                }
                gameObject.SetActive(false);
            }
        }
    }
}
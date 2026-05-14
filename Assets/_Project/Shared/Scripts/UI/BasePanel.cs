using DG.Tweening;
using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Shared.UI
{
    public abstract class BasePanel<T> : BasePanel
    {
        public abstract void Setup(T data);
    }

    public abstract class BasePanel : MonoBehaviour
    {
        [Header("Base Panel Events")]
        [SerializeField] private UIPanelEvent _onPanelRegister;
        [SerializeField] private UIPanelEvent _onPanelShow;
        [SerializeField] private UIPanelEvent _onPanelHide;

        [Header("[Optional] Base Panel Settings")]
        [SerializeField] private BaseUIEffect _contentEffect;
        [SerializeField] protected CanvasGroup _backgroundCanvas;

        public virtual void Show()
        {
            _onPanelShow.Raise(this);

            if(_contentEffect == null || _backgroundCanvas == null)
            {
                // Nếu không có hiệu ứng hoặc background, chỉ cần bật GameObject
                if (_backgroundCanvas != null)
                    _backgroundCanvas.gameObject.SetActive(true);
                return;
            }

            _backgroundCanvas.gameObject.SetActive(true);
            _contentEffect.gameObject.SetActive(true);

            _backgroundCanvas.alpha = 0f;
            DOTween.To(() => _backgroundCanvas.alpha, x => _backgroundCanvas.alpha = x, 1f, _contentEffect.Duration).SetUpdate(true);

            _contentEffect.Show();
        }

        public virtual void Hide(System.Action onComplete = null)
        {
            _onPanelHide.Raise(this);

            if(_contentEffect == null || _backgroundCanvas == null)
            {
                // Nếu không có hiệu ứng hoặc background, chỉ cần tắt GameObject
                if (_backgroundCanvas != null)
                    _backgroundCanvas.gameObject.SetActive(false);
                onComplete?.Invoke();
                return;
            }

            DOTween.To(() => _backgroundCanvas.alpha, x => _backgroundCanvas.alpha = x, 0f, _contentEffect.Duration).SetUpdate(true).SetLink(gameObject);
            // Kêu Content ẩn đi, khi ẩn xong thì tắt GameObject cha
            _contentEffect.Hide(() =>
            {
                onComplete?.Invoke();
                _backgroundCanvas.gameObject.SetActive(false); // Tắt background
                _contentEffect.gameObject.SetActive(false); // Reset vị trí content
            });
        }

        protected virtual void Start()
        {
            _onPanelRegister.Raise(this);
        }
    }
}

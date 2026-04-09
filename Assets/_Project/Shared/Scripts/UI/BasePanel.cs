using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Shared
{
    public abstract class BasePanel<T> : BasePanel
    {
        public abstract void Setup(T data);
    }

    public abstract class BasePanel : MonoBehaviour
    {
        [SerializeField] private Button _closeButton;
        private PopScaleEffect _popScaleEffect;
        protected virtual void Awake()
        {
            // Tự đăng ký với UIManager
            UIManager.Instance.Register(this);
            gameObject.SetActive(false);

            //
            _popScaleEffect = GetComponent<PopScaleEffect>();
            if (_closeButton != null)
            {
                _closeButton.onClick.AddListener(() => _popScaleEffect.Hide());
            }
            else
            {
                _closeButton.onClick.AddListener(() => gameObject.SetActive(false));
            }
        }
    }
}

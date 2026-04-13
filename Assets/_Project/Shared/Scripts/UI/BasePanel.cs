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
        public abstract void Show();
        public abstract void Hide(System.Action onComplete = null);
    }
}

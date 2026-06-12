using UnityEngine;

namespace LabDiner.Shared.DesignPattern
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField] private bool dontDestroyOnLoad = false;
        private static T instance;

        public static T Instance => instance;

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this as T;

            if (dontDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
                transform.SetParent(null);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CoreStationToastUI : MonoBehaviour
    {

        [Header("Settings")]
        [SerializeField] private float _displayDuration = 1.5f;

        [Header("Pool Settings")]
        [SerializeField] private PopScaleEffect _toastPrefab; // Prefab mẫu
        [SerializeField] private Transform _container;     // Nơi chứa các toast (có Vertical Layout Group)
        [SerializeField] private int _poolSize = 2;

        private List<PopScaleEffect> _pool = new List<PopScaleEffect>();

        private void Awake()
        {
            // Khởi tạo sẵn một vài cái để dùng dần (Object Pooling)
            for (int i = 0; i < _poolSize; i++)
            {
                CreateNewInstance();
            }
        }

        private PopScaleEffect CreateNewInstance()
        {
            var instance = Instantiate(_toastPrefab, _container);
            instance.gameObject.SetActive(false);
            _pool.Add(instance);
            return instance;
        }

        public void Show(string data)
        {
            gameObject.SetActive(true);

            // Tìm một cái đang rảnh trong pool
            PopScaleEffect toast = _pool.Find(t => !t.gameObject.activeSelf);
            
            if (toast == null)
            {
                toast = CreateNewInstance();
            }

            // Thiết lập nội dung
            var txt = toast.GetComponentInChildren<TextMeshProUGUI>();
            if (txt != null) txt.text = data;

            // Hiển thị
            toast.gameObject.SetActive(true);
            
            // Ép Layout Group tính toán lại vị trí ngay lập tức nếu cần
            // Canvas.ForceUpdateCanvases(); 

            //effect tự động tắt
            toast.Show(() =>
            {
                StartCoroutine(DelayedHide(toast));
            });
        }

        IEnumerator DelayedHide(PopScaleEffect toast)
        {
            yield return new WaitForSeconds(_displayDuration);
            toast.Hide(() => toast.gameObject.SetActive(false));
        }
    }
}
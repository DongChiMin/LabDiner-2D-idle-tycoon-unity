using LabDiner.Restaurant.Pooling;
using LabDiner.Shared.Enum;
using LabDiner.Shared.Event;
using LabDiner.Shared.Extension;
using TMPro;
using UnityEngine;
using DG.Tweening;
using LabDiner.Shared.SO;

namespace LabDiner.Restaurant.UI
{
    public class CurrencyHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _gemText;
        [SerializeField] private DoubleRuntimeSO _coinRuntimeData;
        [SerializeField] private IntRuntimeSO _gemRuntimeData;

        [Header("Events")]
        [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;
        [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;

        void OnEnable()
        {
            _coinRuntimeData.OnValueChanged += UpdateCoinUI;
            _onCoinFlyAdded.Register(HandleCoinFlyAdded);

            _gemRuntimeData.OnValueChanged += UpdateGemUI;
            _onGemFlyAdded.Register(HandleGemFlyAdded);
        }

        void OnDisable()
        {
            _coinRuntimeData.OnValueChanged -= UpdateCoinUI;
            _onCoinFlyAdded.Unregister(HandleCoinFlyAdded);

            _gemRuntimeData.OnValueChanged -= UpdateGemUI;
            _onGemFlyAdded.Unregister(HandleGemFlyAdded);
            
            // Dừng các hiệu ứng scale nếu đang chạy
            _gemText.transform.parent.DOKill();
        }

        void Start()
        {
            _coinRuntimeData.Add(0); // Kích hoạt callback để cập nhật UI
        }

        private void UpdateCoinUI(double newCoinAmount)
        {
            _coinText.text = CurrencyFormatter.Format(newCoinAmount);
        }

        private void UpdateGemUI(int newGemAmount)
        {
            _gemText.text = _gemRuntimeData.Value.ToString();
        }

        private void HandleCoinFlyAdded(CoinRewardData data)
        {
            Vector3 startPos = data.startPos;
            Vector3 endPos = _coinText.transform.position; 
            
            // Biến flag để chỉ cập nhật text 1 lần duy nhất khi viên coin đầu tiên chạm đích
            bool hasUpdatedText = false;

            PoolContext.Instance.CurrencyFlyPool.SpawnFlyEffect(CurrencyType.Coin, startPos, endPos, data.RewardValue, () =>
            {
                // Khi viên coin đầu tiên chạm đích, cập nhật text ngay lập tức
                if (!hasUpdatedText)
                {
                    _coinRuntimeData.Add(data.RewardValue);
                    _coinText.text = CurrencyFormatter.Format(_coinRuntimeData.Value);
                    hasUpdatedText = true;
                }

                // Hiệu ứng "nảy" nhẹ HUD để tạo cảm giác phản hồi (Feedback)
                _coinText.transform.parent.DOKill(true);
                _coinText.transform.parent.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            });
        }

        private void HandleGemFlyAdded(GemRewardData data)
        {
            Vector3 startPos = data.startPos;
            Vector3 endPos = _gemText.transform.position; 
            
            // Biến flag để chỉ cập nhật text 1 lần duy nhất khi viên gem đầu tiên chạm đích
            bool hasUpdatedText = false;

            PoolContext.Instance.CurrencyFlyPool.SpawnFlyEffect(CurrencyType.Gem, startPos, endPos, data.RewardValue, () =>
            {
                // Khi viên gem đầu tiên chạm đích, cập nhật text ngay lập tức
                if (!hasUpdatedText)
                {
                    _gemRuntimeData.Add(data.RewardValue);
                    _gemText.text = CurrencyFormatter.Format(_gemRuntimeData.Value);
                    hasUpdatedText = true;
                }

                // Hiệu ứng "nảy" nhẹ HUD để tạo cảm giác phản hồi (Feedback)
                _gemText.transform.parent.DOKill(true);
                _gemText.transform.parent.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            });
        }
    }
}
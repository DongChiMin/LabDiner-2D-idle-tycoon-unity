using LabDiner.Restaurant.Pooling;
using LabDiner.Shared.Enum;
using LabDiner.Shared.Event;
using LabDiner.Shared.Extension;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace LabDiner.Restaurant.UI
{
    public class CurrencyHUD : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _gemText;

        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinUpdated;
        [SerializeField] private LevelCoinFlyEvent _onCoinFlyAdded;

        [SerializeField] private LevelGemEvent _onGemUpdated;
        [SerializeField] private LevelGemFlyEvent _onGemFlyAdded;

        // Xóa _tweenDuration và các biến _displayed nếu không còn nhu cầu dùng cho việc gì khác
        private double _currentCoin;
        private int _currentGem;

        void OnEnable()
        {
            _onCoinUpdated.Register(UpdateCoinUI);
            _onCoinFlyAdded.Register(HandleCoinFlyAdded);

            _onGemUpdated.Register(UpdateGemUI);
            _onGemFlyAdded.Register(HandleGemFlyAdded);
        }

        void OnDisable()
        {
            _onCoinUpdated.Unregister(UpdateCoinUI);
            _onCoinFlyAdded.Unregister(HandleCoinFlyAdded);

            _onGemUpdated.Unregister(UpdateGemUI);
            _onGemFlyAdded.Unregister(HandleGemFlyAdded);
            
            // Dừng các hiệu ứng scale nếu đang chạy
            _gemText.transform.parent.DOKill();
        }

        private void UpdateCoinUI(double newCoinAmount)
        {
            _currentCoin = newCoinAmount;
            _coinText.text = CurrencyFormatter.Format(_currentCoin);
        }

        private void UpdateGemUI(int newGemAmount)
        {
            _currentGem = newGemAmount;
            _gemText.text = _currentGem.ToString();
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
                    _currentCoin += data.RewardValue;
                    _coinText.text = CurrencyFormatter.Format(_currentCoin); 
                    hasUpdatedText = true;
                }

                // Hiệu ứng "nảy" nhẹ HUD để tạo cảm giác phản hồi (Feedback)
                _coinText.transform.parent.DOKill(true);
                _coinText.transform.parent.DOPunchScale(Vector3.one * 0.1f, 0.1f);

                //Thông báo coin đã được cập nhật sau khi hiệu ứng bay kết thúc
                _onCoinUpdated.Raise(_currentCoin);
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
                    _currentGem += data.RewardValue;
                    _gemText.text = CurrencyFormatter.Format(_currentGem); 
                    hasUpdatedText = true;
                }

                // Hiệu ứng "nảy" nhẹ HUD để tạo cảm giác phản hồi (Feedback)
                _gemText.transform.parent.DOKill(true);
                _gemText.transform.parent.DOPunchScale(Vector3.one * 0.1f, 0.1f);

                //Thông báo gem đã được cập nhật sau khi hiệu ứng bay kết thúc
                _onGemUpdated.Raise(_currentGem);
            });
        }
    }
}
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
        [SerializeField] private DoubleRuntimeSO _coinRuntimeData;
        [SerializeField] private IntRuntimeSO _gemRuntimeData;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private TextMeshProUGUI _gemText;

        void OnDisable()
        {
            // Dừng các hiệu ứng scale nếu đang chạy
            _gemText.transform.parent.DOKill();
        }

        //biến kiểm tra đã chạy xong animation bay chưa
        bool hasUpdatedCoinText = true;
        bool hasUpdatedGemText = true;

        public void RequestSetCoinText(double newCoinValue)
        {
            if(hasUpdatedCoinText) _coinText.text = CurrencyFormatter.Format(newCoinValue);
        }

        public void RequestSetGemText(int newGemValue)
        {
            if(hasUpdatedGemText) _gemText.text = CurrencyFormatter.Format(newGemValue);
        }

        public void PlayCoinFlyAnimation(CoinRewardData data)
        {
            Vector3 startPos = data.startPos;
            Vector3 endPos = _coinText.transform.position; 
            
            // Biến flag để chỉ cập nhật text 1 lần duy nhất khi viên coin đầu tiên chạm đích
            hasUpdatedCoinText = false;

            PoolContext.Instance.CurrencyFlyPool.SpawnFlyEffect(CurrencyType.Coin, startPos, endPos, data.RewardValue, () =>
            {
                // Khi viên coin đầu tiên chạm đích, cập nhật text ngay lập tức
                if (!hasUpdatedCoinText)
                {
                    _coinText.text = CurrencyFormatter.Format(_coinRuntimeData.Value);
                    hasUpdatedCoinText = true;
                }

                // Hiệu ứng "nảy" nhẹ HUD để tạo cảm giác phản hồi (Feedback)
                _coinText.transform.parent.DOKill(true);
                _coinText.transform.parent.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            });
        }

        public void PlayGemFlyAnimation(GemRewardData data)
        {
            Vector3 startPos = data.startPos;
            Vector3 endPos = _gemText.transform.position; 

            // Biến flag để chỉ cập nhật text 1 lần duy nhất khi viên gem đầu tiên chạm đích
            hasUpdatedGemText = false;

            PoolContext.Instance.CurrencyFlyPool.SpawnFlyEffect(CurrencyType.Gem, startPos, endPos, data.RewardValue, () =>
            {
                // Khi viên gem đầu tiên chạm đích, cập nhật text ngay lập tức
                if (!hasUpdatedGemText)
                {
                    _gemText.text = CurrencyFormatter.Format(_gemRuntimeData.Value);
                    hasUpdatedGemText = true;
                }

                // Hiệu ứng "nảy" nhẹ HUD để tạo cảm giác phản hồi (Feedback)
                _gemText.transform.parent.DOKill(true);
                _gemText.transform.parent.DOPunchScale(Vector3.one * 0.1f, 0.1f);
            });
        }
    }
}
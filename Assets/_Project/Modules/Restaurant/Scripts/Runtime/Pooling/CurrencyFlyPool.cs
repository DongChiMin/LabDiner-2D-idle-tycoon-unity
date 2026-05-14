using LabDiner.Shared.DesignPattern;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;
using LabDiner.Shared.Enum;

namespace LabDiner.Restaurant.Pooling
{
    public class CurrencyFlyPool : SceneObjectPooling<Image>
    {
        [Header("Currency Settings")]
        [SerializeField] private Sprite _gemSprite;
        [SerializeField] private Sprite _coinSprite;

        [Header("Animation Settings")]
        [SerializeField] private float _scatterRadius = 150f;     // Bán kính tỏa ra lúc bùng nổ (tùy thuộc vào độ phân giải Canvas)
        [SerializeField] private float _scatterDuration = 0.4f;   // Thời gian tỏa ra
        [SerializeField] private float _flyDuration = 0.6f;       // Thời gian bay về đích
        [SerializeField] private float _maxRandomDelay = 0.15f;   // Độ trễ ngẫu nhiên để các coin không bay cùng 1 lúc y hệt nhau

        /// <summary>
        /// API để kích hoạt hiệu ứng bay
        /// </summary>
        public void SpawnFlyEffect(CurrencyType currencyType, Vector3 startPos, Vector3 targetPos, int amount, Action onOneGemReached = null)
        {
            // Giới hạn số lượng hiển thị để không tràn màn hình (Visual only)
            int visualAmount = Mathf.Min(amount, 15);

            for (int i = 0; i < visualAmount; i++)
            {
                // Sử dụng hàm Get() từ class cha (SceneObjectPooling)
                Image gem = Get(startPos, Quaternion.identity);

                // Thiết lập sprite dựa trên loại tiền
                gem.sprite = currencyType == CurrencyType.Gem ? _gemSprite : _coinSprite;

                // Thực hiện hiệu ứng
                AnimateGem(gem, targetPos, onOneGemReached);
            }
        }

        private void AnimateGem(Image gem, Vector3 targetPos, Action onComplete)
        {
            gem.transform.DOKill();
            
            // 0. Reset trạng thái ban đầu của obj lấy từ Pool
            gem.transform.localScale = Vector3.zero;
            gem.transform.rotation = Quaternion.identity;
            // Nếu bạn có xử lý màu sắc mờ đi thì nhớ reset color: gem.color = Color.white;

            // Tạo một Sequence mới của DOTween
            Sequence sequence = DOTween.Sequence();

            // Tính toán vị trí ngẫu nhiên để coin bùng nổ tỏa ra xung quanh
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * _scatterRadius;
            Vector3 scatterPos = gem.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
            
            // Xoay ngẫu nhiên một góc để trông tự nhiên hơn
            Vector3 randomRotation = new Vector3(0, 0, UnityEngine.Random.Range(-180f, 180f));

            // Thêm một chút delay ngẫu nhiên để cả cụm coin không chuyển động quá đồng đều
            float delay = UnityEngine.Random.Range(0f, _maxRandomDelay);
            sequence.SetDelay(delay);

            // --- GIAI ĐOẠN 1: BÙNG NỔ (SCATTER) ---
            // Scale từ 0 lên 1 tạo cảm giác pop up
            sequence.Append(gem.transform.DOScale(Vector3.one, _scatterDuration).SetEase(Ease.OutBack).SetLink(gameObject));
            // Cùng lúc đó, di chuyển tới vị trí tỏa ra và xoay
            sequence.Join(gem.transform.DOMove(scatterPos, _scatterDuration).SetEase(Ease.OutCubic).SetLink(gameObject));
            sequence.Join(gem.transform.DORotate(randomRotation, _scatterDuration).SetEase(Ease.OutCubic).SetLink(gameObject));

            // Dừng lại trên không một khoảng thời gian cực ngắn để tạo cảm giác "treo lơ lửng" (hang time)
            sequence.AppendInterval(0.05f);

            // --- GIAI ĐOẠN 2: HỘI TỤ VỀ ĐÍCH (FLY TO TARGET) ---
            // Sử dụng InBack để đồng tiền hơi lùi lại một chút tạo đà, rồi lao vút đi
            sequence.Append(gem.transform.DOMove(targetPos, _flyDuration).SetEase(Ease.InBack).SetLink(gameObject));
            // Thu nhỏ lại một chút và trả góc xoay về 0 khi bay gần tới đích
            sequence.Join(gem.transform.DOScale(Vector3.one * 0.8f, _flyDuration).SetLink(gameObject));
            sequence.Join(gem.transform.DORotate(Vector3.zero, _flyDuration).SetLink(gameObject));

            // --- GIAI ĐOẠN 3: KẾT THÚC ---
            sequence.OnComplete(() =>
            {
                // Gọi callback (ví dụ để update Text tiền, phát âm thanh ting ting...)
                onComplete?.Invoke();

                ReturnToPool(gem); 
            });
        }
    }
}
using LabDiner.Shared.DesignPattern;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System;
using LabDiner.Shared.Enum;

namespace LabDiner.Restaurant.Pooling
{
    public class CurrencyBurstPool : SceneObjectPooling<Image>
    {
        [Header("Currency Settings")]
        [SerializeField] private Sprite _gemSprite;
        [SerializeField] private Sprite _coinSprite;

        [Header("Burst Settings")]
        [SerializeField] private float _burstScatterRadius = 150f;     // Bán kính tỏa ra khi burst
        [SerializeField] private float _burstScatterDuration = 0.4f;   // Thời gian tỏa ra
        [SerializeField] private float _maxRandomDelay = 0.15f;   // Độ trễ ngẫu nhiên để các coin không bay cùng 1 lúc y hệt nhau
        [SerializeField] private Ease _burstScatterEase = Ease.OutCubic; // Kiểu chuyển động khi tỏa ra
        [SerializeField] private Ease _burstFadeEase = Ease.InQuad;    // Kiểu chuyển động khi rơi xuống và mờ dần
        [SerializeField] private float _coinScale = 0.6f;
    
       /// <summary>
        /// API để kích hoạt hiệu ứng coin tỏa ra xung quanh rồi mờ dần và biến mất
        /// </summary>
        public void SpawnBurstEffect(CurrencyType currencyType, Vector3 startPos, double amount, bool isWorldSpace = false, Action onComplete = null)
        {
            // Chuyển đổi tọa độ nếu đầu vào là World Space
            Vector3 spawnPos = GetCanvasPosition(startPos, isWorldSpace);

            // Giới hạn số lượng hiển thị để tối ưu hiệu năng
            int visualAmount = Mathf.Min((int)amount, 15);

            for (int i = 0; i < visualAmount; i++)
            {
                // Sử dụng vị trí đã được convert (spawnPos) thay vì startPos gốc
                Image coin = Get(spawnPos, Quaternion.identity);

                // Thiết lập sprite dựa trên loại tiền
                coin.sprite = currencyType == CurrencyType.Gem ? _gemSprite : _coinSprite;

                // Thực hiện hiệu ứng tỏa ra và biến mất
                AnimateBurstCoin(coin, onComplete);
            }
        }

        private void AnimateBurstCoin(Image coin, Action onComplete)
        {
            coin.transform.DOKill();
            
            // 0. Reset trạng thái ban đầu của coin (bắt đầu nhỏ gọn từ tâm với kích thước _coinScale)
            coin.transform.localScale = Vector3.one * _coinScale; 
            coin.transform.rotation = Quaternion.identity;
            
            Color initialColor = coin.color;
            initialColor.a = 1f; // Bắt đầu hiện rõ hoàn toàn
            coin.color = initialColor;

            // Tạo Sequence mới của DOTween
            Sequence sequence = DOTween.Sequence();

            // Tính toán vị trí ngẫu nhiên để coin bùng nổ tỏa ra
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle * _burstScatterRadius;
            Vector3 scatterPos = coin.transform.position + new Vector3(randomOffset.x, randomOffset.y, 0f);
            
            // Xoay ngẫu nhiên
            Vector3 randomRotation = new Vector3(0, 0, UnityEngine.Random.Range(-180f, 180f));

            // Delay ngẫu nhiên nhẹ để tạo hiệu ứng tự nhiên
            float delay = UnityEngine.Random.Range(0f, _maxRandomDelay);
            sequence.SetDelay(delay);

            // --- HIỆU ỨNG NỔ: ĐỒNG THỜI BẮT RA, XOAY, MỜ DẦN VÀ THU NHỎ KHI CÀNG XA ---
            // 1. Bay ra xa và xoay
            sequence.Append(coin.transform.DOMove(scatterPos, _burstScatterDuration).SetEase(_burstScatterEase).SetLink(gameObject));
            sequence.Join(coin.transform.DORotate(randomRotation, _burstScatterDuration).SetEase(_burstScatterEase).SetLink(gameObject));
            
            // 2. Đồng thời mờ dần (alpha về 0) từ tâm ra tới biên
            sequence.Join(DOTween.To(() => coin.color, x => coin.color = x, new Color(initialColor.r, initialColor.g, initialColor.b, 0f), _burstScatterDuration)
                .SetEase(_burstFadeEase)
                .SetLink(gameObject));
                
            // 3. Đồng thời thu nhỏ dần về 0 khi bay ra xa để tạo cảm giác tan biến hẳn
            sequence.Join(coin.transform.DOScale(Vector3.zero, _burstScatterDuration).SetEase(_burstFadeEase).SetLink(gameObject));

            // --- GIAI ĐOẠN 3: KẾT THÚC & TRẢ VỀ POOL ---
            sequence.OnComplete(() =>
            {
                // Trả lại màu và scale gốc để lần sau lấy ra dùng không bị lỗi
                coin.color = initialColor;
                coin.transform.localScale = Vector3.one; 
                
                onComplete?.Invoke();
                ReturnToPool(coin);
            });
        }

        /// <summary>
        /// Chuyển đổi tọa độ từ World Space sang vị trí cục bộ (Local Position) trong Canvas của UI Pool
        /// </summary>
        private Vector3 GetCanvasPosition(Vector3 worldPos, bool isWorldSpace)
        {
            if (!isWorldSpace) return worldPos; // Nếu vốn dĩ là UI position rồi thì giữ nguyên

            // Lấy Canvas gốc chứa pool này (hoặc có thể trỏ trực tiếp đến Canvas của bạn)
            Canvas canvas = GetComponentInParent<Canvas>();
            if (canvas == null) return worldPos;

            Camera worldCamera = Camera.main;
            
            // Nếu Canvas là Overlay thì camera truyền vào thường là null, nếu là Screen Space - Camera thì dùng camera của canvas
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                worldCamera = canvas.worldCamera;
            }

            // 1. Chuyển từ World Position sang Screen Position (Pixel màn hình)
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(worldCamera, worldPos);

            // 2. Chuyển từ Screen Position sang Local Position của RectTransform chứa Pool
            RectTransform canvasRect = canvas.transform as RectTransform;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : worldCamera, out Vector2 localPoint))
            {
                return canvasRect.TransformPoint(localPoint); // Trả về tọa độ World chuẩn của UI hoặc local tùy ý
            }

            return worldPos;
        }
    }
}
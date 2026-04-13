
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class CoreStationStarUI : MonoBehaviour
    {
        [SerializeField] private Image _starPrefab;
        [SerializeField] private Sprite _activeStarSprite;
        [SerializeField] private Sprite _inactiveStarSprite;
        private List<Image> _spawnedStars = new List<Image>();

        public void Setup(int currentStars, int maxStars)
        {
            // Nếu số lượng sao đã sinh ra khác với yêu cầu, mới cần xử lý
            if (_spawnedStars.Count != maxStars) GenerateStars(maxStars);
            
            UpdateRating(currentStars);
        }

        public void UpdateRating(int currentStars)
        {
            for (int i = 0; i < _spawnedStars.Count; i++)
            {
                // Nếu vị trí của sao nhỏ hơn số sao hiện tại thì "Sáng", ngược lại "Tối"
                _spawnedStars[i].sprite = (i < currentStars) ? _activeStarSprite : _inactiveStarSprite;
                _spawnedStars[i].color = (i < currentStars) ? Color.blue : new Color(1, 1, 1, 0.3f); // Ví dụ: sao không đạt được sẽ mờ đi
            }
        }

        private void GenerateStars(int quantity)
        {
            // Dọn dẹp nếu đã có sao cũ (Trường hợp đổi từ bếp 3 sao sang bếp 5 sao)
            foreach (var star in _spawnedStars) Destroy(star.gameObject);
                _spawnedStars.Clear();

            for (int i = 0; i < quantity; i++)
            {
                Image newStar = Instantiate(_starPrefab, transform);
                _spawnedStars.Add(newStar);
            }
        }
    }
}
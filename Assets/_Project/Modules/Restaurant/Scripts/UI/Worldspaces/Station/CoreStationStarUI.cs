using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class CoreStationStarUI : MonoBehaviour
    {
        [Header("Star Settings")]
        [SerializeField] private Image _starPrefab;
        [SerializeField] private Sprite _activeStarSprite;
        [SerializeField] private Sprite _inactiveStarSprite;
        [SerializeField] private Transform _starContainer;

        [Header("Reward Settings")]
        [SerializeField] private Image _rewardPrefab;
        [SerializeField] private Transform _rewardContainer;

        [Header("ProgressBar Settings")]
        [SerializeField] private Slider _starProgressFill;

        private List<Image> _spawnedStars = new List<Image>();
        private List<Image> _spawnedRewards = new List<Image>();
        private bool _isMaxLevel = false;

        public void Setup(CoreStationUIData data)
        {
            if(_isMaxLevel)
            {
                return;
            }

            int currentStars = data.StarQuantity;
            int maxStars = data.MaxStar;
            _starProgressFill.value = data.StarProgress;
            // Nếu số lượng sao đã sinh ra khác với yêu cầu, mới cần xử lý
            if (_spawnedStars.Count != maxStars)
            {
                GenerateStars(maxStars);
                GenerateRewards(data.StarRewardIcons);
            }
            
            UpdateRating(currentStars);
        }

        public void MaxLevelReached(int maxStar)
        {
            _isMaxLevel = true;
            UpdateRating(maxStar);
            _starProgressFill.value = 1f;
        }

        public void UpdateRating(int currentStars)
        {
            for (int i = 0; i < _spawnedStars.Count; i++)
            {
                // Nếu vị trí của sao nhỏ hơn số sao hiện tại thì "Sáng", ngược lại "Tối"
                _spawnedStars[i].sprite = (i < currentStars) ? _activeStarSprite : _inactiveStarSprite;
            }
        }

        private void GenerateStars(int quantity)
        {
            // Dọn dẹp nếu đã có sao cũ (Trường hợp đổi từ bếp 3 sao sang bếp 5 sao)
            foreach (var star in _spawnedStars) Destroy(star.gameObject);
                _spawnedStars.Clear();
            
            //dọn dẹp container
                foreach (Transform child in _starContainer)
                {
                    Destroy(child.gameObject);
                }

            for (int i = 0; i < quantity; i++)
            {
                Image newStar = Instantiate(_starPrefab, _starContainer);
                _spawnedStars.Add(newStar);
            }
        }

        private void GenerateRewards(List<Sprite> rewardIcons)
        {
            // Nếu số lượng phần thưởng đã sinh ra khác với yêu cầu, mới cần destroy và tạo lại
            if(rewardIcons.Count != _spawnedRewards.Count)
            {
                // Destroy phần thưởng cũ nếu có
                foreach (var reward in _spawnedRewards) Destroy(reward.gameObject);
                _spawnedRewards.Clear();

                //dọn dẹp container
                foreach (Transform child in _rewardContainer)
                {
                    Destroy(child.gameObject);
                }

                //Sinh ra prefab mới
                foreach (var icon in rewardIcons)
                {
                    Image newReward = Instantiate(_rewardPrefab, _rewardContainer);
                    newReward.sprite = icon;
                    _spawnedRewards.Add(newReward);
                }
            }
            else
            {
                //Sửa các prefab đã có sẵn
                for(int i = 0; i < rewardIcons.Count; i++)
                {
                    _spawnedRewards[i].sprite = rewardIcons[i];
                }
            }

            
        }

    }
}

using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class ReputationBarHUD : MonoBehaviour
    {
        [SerializeField] private ReputationEvent _onReputationChanged;
        [SerializeField] private Image _fillImage;
        [Header("[DEBUG]")]
        [SerializeField] private float _currentReputationRatio;

        void OnEnable()
        {
            _onReputationChanged.Register(UpdateReputation);
        }

        void OnDisable()
        {
            _onReputationChanged.Unregister(UpdateReputation);
        }

        public void UpdateReputation(float ratio)
        {
            _fillImage.fillAmount = ratio;
        }
    }
}
        
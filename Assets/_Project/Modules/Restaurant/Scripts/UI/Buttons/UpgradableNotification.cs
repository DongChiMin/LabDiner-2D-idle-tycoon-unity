using LabDiner.Restaurant.Event;
using LabDiner.Shared.Event;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public class UpgradableNotification : MonoBehaviour
    {
        [SerializeField] private LevelUpgradableEvent _onLevelUpgradable;
        [SerializeField] private PopScaleEffect _popScaleEffect;
        [SerializeField] private AttentionEffect _attentionEffect;
        private bool isOn = false;

        void OnEnable()
        {
            _onLevelUpgradable.Register(HandleLevelUpgradable);
        }

        void OnDisable()
        {
            _onLevelUpgradable.Unregister(HandleLevelUpgradable);
        }

        void Awake()
        {
            _popScaleEffect.gameObject.SetActive(false);
        }

        private void HandleLevelUpgradable(bool canUpgrade)
        {
            if (canUpgrade && !isOn)
            {
                isOn = true;
                _popScaleEffect.gameObject.SetActive(true);
                _popScaleEffect.Show(() =>
                {
                    
                });
            }
            else if (!canUpgrade && isOn)
            {
                isOn = false;
                _popScaleEffect.Hide(() =>
                {
                    _popScaleEffect.gameObject.SetActive(false);
                });
            }
        }
    }
}

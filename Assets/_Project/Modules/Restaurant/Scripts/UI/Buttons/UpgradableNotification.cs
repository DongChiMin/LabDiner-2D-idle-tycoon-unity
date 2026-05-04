using LabDiner.Restaurant.Event;
using LabDiner.Shared.Event;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public class UpgradableNotification : MonoBehaviour
    {
        [SerializeField] private LevelUpgradableEvent _onLevelUpgradable;
        [SerializeField] private AttentionEffect _attentionEffect;
        [SerializeField] private PopScaleEffect _popScaleEffect;
        private bool _isOn = true;
        private GameObject _upgradeIconInstance;

        void Awake()
        {
            _upgradeIconInstance = _attentionEffect.gameObject;
        }

        void Start()
        {
            _isOn = _upgradeIconInstance.activeSelf;
        }

        void OnEnable()
        {
            _onLevelUpgradable.Register(HandleLevelUpgradable);
        }

        void OnDisable()
        {
            _onLevelUpgradable.Unregister(HandleLevelUpgradable);
        }

        private void HandleLevelUpgradable(bool canUpgrade)
        {
            if (canUpgrade && !_isOn)
            {
                _upgradeIconInstance.SetActive(true);
                _popScaleEffect.Show(() =>
                {
                    _attentionEffect.enabled = true;
                    _popScaleEffect.enabled = false;
                    _isOn = true;
                });
            }
            else if(!canUpgrade && _isOn)
            {
                _popScaleEffect.enabled = true;
                _attentionEffect.enabled = false;
                _popScaleEffect.Hide(() =>
                {
                    _upgradeIconInstance.SetActive(false);
                    _isOn = false;
                });
            }
        }
    }
}

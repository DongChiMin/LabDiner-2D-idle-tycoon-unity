using LabDiner.Restaurant.Event;
using LabDiner.Shared.Event;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.Restaurant.UI
{
    public class UpgradableNotification : MonoBehaviour
    {
        [SerializeField] private LevelUpgradableEvent _onLevelUpgradable;
        [SerializeField] private PopScaleEffect _upgradableIcon;

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
            _upgradableIcon.gameObject.SetActive(false);
        }

        private void HandleLevelUpgradable(bool canUpgrade)
        {
            if (canUpgrade)
            {
                _upgradableIcon.gameObject.SetActive(true);
                _upgradableIcon.Show();
            }
            else
            {
                _upgradableIcon.Hide(() =>
                {
                    _upgradableIcon.gameObject.SetActive(false);
                });
            }
        }
    }
}

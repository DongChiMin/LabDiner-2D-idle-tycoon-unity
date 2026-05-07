using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class LevelUpgradeController : MonoBehaviour, ILevelInitializable
    {
        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinUpdated;
        [SerializeField] private LevelCoinEvent _onCoinSpent;
        [SerializeField] private LevelUpgradableEvent _onLevelUpgradable;
        [SerializeField] private UIPopupEvent _onPopupShow;

        [Header("Item References")]
        [SerializeField] private LevelUpgradeItem _levelUpgradeItemPrefab;
        [SerializeField] private Transform _itemParent;
        [SerializeField] private LevelUpgradePanel _panel;

        private List<LevelUpgradeItem> upgradeItems = new();

        void OnEnable()
        {
            _panel.CloseButton.onClick.AddListener(HandlePopupHide);

            _onPopupShow.Register(HandlePopupShow);
            _onCoinUpdated.Register(HandleCoinUpdated);
        }

        void OnDisable()
        {
            _panel.CloseButton.onClick.RemoveListener(HandlePopupHide);

            _onPopupShow.Unregister(HandlePopupShow);
            _onCoinUpdated.Unregister(HandleCoinUpdated);
        }

        #region API

        public void Init(LevelConfigSO levelConfigSO)
        {
            List<BaseUpgradeSO> baseUpgradeSOs = levelConfigSO.AvailableUpgrades;
            double levelCoin = LevelManagerContext.Instance.LevelCurrencyManager.CurrentCoin;
            
            foreach (Transform child in _itemParent)
            {
                Destroy(child.gameObject);
            }
            
            //Tạo các prefab, gán data
            for(int i = 0; i < baseUpgradeSOs.Count; i++)
            {
                BaseUpgradeSO data = baseUpgradeSOs[i];
                LevelUpgradeItem item = Instantiate(_levelUpgradeItemPrefab, _itemParent);
                item.Init(data);

                Button upgradeButton = item.UpgradeButton;
                upgradeButton.onClick.AddListener(() => 
                {
                    _onCoinSpent.Raise(data.UpgradeCost);
                    data.ApplyUpgrade();
                    item.gameObject.SetActive(false);
                });

                upgradeItems.Add(item);
            }
        }

        #endregion

        private void HandlePopupShow()
        {
            double levelCoin = LevelManagerContext.Instance.LevelCurrencyManager.CurrentCoin;
            HandleCoinUpdated(levelCoin);

            _panel.Show();
        }

        private void HandlePopupHide()
        {
            _panel.Hide();
        }

        private void HandleCoinUpdated(double newCoinValue)
        {
            bool canUpgrade = false;
            foreach(LevelUpgradeItem item in upgradeItems)
            {
                bool isBuyable = item.UpgradeSO.UpgradeCost <= newCoinValue;
                item.ToggleUpgradeButton(isBuyable);
                if (isBuyable)
                    canUpgrade = true;
            }
            
            _onLevelUpgradable.Raise(canUpgrade);
        }

    }
}

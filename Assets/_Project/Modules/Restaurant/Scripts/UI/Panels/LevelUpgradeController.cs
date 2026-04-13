using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class LevelUpgradeController : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private LevelCoinEvent _onCoinUpdated;
        [SerializeField] private LevelCoinEvent _onCoinSpent;
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

            Debug.Log("Show Level Upgrade Panel");

            _panel.Show();
        }

        private void HandlePopupHide()
        {
            _panel.Hide();
        }

        private void HandleCoinUpdated(double newCoinValue)
        {
            foreach(LevelUpgradeItem item in upgradeItems)
            {
                bool isBuyable = item.UpgradeSO.UpgradeCost <= newCoinValue;
                item.ToggleUpgradeButton(isBuyable);
            }
        }

    }
}

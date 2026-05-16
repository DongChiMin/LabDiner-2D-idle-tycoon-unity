using System;
using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using LabDiner.Shared.SO;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class LevelUpgradeController : MonoBehaviour, ILevelInitializable
    {
        [Header("Data")]
        [SerializeField] private DoubleRuntimeSO _coinData;
        [SerializeField] private LevelUpgradeRuntimeSO _levelUpgradeRuntimeSO;

        [Header("Events")]
        [SerializeField] private LevelConfigEvent _onLevelInit;
        [SerializeField] private BoolEvent _onLevelUpgradable;
        [SerializeField] private UIPopupEvent _onPopupShow;

        [Header("Item References")]
        [SerializeField] private LevelUpgradeItem _levelUpgradeItemPrefab;
        [SerializeField] private Transform _itemParent;
        [SerializeField] private LevelUpgradePanel _panel;

        private List<LevelUpgradeItem> upgradeItems = new();

        void OnEnable()
        {
            _panel.CloseButton.onClick.AddListener(HandlePopupHide);
            _onLevelInit.Register(Init);
            _onPopupShow.Register(HandlePopupShow);
            _coinData.OnValueChanged += HandleCoinUpdated;
        }

        void OnDisable()
        {
            _panel.CloseButton.onClick.RemoveListener(HandlePopupHide);
            _onLevelInit.Unregister(Init);
            _onPopupShow.Unregister(HandlePopupShow);
            _coinData.OnValueChanged -= HandleCoinUpdated;
        }

        #region API

        public void Init(LevelConfigSO levelConfigSO)
        {
            _levelUpgradeRuntimeSO.Clear();
            List<BaseUpgradeSO> baseUpgradeSOs = levelConfigSO.AvailableUpgrades;
            double levelCoin = _coinData.Value;

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
                    _coinData.Add(-data.UpgradeCost);
                    data.ApplyUpgrade();
                    item.gameObject.SetActive(false);
                    _levelUpgradeRuntimeSO.Complete(data);
                });

                upgradeItems.Add(item);
                _levelUpgradeRuntimeSO.Add(data);
            }
        }

        #endregion

        private void HandlePopupShow()
        {
            double levelCoin = _coinData.Value;
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

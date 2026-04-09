using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelUpgradePanel : BasePanel<int>
    {
        [SerializeField] private LevelUpgradeItem levelUpgradeItemPrefab;
        [SerializeField] private Transform contentParent;
        private List<BaseUpgradeSO> baseUpgradeSOs;

        public override void Setup(int a)
        {
        }

        #region API
        
        public void Init(LevelConfigSO levelConfigSO)
        {
            baseUpgradeSOs = levelConfigSO.AvailableUpgrades;

            foreach (Transform child in contentParent)
            {
                Destroy(child.gameObject);
            }
            
            for(int i = 0; i < baseUpgradeSOs.Count; i++)
            {
                var item = Instantiate(levelUpgradeItemPrefab, contentParent);
                item.Init(baseUpgradeSOs[i]);
            }
        }

        #endregion


    }
}

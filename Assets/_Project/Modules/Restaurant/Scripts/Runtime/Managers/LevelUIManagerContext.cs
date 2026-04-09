using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelUIManagerContext : MonoBehaviour
    {
        [SerializeField] private LevelConfigSO levelConfigSO;
        [SerializeField] private LevelUpgradePanel levelUpgradePanel;

        void Awake()
        {
            levelUpgradePanel.Init(levelConfigSO);
        }
    }
}

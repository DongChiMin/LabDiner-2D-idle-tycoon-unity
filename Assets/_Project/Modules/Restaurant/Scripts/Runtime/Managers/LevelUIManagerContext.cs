using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class LevelUIManagerContext : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private LevelUpgradeController levelUpgradeController;
        [SerializeField] private LevelMissionController levelMissionController;

        public void Init(LevelConfigSO config)
        {
            levelUpgradeController.Init(config);
            levelMissionController.Init(config);
        }
    }
}

using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelUIManagerContext : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private LevelUpgradeController levelUpgradeController;

        public void Init(LevelConfigSO config)
        {
            levelUpgradeController.Init(config);
        }
    }
}

using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelUIManagerContext : MonoBehaviour
    {
        [SerializeField] private LevelConfigSO levelConfigSO;
        [SerializeField] private LevelUpgradeController levelUpgradeController;

        void Awake()
        {
            levelUpgradeController.Init(levelConfigSO);
        }
    }
}

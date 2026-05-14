using UnityEngine;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;

namespace LabDiner.LevelSystem.Domain
{
    [CreateAssetMenu(fileName = "LevelRegistry", menuName = "SO/Level/LevelRegistry")]
    public class LevelRegistrySO : ScriptableObject
    {
        public List<LevelConfigSO> registry;

        public LevelConfigSO GetConfigByLevel(int levelIndex)
        {
            foreach(LevelConfigSO config in registry)
            {
                if (config.LevelIndex == levelIndex)
                {
                    return config;
                }
            }
            Debug.LogError($"[LevelRegistrySO] No config found for level {levelIndex}!");
            return null;
        }
    }
}
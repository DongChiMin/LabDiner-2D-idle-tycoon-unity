using UnityEngine;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;

namespace LabDiner.LevelSystem.Domain
{
    [CreateAssetMenu(fileName = "LevelRegistry", menuName = "SO/Level/LevelRegistry")]
    public class LevelRegistrySO : ScriptableObject
    {
        public List<LevelConfigSO> Levels => GetAllConfigs();
        [SerializeField] private List<LevelChapterSO> chapters;

        public LevelConfigSO GetFirstLevelConfigSO()
        {
            if (chapters.Count == 0 || chapters[0].Levels.Count == 0)
            {
                Debug.LogError("[LevelRegistrySO] No chapters or levels found in the registry!");
                return null;
            }
            return chapters[0].Levels[0];
        }

        public LevelChapterSO GetChapterByLevelConfig(LevelConfigSO config)
        {
            foreach (LevelChapterSO chapter in chapters)
            {
                if (chapter.Levels.Contains(config))
                {
                    return chapter;
                }
            }
            Debug.LogError($"[LevelRegistrySO] No chapter found for level {config.LevelIndex}!");
            return null;
        }

        public List<LevelConfigSO> GetLevelConfigsByChapter(LevelChapterSO chapter)
        {
            if (chapter == null)
            {
                Debug.LogError("[LevelRegistrySO] Chapter is null!");
                return new List<LevelConfigSO>();
            }
            return chapter.Levels;
        }

        public LevelConfigSO GetConfigByID(string ID)
        {
            foreach(LevelChapterSO chapter in chapters)
            {
                foreach(LevelConfigSO config in chapter.Levels)
                {
                    if (config.ID == ID)
                    {
                        return config;
                    }
                }
            }
            Debug.LogError($"[LevelRegistrySO] No config found for level ID {ID}!");
            return null;
        }

        public LevelConfigSO GetNextLevelConfigSO(LevelConfigSO currentConfig)
        {
            if (currentConfig == null)
            {
                Debug.LogError("[LevelRegistrySO] Current config is null!");
                return null;
            }

            for (int i = 0; i < chapters.Count; i++)
            {
                LevelChapterSO chapter = chapters[i];
                for (int j = 0; j < chapter.Levels.Count; j++)
                {
                    LevelConfigSO config = chapter.Levels[j];
                    if (config == currentConfig)
                    {
                        // Found the current level, now get the next one
                        if (j + 1 < chapter.Levels.Count)
                        {
                            return chapter.Levels[j + 1]; // Next level in the same chapter
                        }
                        else if (i + 1 < chapters.Count)
                        {
                            return chapters[i + 1].Levels[0]; // First level of the next chapter
                        }
                        else
                        {
                            Debug.Log("[LevelRegistrySO] This is the last level. No next level available.");
                            return null; // No next level available
                        }
                    }
                }
            }
            Debug.LogError($"[LevelRegistrySO] No config found for level {currentConfig.LevelIndex}!");
            return null;
        }

        private List<LevelConfigSO> GetAllConfigs()
        {
            List<LevelConfigSO> allConfigs = new List<LevelConfigSO>();
            foreach (LevelChapterSO chapter in chapters)
            {
                allConfigs.AddRange(chapter.Levels);
            }
            return allConfigs;
        }
    }
}
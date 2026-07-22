using UnityEngine;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;

namespace LabDiner.LevelSystem.Domain
{
    [CreateAssetMenu(fileName = "LevelChapter", menuName = "SO/Level/LevelChapter")]
    public class LevelChapterSO : ScriptableObject
    {
        public string ChapterID;
        public int ChapterIndex;
        public string ChapterName;
        public List<LevelConfigSO> Levels;
    }
}
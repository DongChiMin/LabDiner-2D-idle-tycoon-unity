using UnityEngine;
using LabDiner.Shared.SO;

namespace LabDiner.Shared
{
    public interface ILevelInitializable
    {
        public void Init(LevelConfigSO config);
    }
}
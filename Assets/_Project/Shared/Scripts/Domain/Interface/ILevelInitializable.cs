using UnityEngine;

namespace LabDiner.Shared
{
    public interface ILevelInitializable
    {
        public void Init(LevelConfigSO config);
    }
}
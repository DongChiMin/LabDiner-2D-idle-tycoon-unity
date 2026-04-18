using System.Collections.Generic;
using UnityEngine;
using LabDiner.Shared.SO;

namespace LabDiner.Shared
{
        public class GameBootstrap : MonoBehaviour
    {
        [SerializeField] private LevelConfigSO _levelConfigSO;
        [SerializeField] private List<MonoBehaviour> _managers = new List<MonoBehaviour>();

        private void Start()
        {            
            foreach (var obj in _managers)
            {
                if (obj is ILevelInitializable system)
                {
                    system.Init(_levelConfigSO);
                }
                else
                {
                    Debug.LogWarning($"Object {obj.name} does not implement ILevelInitializable");
                }
            }
        }
    }
}

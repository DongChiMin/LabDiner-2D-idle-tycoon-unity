using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using NavMeshPlus.Components;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class AreaBakeManager : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        public void Init(LevelConfigSO config)
        {
            _navMeshSurface.BuildNavMesh();

            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}

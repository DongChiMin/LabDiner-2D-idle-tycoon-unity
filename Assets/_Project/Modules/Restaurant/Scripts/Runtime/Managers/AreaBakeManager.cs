using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using NavMeshPlus.Components;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class AreaBakeManager : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private NavMeshSurface _navMeshSurface;

        public void Init(LevelConfigSO config)
        {
            Debug.unityLogger.logEnabled = false;
            Physics2D.SyncTransforms();
            _navMeshSurface.BuildNavMesh();
            Debug.unityLogger.logEnabled = true;

            foreach(Transform child in transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}

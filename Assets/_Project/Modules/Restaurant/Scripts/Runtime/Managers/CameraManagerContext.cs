using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CameraManagerContext : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private CameraController cameraController;

        public void Init(LevelConfigSO config)
        {
            Debug.Log("CameraManagerContext Init with config: " + config.name);
            cameraController.Init(config);
        }
    }
}

using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CameraManagerContext : MonoBehaviour, ILevelInitializable
    {
        [SerializeField] private CameraController cameraController;

        public void Init(LevelConfigSO config)
        {
            cameraController.Init(config);
        }
    }
}

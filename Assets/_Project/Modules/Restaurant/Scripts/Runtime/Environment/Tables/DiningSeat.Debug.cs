#if UNITY_EDITOR
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public partial class DiningSeat : MonoBehaviour, ITaskProducer
    {
        [Header("DEBUG")]
        [SerializeField] private Order _order;

        
    }
}
#endif
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.Environment
{
    public partial class DiningSeat : MonoBehaviour
    {
        public bool IsOccupied => _occupiedGuest != null;
        public Transform WorkPos => _workPos;

        [Header("Settings")]
        [SerializeField] private TaskRuntimeSO _taskRuntimeSO;
        [SerializeField] private OrderEvent _onNewUnservedOrder;
        [SerializeField] private Transform _workPos;

        [Header("[Runtime]")]
        [SerializeField] private GuestContext _occupiedGuest;

        public void Occupy(GuestContext guest)
        {
            _occupiedGuest = guest;
        }
    }
}
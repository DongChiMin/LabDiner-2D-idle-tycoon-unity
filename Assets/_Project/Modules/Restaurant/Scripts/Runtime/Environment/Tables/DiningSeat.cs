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
        public bool IsOccupied => _occupiedGuest != null;
        public Transform WorkPos => _workPos;

        [Header("Settings")]
        [SerializeField] private TaskRuntimeSO _taskRuntimeSO;
        [SerializeField] private OrderEvent _onNewUnservedOrder;
        [SerializeField] private Transform _workPos;

        [Header("[Runtime]")]
        [SerializeField] private GuestContext _occupiedGuest;

        private ServingTask _servingTask;

        void Awake()
        {
            _servingTask = new ServingTask(_workPos, _occupiedGuest);
        }

        public void Occupy(GuestContext guest)
        {
            _occupiedGuest = guest;
        }

        public void WaitingForServe(Order order)
        {
            _order = order;
            PublishTask();
        }

        public void PublishTask()
        {
            _servingTask.SetGuest(_occupiedGuest);
            _taskRuntimeSO.Add(_servingTask);
        }

        public void OnTaskCompleted()
        {
            Debug.Log($"Nhân viên đã phục vụ xong bàn này!");
        }
    }
}
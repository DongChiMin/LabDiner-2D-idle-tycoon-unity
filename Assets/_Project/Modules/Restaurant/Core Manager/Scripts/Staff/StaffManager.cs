
using System.Collections;
using System.Collections.Generic;
using LabDiner.Restaurant;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StaffManager<TStaff, TTask> : MonoBehaviour
        where TStaff : IStaff
        where TTask : IStaffTask
    {

        [Header("Settings")]
        [SerializeField] private GameEvent<TTask> _onNewTask;
        [SerializeField] private GameEvent<TStaff> _onStaffAvailable;
        [SerializeField] List<TStaff> _staffs = new();
        [SerializeField] Queue<TStaff> _availableStaffs = new();
        [SerializeField] Queue<TTask> _taskQueue = new();

        [Header("[DEBUG VIEW]")]
        [SerializeField] protected List<TStaff> _debugAvailableStaffs = new();
        [SerializeField] protected List<TTask> _debugTasks = new();

        void OnEnable()
        {
            _onNewTask.Register(HandleNewTask);
            _onStaffAvailable.Register(HandleStaffAvailable);
        }

        void OnDisable()
        {
            _onNewTask.Unregister(HandleNewTask);
            _onStaffAvailable.Unregister(HandleStaffAvailable);
        }

        void Start()
        {
            _availableStaffs = new Queue<TStaff>(_staffs);
            SyncDebugView();
        }

        protected void HandleNewTask(TTask newTask)
        {
            if (_availableStaffs.Count > 0)
            {
                AssignTaskToStaff(_availableStaffs.Dequeue(), newTask);
            }
            else
            {
                _taskQueue.Enqueue(newTask);
                Debug.Log("Hết nhân viên rảnh để phục vụ đơn hàng mới!");
            }
            SyncDebugView();
        }

        protected void HandleStaffAvailable(TStaff staff)
        {
            if (_taskQueue.Count > 0)
            {
                TTask nextTask = _taskQueue.Dequeue();
                AssignTaskToStaff(staff, nextTask);
            }
            else
            {
                _availableStaffs.Enqueue(staff);
                Debug.Log("Không có đơn hàng nào đang chờ, nhân viên " + staff + " đã sẵn sàng.");
            }
            SyncDebugView();
        }

        protected void AssignTaskToStaff(TStaff staff, TTask task)
        {
            staff.DoTask(task);
        }



        private void SyncDebugView()
        {
            // Chỉ chạy trong Editor để tránh tốn tài nguyên khi build game thật
#if UNITY_EDITOR
            _debugTasks = new List<TTask>(_taskQueue);
            _debugAvailableStaffs = new List<TStaff>(_availableStaffs);
#endif
        }
    }
}
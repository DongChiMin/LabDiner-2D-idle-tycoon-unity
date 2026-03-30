
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
        [SerializeField] protected List<TTask> _debugTasksQueue = new();

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

        #region API
        /// <summary>
        /// Kiểm tra xem task có thể được giao cho nhân viên hay không (ví dụ: dựa trên loại task, trạng thái nhân viên...)
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected virtual bool CanAssignTask(TTask task) => true;
        /// <summary>
        /// Xử lý task trước khi giao cho nhân viên (ví dụ: thêm thông tin, chuyển đổi dữ liệu...)
        /// </summary>
        /// <param name="task"></param>
        /// <returns></returns>
        protected virtual TTask ProcessTaskBeforeExecute(TTask task) => task;
        #endregion

        protected void HandleNewTask(TTask newTask)
        {
            if (_availableStaffs.Count > 0)
            {
                if (!CanAssignTask(newTask)) return;

                TTask processedTask = ProcessTaskBeforeExecute(newTask);

                AssignTaskToStaff(_availableStaffs.Dequeue(), processedTask);
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
                TTask nextTask = _taskQueue.Peek();
                if (!CanAssignTask(nextTask)) return;

                nextTask = _taskQueue.Dequeue();
                TTask processedTask = ProcessTaskBeforeExecute(nextTask);

                
                AssignTaskToStaff(staff, processedTask);
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


        #region EDITOR_ONLY
        private void SyncDebugView()
        {
            // Chỉ chạy trong Editor để tránh tốn tài nguyên khi build game thật
#if UNITY_EDITOR
            _debugTasksQueue = new List<TTask>(_taskQueue);
            _debugAvailableStaffs = new List<TStaff>(_availableStaffs);
#endif
        }
        #endregion
    }
}
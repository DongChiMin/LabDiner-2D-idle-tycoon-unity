
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LabDiner.Restaurant;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Restaurant
{
    /// <summary>
    /// Một nhân viên có thể được quản lý bởi nhiều manager
    /// </summary>
    /// <typeparam name="TStaff"></typeparam>
    /// <typeparam name="TTask"></typeparam>
    public class StaffManager<TStaff, TTask> : MonoBehaviour, IStaffRegisterable<TStaff>
        where TStaff : IStaff
        where TTask : IStaffTask
    {

        [Header("Settings")]
        [SerializeField] private GameEvent<TTask> _onNewTask;
        [SerializeField] private GameEvent<TStaff> _onStaffAvailable;
        [Tooltip("Độ ưu tiên của event khi nhân viên sẵn sàng (dùng cho trường hợp nhân viên được quản lý bởi nhiều manager). Số càng lớn thì độ ưu tiên càng cao (được gọi trước khi Raise event).")]
        [SerializeField] private int _StaffAvailableEventPriority = 0;
        [SerializeField] List<TStaff> _staffs = new();
        Queue<TTask> _taskQueue = new();

        [Header("Optional")]
        [SerializeField, MaybeNull] private GameEvent<Station> _onStationAvailable;

        [Header("[DEBUG VIEW]")]
        [SerializeField] protected List<TStaff> _debugAvailableStaffs = new();
        [SerializeField] protected List<TTask> _debugTasksQueue = new();

        void OnEnable()
        {
            _onNewTask.Register(HandleNewTask);
            _onStaffAvailable.Register(HandleStaffAvailable, _StaffAvailableEventPriority);
            _onStationAvailable?.Register(HandleStationAvailable);
        }

        void OnDisable()
        {
            _onNewTask.Unregister(HandleNewTask);
            _onStaffAvailable.Unregister(HandleStaffAvailable);
            _onStationAvailable?.Unregister(HandleStationAvailable);
        }

        void Start()
        {
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

        public void AssignNewStaff(TStaff staff)
        {
            _staffs.Add(staff);
            HandleStaffAvailable(staff);
        }
        #endregion

        protected void HandleNewTask(TTask newTask)
        {
            TStaff availableStaff = GetAvailableStaff();

            if (availableStaff != null && CanAssignTask(newTask))
            {
                TTask processedTask = ProcessTaskBeforeExecute(newTask);

                AssignTaskToStaff(availableStaff, processedTask);
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
            Debug.Log("Được gọi từ manager " + this + " khi nhân viên " + staff + " sẵn sàng." + staff.IsAvailable);
            if (_taskQueue.Count > 0 && staff.IsAvailable)
            {
                TTask nextTask = _taskQueue.Peek();
                if (!CanAssignTask(nextTask)) return;

                nextTask = _taskQueue.Dequeue();
                TTask processedTask = ProcessTaskBeforeExecute(nextTask);

                AssignTaskToStaff(staff, processedTask);
            }
            else
            {
                // _availableStaffs.Enqueue(staff);
                Debug.Log("Không có đơn hàng nào đang chờ, hoặc nhân viên đã bận");
            }
            SyncDebugView();
        }

        protected void HandleStationAvailable(Station station)
        {
            TStaff availableStaff = GetAvailableStaff();
            if (_taskQueue.Count > 0 && availableStaff != null)
            {
                TTask nextTask = _taskQueue.Peek();
                if (!CanAssignTask(nextTask)) return;

                nextTask = _taskQueue.Dequeue();
                TTask processedTask = ProcessTaskBeforeExecute(nextTask);

                AssignTaskToStaff(availableStaff, processedTask);
            }
        }

        protected void AssignTaskToStaff(TStaff staff, TTask task)
        {
            staff.DoTask(task);
        }

        private TStaff GetAvailableStaff()
        {
            for (int i = 0; i < _staffs.Count; i++)
            {
                if (_staffs[i].IsAvailable)
                {
                    return _staffs[i];
                }
            }
            return default;
        }


        #region EDITOR_ONLY
        private void SyncDebugView()
        {
            // Chỉ chạy trong Editor để tránh tốn tài nguyên khi build game thật
#if UNITY_EDITOR
            _debugTasksQueue = new List<TTask>(_taskQueue);
            for (int i = 0; i < _staffs.Count; i++)
            {
                if (_staffs[i].IsAvailable)
                {
                    if (!_debugAvailableStaffs.Contains(_staffs[i]))
                        _debugAvailableStaffs.Add(_staffs[i]);
                }
                else
                {
                    if (_debugAvailableStaffs.Contains(_staffs[i]))
                        _debugAvailableStaffs.Remove(_staffs[i]);
                }
            }
#endif
        }
        #endregion
    }
}
using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class StaffTaskManager : MonoBehaviour
    {
        [SerializeField] private TaskRuntimeSO _taskRegistry;
        [SerializeField] private List<Staff> _allStaff; // Danh sách nhân viên trong nhà hàng

        void OnEnable() => _taskRegistry.OnTaskAdded += DispatchTasks;
        void OnDisable() => _taskRegistry.OnTaskAdded -= DispatchTasks;

        private void DispatchTasks()
        {
            // 1. Lấy ra các task đang chờ
            // 2. Tìm nhân viên rảnh và có kỹ năng phù hợp
            // 3. Giao task trực tiếp cho nhân viên đó
        }
    }
}
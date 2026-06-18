#if UNITY_EDITOR
using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    public partial class TaskRuntimeSO : ScriptableObject
    {
        [Header("[DEBUG]")]
        [SerializeField] private TaskType _filterType; // Chọn loại muốn soi
        [SerializeReference] private List<BaseTask> _viewingTasks = new();

        partial void Debug_FetchData()
        {
            _viewingTasks.Clear();
            if (_taskQueues.TryGetValue(_filterType, out var queue))
            {
                _viewingTasks.AddRange(queue);
            }
        }

        void OnValidate()
        {
            Debug_FetchData();
        }
    }
}
#endif
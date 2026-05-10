using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "TaskRuntimeSet", menuName = "LabDiner/RuntimeSet/Task")]
    public class TaskRuntimeSO : ScriptableObject
    {
        //Ví dụ: Dict<ServingTask, Queue...> 
        private Dictionary<TaskType, Queue<BaseTask>> _taskQueues = new Dictionary<TaskType, Queue<BaseTask>>();
        public System.Action OnTaskAdded;

        public void Add<T>(T task) where T : BaseTask
        {
            if (!_taskQueues.ContainsKey(task.Type))
            {
                _taskQueues[task.Type] = new Queue<BaseTask>();
            }
            _taskQueues[task.Type].Enqueue(task);
            OnTaskAdded?.Invoke();
        }

        //Lấy task theo type và skill của nhân viên, đồng thời đánh dấu là đã được nhận để tránh bị nhận trùng
        //Nếu task đã được nhận thì sẽ xóa khỏi queue và lấy task tiếp theo trong queue
        public T GetTask<T>(TaskType type) where T : BaseTask
        {
            if (_taskQueues.TryGetValue(type, out var queue))
            {
                while (queue.Count > 0)
                {
                    var task = queue.Dequeue();
                    if (!task.IsAssigned)
                    {
                        task.IsAssigned = true;
                        return task as T;
                    }
                }
            }
            return null;
        }
    }
}
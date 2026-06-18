using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "TaskRuntimeSet", menuName = "SO/Runtime/Task")]
    public partial class TaskRuntimeSO : ScriptableObject
    {
        //Ví dụ: Dict<ServingTask, danh sách task có type tương ứng...> 
        private Dictionary<TaskType, List<BaseTask>> _taskQueues = new Dictionary<TaskType, List<BaseTask>>();
        public System.Action OnTasksUpdated;

        public void Add<T>(T task) where T : BaseTask
        {
            if (!_taskQueues.ContainsKey(task.Type))
            {
                _taskQueues[task.Type] = new List<BaseTask>();
            }
            _taskQueues[task.Type].Add(task);
            OnTasksUpdated?.Invoke();
            Debug_FetchData();
        }

        //Lấy task theo type và skill của nhân viên, đồng thời đánh dấu là đã được nhận để tránh bị nhận trùng
        //Nếu task đã được nhận thì sẽ xóa khỏi queue và lấy task tiếp theo trong queue
        public T TryGetTask<T>(TaskType type) where T : BaseTask
        {
            if (!_taskQueues.TryGetValue(type, out var queue)) return null;

            for(int i = 0; i < queue.Count; i++)
            {
                Debug_FetchData();
                var task = queue[i];

                //Nếu task không còn vị trí làm việc, kiểm tra task tiếp theo
                if(!task.IsWorkPosAvailable) continue;
                
                //Nếu task đã có ai đó nhận
                if (task.IsAssigned)
                {
                    queue.RemoveAt(i);
                    i--;    //Điều chỉnh index sau khi xóa
                    continue;
                }

                task.IsAssigned = true;
                queue.RemoveAt(i);
                return task as T;
            }

            return null;
        }

        partial void Debug_FetchData();
    }
}
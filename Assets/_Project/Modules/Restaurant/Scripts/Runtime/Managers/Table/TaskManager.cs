using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class TaskManager : MonoBehaviour, ILevelRebuildable
    {
        [SerializeField] private TaskRuntimeSO _taskRuntimeSO;

        public void Rebuild()
        {
            _taskRuntimeSO.Clear();
        }
    }
}
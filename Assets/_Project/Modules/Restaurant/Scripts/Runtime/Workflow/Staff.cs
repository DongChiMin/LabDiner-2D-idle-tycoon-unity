using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Interface;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LabDiner.Restaurant.Workflow
{
    public class Staff : MonoBehaviour
    {
        public Transform RestPosition
        {
            get => _currentRestPos;
            set => _currentRestPos = value;
        }

        [Header("Config")]
        [SerializeField] private List<StaffSkill> _mySkills; // Ví dụ: [Cleaning, Serving]
        [SerializeField] private TaskRuntimeSO _taskRegistry;

        [Header("Runtime")]
        [SerializeField] private Transform _currentRestPos;

        private BaseTask _currentTask;
        private bool _isWorking;

        void OnEnable()
        {
            _taskRegistry.OnTasksUpdated += HandleTasksUpdated;
        }

        void OnDisable()
        {
            _taskRegistry.OnTasksUpdated -= HandleTasksUpdated;
        }

        void Awake()
        {
            _mySkills.Sort((a, b) => b.Priority.CompareTo(a.Priority)); // Sắp xếp kỹ năng theo độ ưu tiên
        }

        private void HandleTasksUpdated()
        {
            if (!_isWorking)
            {
                TryGetNewTask();
            }
        }

        private void TryGetNewTask()
        {
            foreach (var skill in _mySkills)
            {
                _currentTask = _taskRegistry.TryGetTask<BaseTask>(skill.SkillType);
                if (_currentTask != null)
                {
                    _isWorking = true;
                    StopAllCoroutines();
                    StartCoroutine(skill.Execute(_currentTask, onComplete: () =>
                    {
                        _currentTask = null;
                        _isWorking = false;
                        TryGetNewTask(); // Sau khi hoàn thành task, thử lấy task mới
                    }));
                    break;
                }
            }
        }
    }
}
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Interface;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LabDiner.Restaurant.Humanoid
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
            _taskRegistry.OnTaskAdded += HandleTaskAdded;
        }

        void OnDisable()
        {
            _taskRegistry.OnTaskAdded -= HandleTaskAdded;
        }

        void Awake()
        {
            _mySkills.Sort((a, b) => b.Priority.CompareTo(a.Priority)); // Sắp xếp kỹ năng theo độ ưu tiên
        }

        private void HandleTaskAdded()
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
                _currentTask = _taskRegistry.GetTask<BaseTask>(skill.SkillType);
                if (_currentTask != null)
                {
                    _isWorking = true;
                    StopAllCoroutines();
                    StartCoroutine(skill.PerformTask(_currentTask, onComplete: () =>
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
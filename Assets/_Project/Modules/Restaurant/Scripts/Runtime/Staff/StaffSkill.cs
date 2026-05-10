using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [RequireComponent(typeof(Staff))]
    public abstract class StaffSkill : MonoBehaviour
    {
        public int Priority => _priority;
        [SerializeField] private int _priority; // Độ ưu tiên khi có nhiều task phù hợp với kỹ năng của nhân viên
        protected Staff _staff;

        void Awake()
        {
            _staff = GetComponent<Staff>();
        }

        public abstract TaskType SkillType { get; } 
        public abstract IEnumerator PerformTask(BaseTask task, Action onComplete);
    }
}
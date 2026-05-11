using System;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    [Serializable]
    public abstract class BaseTask
    {
        public Transform Location { get => _location; }
        public bool IsAssigned { get => _isAssigned; set => _isAssigned = value; }

        [SerializeField] protected TaskType _type;
        [SerializeField] protected Transform _location;
        [SerializeField] protected bool _isAssigned;

        public abstract TaskType Type { get; }
        public abstract bool IsWorkPosAvailable { get; }
    }
}
using System;
using LabDiner.Restaurant.Enum;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [Serializable]
    public abstract class BaseTask
    {
        [SerializeField] protected TaskType _type;
        [SerializeField] protected Transform _location;
        [SerializeField] protected bool _isAssigned;

        public abstract TaskType Type { get; }
        public Transform Location { get => _location; }
        public bool IsAssigned { get => _isAssigned; set => _isAssigned = value; }
    }
}
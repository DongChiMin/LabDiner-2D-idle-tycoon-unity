
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class Station : MonoBehaviour
    {
        public CoreStation CoreStation => _coreStation;
        public Transform WorkPos => _workPos;
        
        [Header("Attributes")]
        [SerializeField] private CoreStation _coreStation;
        [SerializeField] private Transform _workPos;
        [SerializeField] public bool IsAvailable = true;
        [SerializeField] public bool IsUnlocked = false;
    }
}
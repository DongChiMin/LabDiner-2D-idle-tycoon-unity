#if UNITY_EDITOR
using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.UI;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    
    public partial class ServingSkill : StaffSkill
    {
        [Header("[DEBUG]")]
        [SerializeField] private ServingTask _debugTask;
        private partial void Debug_FetchData(ServingTask servingTask)
        {
            _debugTask = servingTask;
        }
    }
}
#endif
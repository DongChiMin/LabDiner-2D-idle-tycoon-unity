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
    
    public partial class ShippingSkill : StaffSkill
    {
        [Header("[DEBUG]")]
        [SerializeField] private ShippingTask _debugTask;
        partial void Debug_FetchData(ShippingTask shippingTask)
        {
            _debugTask = shippingTask;
        }
    }
}
#endif
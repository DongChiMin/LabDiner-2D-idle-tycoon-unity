using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    [Serializable]
    public class ServingTask : BaseTask
    {
        // Implement abstract properties
        public override TaskType Type => TaskType.Serving;
        public override bool IsWorkPosAvailable => _location != null;

        //Custom properties
        public Order Order => _order;

        //Attributes
        [SerializeField] private Order _order;

        public ServingTask (Transform location, Order order)
        {
            _type = TaskType.Serving;
            _isAssigned = false;
            _location = location;
            _order = order;
        }

        public void SetOrder(Order order)
        {
            _order = order;
        }

        public void SetLocation(Transform location)
        {
            _location = location;
        }
    }
}
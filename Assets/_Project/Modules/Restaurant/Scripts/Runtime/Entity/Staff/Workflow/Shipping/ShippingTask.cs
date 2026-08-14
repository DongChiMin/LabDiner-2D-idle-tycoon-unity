using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Model;
using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    [Serializable]
    public class ShippingTask : BaseTask
    {
        public PassTable PassTable => _passTable;
        public CookingTask CookingTask => _cookingTask;

        // Implement abstract properties
        public override TaskType Type => TaskType.Shipping;
        public override bool IsWorkPosAvailable => _location != null;

        //Attributes
        private CookingTask _cookingTask;
        private PassTable _passTable;

        public ShippingTask (PassTable passTable, Transform location, CookingTask task)
        {
            _type = TaskType.Shipping;
            _isAssigned = false;
            _passTable = passTable;
            _location = location;
            _cookingTask = task;
        }

        public void SetCookingTask(CookingTask task)
        {
            _cookingTask = task;
        }

        public void SetLocation(Transform location)
        {
            _location = location;
        }
    }
}
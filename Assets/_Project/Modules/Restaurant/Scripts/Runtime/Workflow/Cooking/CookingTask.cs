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
    public class CookingTask : BaseTask 
    {
        // Implement abstract properties
        public override TaskType Type => TaskType.Cooking;
        public override bool IsWorkPosAvailable => _coreStation.HasAnyAvailableWorkPos();

        //Custom properties
        public Station StationTarget => _stationTarget;
        public PassTable PassTableTarget => _passTableTarget;
        public CoreStation CoreStation => _coreStation;
        public Order Order => _order;
        public double Profit => _profit;

        //Attributes
        [SerializeField] private Order _order;
        [SerializeField] private CoreStation _coreStation;

        //Attributes được cập nhật trong quá trình thực hiện task
        [SerializeField] private PassTable _passTableTarget;
        [SerializeField] private Station _stationTarget;
        [SerializeField] private double _profit;
        public CookingTask (Transform location, Order order, CoreStation coreStation)
        {
            _type = TaskType.Cooking;
            _isAssigned = false;
            _location = location;

            _order = order;
            _coreStation = coreStation;
            _profit = 0;
        }

        public void SetStationTarget(Station station)
        {
            _stationTarget = station;
        }

        public void SetPassTableTarget(PassTable passTable)
        {
            _passTableTarget = passTable;
        }

        public void SetProfit(double profit)
        {
            _profit = profit;
        }
    }
}
using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class ChefManager : StaffManager<ChefContext, CookingTask>
    {
        //Event: _OnNewCookingTask
        //Event: _OnChefAvailable
    
        protected override bool CanAssignTask(CookingTask task)
        {
            return LevelManagerContext.Instance.CoreStationManager.HasAnyAvailableStation(task.CoreStation);
        }

        protected override CookingTask ProcessTaskBeforeExecute(CookingTask task)
        {
            //Xác định station và pass table cụ thể cho task này
            Station availableStation = LevelManagerContext.Instance.CoreStationManager.PopAvailableStation(task.CoreStation);
            PassTable passTable = LevelManagerContext.Instance.PassTableManager.GetAvailablePassTable();
            if(availableStation == null)
            {
                Debug.LogError($"This should not happen! Task {task} was assigned to chef but no available station found!");
                return null;
            }
            if(passTable == null)
            {
                Debug.LogError($"This should not happen! Task {task} was assigned to chef but no available pass table found!");
                return null;
            }
            task.StationTarget = availableStation;
            task.PassTableTarget = passTable;
            return task;
        }
    } 
}
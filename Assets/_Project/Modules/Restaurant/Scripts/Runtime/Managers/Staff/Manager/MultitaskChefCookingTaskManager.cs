using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class MultitaskChefCookingTaskManager : StaffManager<MultitaskChefContext, CookingTask>
    {
        //Event: _OnNewCookingTask
        //Event: _OnMultitaskChefAvailable
    
        protected override bool CanAssignTask(CookingTask task)
        {
            return LevelManagerContext.Instance.CoreStationManager.HasAnyAvailableStation(task.CoreStation);
        }

        protected override CookingTask ProcessTaskBeforeExecute(CookingTask task)
        {
            //Xác định station và pass table cụ thể cho task này
            Station availableStation = LevelManagerContext.Instance.CoreStationManager.PopAvailableStation(task.CoreStation);
            if(availableStation == null)
            {
                Debug.LogError($"This should not happen! Task {task} was assigned to chef but no available station found!");
                return null;
            }
            task.StationTarget = availableStation;
            return task;
        }
    } 
}
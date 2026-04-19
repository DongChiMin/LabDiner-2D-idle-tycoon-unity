
using UnityEngine;

namespace LabDiner.Restaurant
{
    public interface IStaff
    {
        Transform RestPosition { get; set;}
        bool IsAvailable { get; set; }
        void DoTask(IStaffTask task);

        void OnTaskCompleted(IStaffTask task);

        void UpgradeMoveSpeed(float speedBuffValue);
    }
}
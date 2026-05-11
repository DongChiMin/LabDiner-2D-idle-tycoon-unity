using UnityEngine;

namespace LabDiner.Restaurant.Workflow
{
    public partial class CookingSkill : StaffSkill
    {
        [Header("[DEBUG]")]
        [SerializeField] private CookingTask _currentTask;
        
        private partial void Debug_FetchData(CookingTask cookingTask)
        {
            _currentTask = cookingTask;
        }
    }
}
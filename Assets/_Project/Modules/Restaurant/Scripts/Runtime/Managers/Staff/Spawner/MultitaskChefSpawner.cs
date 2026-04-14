using UnityEngine;

namespace LabDiner.Restaurant
{
    [RequireComponent(typeof(MultitaskChefCookingTaskManager))]
    [RequireComponent(typeof(MultitaskChefOrderManager))]
    public class MultitaskChefSpawner : StaffSpawner<MultitaskChefContext>
    {
    }
}
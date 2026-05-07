using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.Restaurant.Event
{
    /// <summary>
    /// chứa thông tin thời gian hoàn thành level
    /// </summary>
    [CreateAssetMenu(fileName = "OnLevelComplete", menuName = "Events/Level/Level Complete Event")]
    public class LevelCompleteEvent : GameEvent<int> { }
}
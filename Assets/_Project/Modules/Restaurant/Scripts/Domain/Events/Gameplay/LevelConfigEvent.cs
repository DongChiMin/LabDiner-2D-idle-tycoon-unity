using LabDiner.Shared.Event;
using LabDiner.Restaurant.Environment;
using UnityEngine;
using LabDiner.Restaurant.SO;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "LevelConfigEvent", menuName = "Events/GamePlay/LevelConfig Event")]
    public class LevelConfigEvent : GameEvent<LevelConfigSO> { }
}
using LabDiner.Shared.Event;
using LabDiner.Restaurant.Environment;
using UnityEngine;
using LabDiner.Restaurant.SO;

namespace LabDiner.Restaurant.Event
{
    [CreateAssetMenu(fileName = "OnLevelInit", menuName = "Events/GamePlay/LevelInit Event")]
    public class LevelConfigEvent : GameEvent<LevelConfigSO> { }
}
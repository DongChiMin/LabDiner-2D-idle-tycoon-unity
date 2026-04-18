
using LabDiner.Shared.Enum;
using UnityEngine;

namespace LabDiner.Shared.SO
{
    /// <summary>
    /// Các hiệu ứng khi station đạt được 1 sao bất kỳ
    /// </summary>
    [CreateAssetMenu(fileName = "StationStarEffect", menuName = "Game/Station/StationStarEffect")]
    public class StationStarEffectSO : ScriptableObject
    {
        public StationStarEffect EffectType;
        public float Value; 
    }
}
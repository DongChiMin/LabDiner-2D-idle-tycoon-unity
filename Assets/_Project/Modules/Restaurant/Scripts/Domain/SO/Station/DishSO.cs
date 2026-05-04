using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "Dish", menuName = "Game/Station/Dish")]
    public class DishSO : ScriptableObject
    {
        public string Id => name; // Sử dụng tên của ScriptableObject làm ID
        public string Name;
        public Sprite Dishicon;
        public Sprite StationIcon;
    }
}
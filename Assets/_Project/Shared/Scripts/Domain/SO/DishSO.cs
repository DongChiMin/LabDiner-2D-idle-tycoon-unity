using System.Collections.Generic;
using UnityEngine;

namespace LabDiner.Shared
{
    [CreateAssetMenu(fileName = "Dish", menuName = "Game/Dish")]
    public class DishSO : ScriptableObject
    {
        public string Id => name; // Sử dụng tên của ScriptableObject làm ID
        public string Name;
        public Sprite Icon;
    }
}
using UnityEngine;

namespace LabDiner.Laboratory
{
    public class PoolContext : Singleton<PoolContext>
    {
        [Header("Pool References")]
        [SerializeField] private SceneObjectPooling<LabIngredientContext> _ingredientPool;

        // Các script khác chỉ có thể đọc, không thể gán lại
        public SceneObjectPooling<LabIngredientContext> IngredientPool => _ingredientPool;
    }
}
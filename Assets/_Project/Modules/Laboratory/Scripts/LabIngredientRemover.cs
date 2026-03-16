using LabDiner.Shared;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Laboratory
{
    public class LabIngredientRemover : MonoBehaviour
    {
        // [SerializeField] private IngredientEvent OnIngredientRemoved;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out LabIngredientContext ingredient))
            {
                // //1. Phát hiện nguyên liệu rơi vào vùng xóa
                // OnIngredientRemoved?.Raise(ingredient.CtxData.IngredientData);

                //2. Trả nguyên liệu về pool thay vì destroy
                ReturnToPool(ingredient.CtxPoolMember);

                //3. Gọi hàm OnEndDrag trên SpawnerMember nếu tồn tại
                ReturnToSpawner(ingredient.CtxSpawnerMember);
            }
        }

        void ReturnToPool(PoolMember poolMember)
        {
            poolMember.ReturnToPool();
        }

        void ReturnToSpawner(SpawnerMember spawnerMember)
        {
            spawnerMember.ReturnToSpawner();
        }
    }
}


using LabDiner.Shared;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Laboratory
{
    public class Cauldron : MonoBehaviour
    {
        [Header("Broadcasting Events")]
        [SerializeField] private IngredientEvent onIngredientDroppedIn;
        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out LabIngredientContext ingredient))
            {
                // 2. Lấy Rigidbody2D của vật đang rơi để kiểm tra vận tốc
                Rigidbody2D rb = ingredient.CtxDrag.Rigidbody;

                // 3. Chỉ tính nếu vận tốc Y < -0.1f (đang rơi xuống)
                if (rb.linearVelocity.y < -0.1f)
                {
                    var data = ingredient.CtxData.IngredientData;

                    //Phát sự kiện nguyên liệu rơi vào nồi
                    onIngredientDroppedIn?.Raise(data);

                    // Trả nguyên liệu về pool thay vì destroy
                    PoolContext.Instance.IngredientPool.ReturnToPool(ingredient);
                }

            }
        }
    }
}
// using UnityEngine;

// namespace LabDiner.Shared
// {
//     public class PoolMember : MonoBehaviour, IPoolable 
// {
//     private SceneObjectPooling<PoolMember> _originPool;

//     public void SetOrigin(SceneObjectPooling<PoolMember> pool) => _originPool = pool;

//     public void ReturnToPool() 
//     {
//         if (_originPool != null) 
//         {
//             _originPool.ReturnToPool(this);
//         }
//         else 
//         {
//             Destroy(gameObject); // Fallback nếu có lỗi
//         }
//     }
// }
// }
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class PoolContext : Singleton<PoolContext>
    {
        [Header("Pool References")]
        [SerializeField] private SceneObjectPooling<GuestContext> _guestPool;

        // Các script khác chỉ có thể đọc, không thể gán lại
        public SceneObjectPooling<GuestContext> GuestPool => _guestPool;
    
    }
}

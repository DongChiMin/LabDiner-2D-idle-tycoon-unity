using UnityEngine;
using LabDiner.Shared.DesignPattern;
using LabDiner.Restaurant.Humanoid;

namespace LabDiner.Restaurant.Pooling
{
    public class PoolContext : Singleton<PoolContext>
    {
        [Header("Pool References")]
        [SerializeField] private SceneObjectPooling<GuestContext> _guestPool;
        [SerializeField] private StaffBoxPool _staffBoxPool;
        [SerializeField] private StationBoxPool _stationBoxPool;
        [SerializeField] private CurrencyFlyPool _currencyFlyPool;

        // Các script khác chỉ có thể đọc, không thể gán lại
        public SceneObjectPooling<GuestContext> GuestPool => _guestPool;
        public StaffBoxPool StaffBoxPool => _staffBoxPool;
        public StationBoxPool StationBoxPool => _stationBoxPool;
        public CurrencyFlyPool CurrencyFlyPool => _currencyFlyPool;
    
    }
}

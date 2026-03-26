using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelManagerContext : Singleton<LevelManagerContext>
    {
        //Các manager đều là dạng Singleton, để tránh spaghetti code thì chỉ các manager được phép gọi nhau
        //Các object ngoài chỉ được gọi thông qua SOEvent
        [Header("Objects")]
        public ChefManager chefManager;
        public WaiterManager waiterManager;
        public GuestManager guestManager;

        [Header("Places")]
        public DiningTableManager diningTableManager;
        public CoreStationManager coreStationManager;

        [Header("Logics")]
        public ServeManager serveManager;
        public LevelCurrencyManager levelCurrencyManager;
        public OrderManager orderManager;
        public LevelConfigSO levelConfigSO;

        [Header("[Optional]")]  //có thể null tùy level
        public WaitingLineManager waitingLineManager;
        public PassTableManager passTableManager;

        public bool HasWaitingLine => waitingLineManager != null;
        public bool HasPassTable => passTableManager != null;

        protected override void Awake()
        {
            base.Awake();
            guestManager.OnInit(levelConfigSO);
        }
    }
}
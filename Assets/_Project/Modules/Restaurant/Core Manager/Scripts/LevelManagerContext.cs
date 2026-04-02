using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelManagerContext : Singleton<LevelManagerContext>
    {
        //Các manager đều là dạng Singleton, để tránh spaghetti code thì chỉ các manager được phép gọi nhau
        //Các object ngoài chỉ được gọi thông qua SOEvent
        [Header("Objects")]
        public ChefManager ChefManager;
        public WaiterManager WiterManager;
        public GuestManager GuestManager;

        [Header("Places")]
        public DiningTableManager DiningTableManager;
        public CoreStationManager CoreStationManager;

        [Header("Logics")]
        public ServeManager ServeManager;
        public LevelCurrencyManager LevelCurrencyManager;
        public OrderManager OrderManager;
        public ReputationManager ReputationManager;
        public LevelConfigSO LevelConfigSO;

        [Header("[Optional]")]  //có thể null tùy level
        public WaitingLineManager WaitingLineManager;
        public PassTableManager PassTableManager;

        public bool HasWaitingLine => WaitingLineManager != null;
        public bool HasPassTable => PassTableManager != null;

        protected override void Awake()
        {
            base.Awake();
            GuestManager.OnInit(LevelConfigSO);
        }
    }
}
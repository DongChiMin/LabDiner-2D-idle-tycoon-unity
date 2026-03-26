using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class LevelManagerContext : Singleton<LevelManagerContext>
    {
        public ChefManager chefManager;
        public WaiterManager waiterManager;
        public DiningTableManager diningTableManager;
        public WaitingLineManager waitingLineManager;
        public PassTableManager passTableManager;
        public ServeManager serveManager;
        public LevelCurrencyManager levelCurrencyManager;
        public CoreStationManager coreStationManager;
        public GuestManager guestManager;
        public OrderManager orderManager;
        public LevelConfigSO levelConfigSO;

        protected override void Awake()
        {
            base.Awake();
            guestManager.OnInit(levelConfigSO);
        }
    }
}
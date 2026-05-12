using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.DesignPattern;
using UnityEngine;

namespace LabDiner.Restaurant.Manager
{
    public class LevelManagerContext : Singleton<LevelManagerContext>, ILevelInitializable
    {
        [Header("Objects")]
        public GuestSpawner GuestSpawner;

        [Header("Places")]
        [SerializeField] private DiningTableManager DiningTableManager;
        [SerializeField] public CoreStationManager CoreStationManager;

        [Header("Logics")]
        // public LevelCurrencyManager LevelCurrencyManager;
        public ReputationManager ReputationManager;

        [Header("[Optional]")]  //có thể null tùy level
        public WaitingLineManager WaitingLineManager;
        public PassTableManager PassTableManager;

        public bool HasWaitingLine => WaitingLineManager != null;
        public bool HasPassTable => PassTableManager != null;

        public void Init(LevelConfigSO config)
        {
            GuestSpawner.Init(config);
            DiningTableManager.Init(config);
        }
    }
}
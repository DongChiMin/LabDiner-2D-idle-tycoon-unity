using UnityEngine;
using LabDiner.LevelSystem.Domain;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Event;
using LabDiner.Shared;

namespace LabDiner.LevelSystem.Runtime
{
    public class LevelLoader : MonoBehaviour
    {
        private const string PROGRESS_FILE_NAME = ProgressRepository.PROGRESS_FILE_NAME;

        [SerializeField] private LevelConfigEvent _onLevelInit;

        [Header("Level Load")]
        [SerializeField] private IntEvent _onLevelComplete;

        [Header("Level Init")]
        [SerializeField] private List<Transform> _initRoots;


        void OnEnable()
        {
            _onLevelComplete.Register(SaveLevel);
        }

        void OnDisable()
        {
            _onLevelComplete.Unregister(SaveLevel);
        }

        private void SaveLevel(int levelCompleted)
        {
            JSONExecutor.WriteToFile(new PlayerProgress
            {
                currentLevelIndex = levelCompleted + 1,
                hasSeenIntro = false
            }.ToJson(), PROGRESS_FILE_NAME, true);
        }

        public void LoadLevel(LevelConfigSO configSO)
        {
            StopAllCoroutines();
            // Phase 1: Sinh ra thực thể/Sắp xếp thực thể vào list
            ExecutePhase1_SetupLayout();

            // Phase 2: Bơm dữ liệu cấu hình
            ExecutePhase2_InitLogic(configSO);

            // Phase 3: Khôi phục tiến độ (Mồi cho cậu triển khai sau)
            // ExecutePhase3_RestoreProgress();
        }

        private void ExecutePhase1_SetupLayout()
        {
            foreach (var root in _initRoots)
            {
                ILevelRebuildable[] initializables = root.GetComponentsInChildren<ILevelRebuildable>();
                foreach (var init in initializables)
                {
                    init.Rebuild();
                }
            }
        }

        private void ExecutePhase2_InitLogic(LevelConfigSO configSO)
        {
            //Init theo thứ tự
            foreach (var root in _initRoots)
            {
                ILevelInitializable[] initializables = root.GetComponentsInChildren<ILevelInitializable>();
                foreach (var init in initializables)
                {
                    init.Init(configSO);
                }
            }

            //Cuối cùng thì mới Init các object ko cần thứ tự
            _onLevelInit.Raise(configSO);
        }
    }
}

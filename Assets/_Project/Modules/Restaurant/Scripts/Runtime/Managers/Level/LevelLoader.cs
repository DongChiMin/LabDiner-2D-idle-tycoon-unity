using UnityEngine;
using LabDiner.LevelSystem.Domain;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Event;
using LabDiner.Shared;
using System.Collections;

namespace LabDiner.Restaurant.Managers
{
    public class LevelLoader : MonoBehaviour
    {
        private string PHASE_1_NAME = "ExecutePhase1_SetupLayout";
        private string PHASE_2_NAME = "ExecutePhase2_InitLogic";
        private string PHASE_3_NAME = "ExecutePhase3_RestoreProgress";
        [SerializeField] private LevelConfigEvent _onLevelInit;
        [SerializeField] private LevelConfigEvent _onLevelIntroStart;

        [Header("Level Load")]
        [SerializeField] private LevelConfigEvent _onLevelComplete;

        [Header("Level Init")]
        [SerializeField] private List<Transform> _initRoots;

        [Header("Level Progress")]
        [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;
        [SerializeField] private LevelRegistrySO _levelRegistry;


        void OnEnable()
        {
            _onLevelComplete.Register(SaveLevel);
        }

        void OnDisable()
        {
            _onLevelComplete.Unregister(SaveLevel);
        }

        private void SaveLevel(LevelConfigSO levelCompleted)
        {
            int levelIndex = levelCompleted.LevelIndex;
            string ID = levelCompleted.ID;
            _progressRuntimeSO.PlayerSave.SetLevelCompleted(ID);
            LevelConfigSO nextLevel = _levelRegistry.GetNextLevelConfigSO(levelCompleted);
            if (nextLevel != null)
            {
                _progressRuntimeSO.PlayerSave.UpdateCurrentLevelID(nextLevel.ID);
            }
        }

        void Awake()
        {
            BootstrapManager.Instance?.RegisterLoadPhases(PHASE_1_NAME, 30f);
            BootstrapManager.Instance?.RegisterLoadPhases(PHASE_2_NAME, 20f);
            BootstrapManager.Instance?.RegisterLoadPhases(PHASE_3_NAME, 20f);
        }

        public void LoadLevel(LevelConfigSO configSO)
        {            
            StopAllCoroutines();
            StartCoroutine(LoadLevelRoutine(configSO));

            
        }

        private IEnumerator LoadLevelRoutine(LevelConfigSO configSO)
        {
            // Phase 1: Sinh ra thực thể/Sắp xếp thực thể vào list
            yield return StartCoroutine(ExecutePhase1_SetupLayout());
            BootstrapManager.Instance?.CompletePhase(PHASE_1_NAME);

            // Phase 2: Bơm dữ liệu cấu hình
            yield return StartCoroutine(ExecutePhase2_InitLogic(configSO));
            BootstrapManager.Instance?.CompletePhase(PHASE_2_NAME);

            // Phase 3: Khôi phục tiến độ (Mồi cho cậu triển khai sau)
            yield return StartCoroutine(ExecutePhase3_RestoreProgress(configSO));
            BootstrapManager.Instance?.CompletePhase(PHASE_3_NAME);
        }

        private IEnumerator ExecutePhase1_SetupLayout()
        {
            int count = 0;
            foreach (var root in _initRoots)
            {
                ILevelRebuildable[] initializables = root.GetComponentsInChildren<ILevelRebuildable>();
                foreach (var init in initializables)
                {
                    init.Rebuild();
                
                    //xử lý bất đồng bộ: mỗi khi rebuild xong 5 root thì đợi sang frame sau
                    //nhường cho UI
                    count++;
                    if(count % 5 == 0)
                    {
                        yield return null;
                    }
                }
            }
        }

        private IEnumerator ExecutePhase2_InitLogic(LevelConfigSO configSO)
        {
            //Init theo thứ tự
            int count = 0;
            foreach (var root in _initRoots)
            {
                ILevelInitializable[] initializables = root.GetComponentsInChildren<ILevelInitializable>();
                foreach (var init in initializables)
                {
                    init.Init(configSO);
                }

                //xử lý bất đồng bộ: mỗi khi init xong 5 root thì đợi sang frame sau
                //nhường cho UI
                count++;
                if(count % 5 == 0)
                {
                    yield return null;
                }
            }

            //Cuối cùng thì mới Init các object ko cần thứ tự
            _onLevelInit.Raise(configSO);
        }

        private IEnumerator ExecutePhase3_RestoreProgress(LevelConfigSO configSO)
        {
            LevelProgressSave progress = _progressRuntimeSO.LevelProgressSave;

            //Nếu chưa xem intro == bắt đầu chơi mới level này
            //- Đưa danh sách nhiệm vụ và upgrade vào progress để sau này lưu lại
            //- Set lại cờ đã xem intro = true
            //- Raise sự kiện bắt đầu intro
            if (!progress.hasSeenIntro)
            {
                _progressRuntimeSO.InitializeProgress(configSO);
                _progressRuntimeSO.LevelProgressSave.UpdateHasSeenIntro(true);
                _progressRuntimeSO.PlayerSave.StartNewLevel(configSO.ID);
                _progressRuntimeSO.OnProgressInject?.Invoke();
                _onLevelIntroStart.Raise(configSO);
            }

            //Nếu đã xem intro rồi
            //- Raise event để các thành phần trong game tự lấy tiến độ chơi và cập nhật
            else
            {
                _progressRuntimeSO.OnProgressInject?.Invoke();
            }
            yield return null;
        }

        
    }
}

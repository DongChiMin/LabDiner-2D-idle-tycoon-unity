using System.Collections.Generic;
using LabDiner.LevelSystem.Domain;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using LabDiner.Shared;
using LabDiner.Shared.Event;
using UnityEngine;
namespace LabDiner.LevelMap.UI
{
    public class LevelMapController : MonoBehaviour, ILevelInitializable, ILevelProgress
    {
        [Header("Data")]
        [SerializeField] private LevelRegistrySO _levelRegistrySO;
        [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;

        [Header("Events")]
        [SerializeField] private UIPopupEvent _onPopupShow;
        [SerializeField] private LevelConfigEvent _onLevelInit;

        [Header("UI References")]
        [SerializeField] private LevelMapPanel _panel;
        [SerializeField] private LevelMapItem _itemPrefab;
        [SerializeField] private Transform _itemContainer;

        private List<LevelMapItem> _levelMapItems = new List<LevelMapItem>();

        void OnEnable()
        {
            _onPopupShow.Register(HandlePopupShow);
            _panel.CloseButton.onClick.AddListener(HandlePopupHide);
            _progressRuntimeSO.OnProgressInject += LoadProgress;
            _onLevelInit.Register(Init);
        }

        void OnDisable()
        {
            _onPopupShow.Unregister(HandlePopupShow);
            _panel.CloseButton.onClick.RemoveListener(HandlePopupHide);
            _progressRuntimeSO.OnProgressInject -= LoadProgress;
            _onLevelInit.Unregister(Init);
        }

        private void HandlePopupShow()
        {
            _panel.Show();
        }

        private void HandlePopupHide()
        {
            _panel.Hide();
        }

        public void Init(LevelConfigSO config)
        {
            LevelChapterSO chapter = _levelRegistrySO.GetChapterByLevelConfig(config);
            List<LevelConfigSO> levelsInChapter = _levelRegistrySO.GetLevelConfigsByChapter(chapter);

            foreach (Transform child in _itemContainer)
            {
                Destroy(child.gameObject);
            }

            foreach (LevelConfigSO levelConfig in levelsInChapter)
            {
                LevelMapItem item = Instantiate(_itemPrefab, _itemContainer);
                item.Setup(levelConfig);
                _levelMapItems.Add(item);
            }
        }

        public void LoadProgress(ProgressSaveRuntimeSO progressRuntimeSO)
        {
            Debug.Log("[LevelMapController] Loading progress for level map items.");
            PlayerSave playerSave = progressRuntimeSO.PlayerSave;
            List<PlayedLevel> playedLevels = playerSave.PlayedLevels;

            foreach(LevelMapItem item in _levelMapItems)
            {
                LevelConfigSO levelConfig = item.LevelConfig;
                PlayedLevel playedLevel = playedLevels.Find(level => level.LevelID == levelConfig.ID);

                //Nếu có dữ liệu nói rằng level này đã được chơi
                if (playedLevel != null)
                {
                    switch (playedLevel.Status)
                    {
                        //Hiển thị UI theo kiểu: đang chơi dở
                        case LevelStatus.InProgress:
                            item.SetInProgressUI();
                            break;
                        //Hiển thị UI theo kiểu: đã hoàn thành level
                        case LevelStatus.Completed:
                            item.SetCompletedUI(playedLevel);
                            break;
                        default:
                            Debug.LogError($"[LevelMapController] Unknown level status for level {levelConfig.ID}: {playedLevel.Status}");
                            break;
                    }
                }
                //Chưa có dữ liệu -> level này chưa chơi
                else
                {
                    item.SetLockedUI();
                }
            }
        }

        public void UpdateProgress()
        {
            throw new System.NotImplementedException();
        }
    }
}

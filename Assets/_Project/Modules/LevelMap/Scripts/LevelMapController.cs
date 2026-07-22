using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.SO;
using LabDiner.Shared;
using LabDiner.Shared.Event;
using UnityEngine;
namespace LabDiner.LevelMap.UI
{
    public class LevelMapController : MonoBehaviour, ILevelInitializable, ILevelProgress
    {
        [Header("Item References")]
        [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;
        [SerializeField] private UIPopupEvent _onPopupShow;
        [SerializeField] private LevelMapPanel _panel;

        private LevelProgressSave _progress => _progressRuntimeSO.LevelProgressSave;

        void OnEnable()
        {
            _onPopupShow.Register(HandlePopupShow);
            _panel.CloseButton.onClick.AddListener(HandlePopupHide);
        }

        void OnDisable()
        {
            _onPopupShow.Unregister(HandlePopupShow);
            _panel.CloseButton.onClick.RemoveListener(HandlePopupHide);
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
            int currentLevelIndex = config.LevelIndex;
        }

        public void LoadProgress()
        {
        }

        public void UpdateProgress()
        {
            throw new System.NotImplementedException();
        }
    }
}

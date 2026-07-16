using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.SO;
using UnityEngine;

namespace LabDiner.LevelMap.UI
{
    public class LevelStartController : MonoBehaviour
    {
        [Header("Runtimes")]
        [SerializeField] private TutorialRuntimeSO _tutorialRuntimeSO;

        [Header("Item References")]
        [SerializeField] private LevelConfigEvent _onLevelIntroStart;
        [SerializeField] private LevelStartPanel _panel;

        void OnEnable()
        {
            _onLevelIntroStart.Register(HandleLevelIntroStart);
            _panel.StartButton.onClick.AddListener(HandleGameStart);
        }

        void OnDisable()
        {
            _onLevelIntroStart.Unregister(HandleLevelIntroStart);
            _panel.StartButton.onClick.RemoveListener(HandleGameStart);
        }

        private void HandleGameStart()
        {
            _panel.Hide();
            _tutorialRuntimeSO.OnLevelStarted?.Invoke();
        }

        private void HandleLevelIntroStart(LevelConfigSO levelConfigSO)
        {
            _panel.Setup(levelConfigSO);
            _panel.Show();
        }
    }
}

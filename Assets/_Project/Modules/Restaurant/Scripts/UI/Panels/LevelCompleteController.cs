using System.Collections.Generic;
using LabDiner.Restaurant.Event;
using LabDiner.Restaurant.Interface;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.Event;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant.UI
{
    public class LevelCompleteController : MonoBehaviour, ILevelInitializable   {
        
        [Header("Events")]
        [SerializeField] private LevelCompleteEvent _onLevelComplete;

        [Header("Item References")]
        [SerializeField] private LevelCompletePanel _panel;

        void OnEnable()
        {
            _panel.ContinueButton.onClick.AddListener(HandleContinue);
            _onLevelComplete.Register(HandleLevelComplete);
        }

        void OnDisable()
        {
            _panel.ContinueButton.onClick.RemoveListener(HandleContinue);
            _onLevelComplete.Unregister(HandleLevelComplete);
        }

        #region API

        public void Init(LevelConfigSO levelConfigSO)
        {
            
        }

        #endregion

        private void HandleLevelComplete(int completionTime)
        {
            _panel.Show();
        }

        private void HandleContinue()
        {
            _panel.Hide();
            // LevelManagerContext.Instance.LoadNextLevel();
        }
    }
}

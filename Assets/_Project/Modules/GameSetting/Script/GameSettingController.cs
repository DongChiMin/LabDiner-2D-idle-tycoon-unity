using LabDiner.Shared.Event;
using UnityEngine;

namespace LabDiner.GameSetting.UI
{
    public class GameSettingController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameSettingPanel _panel;

        [Header("Events")]
        [SerializeField] private UIPopupEvent _onPopupShow;

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


    }
}

using LabDiner.Shared.Event;
using LabDiner.Shared.UI;
using UnityEngine;

namespace LabDiner.GameSetting.UI
{
    public class GameSettingController : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private GameSettingPanel _panel;

        [Header("Events")]
        [SerializeField] private UIPopupEvent _onPopupShow;
        [SerializeField] private ClickOutsideEffect _clickOutsideEffect;

        void OnEnable()
        {
            _onPopupShow.Register(HandlePopupShow);
            _clickOutsideEffect.OnClickOutside += HandlePopupHide;

            //Các nút UI
            _panel.CloseButton.onClick.AddListener(HandlePopupHide);
            _panel.BtnMusic.OnValueChanged += HandleMusicToggle;
            _panel.BtnEffect.OnValueChanged += HandleEffectToggle;
            _panel.BtnHaptic.OnValueChanged += HandleHapticToggle;
            _panel.BtnResetToDefault.onClick.AddListener(HandleResetToDefault);
            _panel.LanguageDropdown.onValueChanged.AddListener(HandleLanguageChange);
            _panel.FPSDropdown.onValueChanged.AddListener(HandleFPSChange);
            _panel.BtnTermsOfService.onClick.AddListener(HandleTermsOfServiceClick);
            _panel.BtnPrivacyPolicy.onClick.AddListener(HandlePrivacyPolicyClick);
        }

        void OnDisable()
        {
            _onPopupShow.Unregister(HandlePopupShow);
            _clickOutsideEffect.OnClickOutside -= HandlePopupHide;

            //Các nút UI
            _panel.CloseButton.onClick.RemoveListener(HandlePopupHide);
            _panel.BtnMusic.OnValueChanged -= HandleMusicToggle;
            _panel.BtnEffect.OnValueChanged -= HandleEffectToggle;
            _panel.BtnHaptic.OnValueChanged -= HandleHapticToggle;
            _panel.BtnResetToDefault.onClick.RemoveListener(HandleResetToDefault);
            _panel.LanguageDropdown.onValueChanged.RemoveListener(HandleLanguageChange);
            _panel.FPSDropdown.onValueChanged.RemoveListener(HandleFPSChange);
            _panel.BtnTermsOfService.onClick.RemoveListener(HandleTermsOfServiceClick);
            _panel.BtnPrivacyPolicy.onClick.RemoveListener(HandlePrivacyPolicyClick);
        }

        void Start()
        {
            ApplyAllSettings();
            _panel.Setup();
        }

        private void HandlePopupShow()
        {
            _panel.Setup();
            _panel.Show();
        }

        private void HandlePopupHide()
        {
            GameSettingVariables.SaveAll();
            _panel.Hide();
        }

        #region UI Event Handlers

        private void HandleMusicToggle(bool isOn)
        {
            GameSettingVariables.Music = isOn ? 1.0f : 0.0f;

            ApplyMusicChange(isOn);
        }

        private void HandleEffectToggle(bool isOn)
        {
            GameSettingVariables.Effect = isOn ? 1.0f : 0.0f;

            ApplyEffectChange(isOn);
        }

        private void HandleHapticToggle(bool isOn)
        {
            GameSettingVariables.Haptic = isOn ? 1.0f : 0.0f;

            ApplyHapticChange(isOn);
        }

        private void HandleResetToDefault()
        {
            GameSettingVariables.ResetToDefault();
            ApplyAllSettings();
            _panel.Setup();
        }

        private void HandleLanguageChange(int index)
        {
            string selectedLanguage = GetLanguageFromDropdownIndex(index);
            GameSettingVariables.Language = selectedLanguage;
            
            ApplyLanguageChange(selectedLanguage);
        }

        private void HandleFPSChange(int index)
        {
            int selectedFPS = GetFPSFromDropdownIndex(index);
            GameSettingVariables.FPS = selectedFPS;

            ApplyFPSChange(selectedFPS);
        }

        private void HandleTermsOfServiceClick()
        {
            Application.OpenURL("https://www.example.com/terms-of-service");
        }

        private void HandlePrivacyPolicyClick()
        {
            Application.OpenURL("https://www.example.com/privacy-policy");
        }
        #endregion

        #region Apply Changes
        private void ApplyAllSettings()
        {
            ApplyMusicChange(GameSettingVariables.Music > 0);
            ApplyEffectChange(GameSettingVariables.Effect > 0);
            ApplyHapticChange(GameSettingVariables.Haptic > 0);
            ApplyLanguageChange(GameSettingVariables.Language);
            ApplyFPSChange(GameSettingVariables.FPS);
        }

        private void ApplyMusicChange(bool isOn)
        {
            //TODO: Thêm logic để bật/tắt âm nhạc trong game nếu cần
        }

        private void ApplyEffectChange(bool isOn)
        {
            //TODO: Thêm logic để bật/tắt hiệu ứng âm thanh trong game nếu cần
        }

        private void ApplyHapticChange(bool isOn)
        {
            //TODO: Thêm logic để bật/tắt rung điện thoại trong game nếu cần
        }
        
        private void ApplyLanguageChange(string language)
        {
            //TODO: Thêm logic để áp dụng thay đổi ngôn ngữ trong game nếu cần
        }

        private void ApplyFPSChange(int fps)
        {
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = fps;
        }

        #endregion

        #region Helper Methods

        private string GetLanguageFromDropdownIndex(int index)
        {
            // Kiểm tra an toàn index để tránh OutOfRangeException
            if (index < 0 || index >= _panel.LanguageDropdown.options.Count) return string.Empty;
            return _panel.LanguageDropdown.options[index].text;
        }

        private int GetFPSFromDropdownIndex(int index)
        {
            if (index >= 0 && index < _panel.FPSDropdown.options.Count)
            {
                if (int.TryParse(_panel.FPSDropdown.options[index].text, out int fps))
                {
                    return fps;
                }
            }
            return 60; // Default FPS
        }

        #endregion
    }
}

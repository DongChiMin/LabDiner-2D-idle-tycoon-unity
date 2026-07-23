using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace LabDiner.GameSetting.UI
{
    public class GameSettingPanel : BasePanel
    {
        public Button CloseButton => _closeButton;
        public ToggleButton BtnMusic => _btnMusic;
        public ToggleButton BtnEffect => _btnEffect;
        public ToggleButton BtnHaptic => _btnHaptic;
        public Button BtnResetToDefault => _btnResetToDefault;
        public TMP_Dropdown LanguageDropdown => _dropdownLanguage;
        public TMP_Dropdown FPSDropdown => _dropdownFPS;
        public Button BtnTermsOfService => _btnTermsOfService;
        public Button BtnPrivacyPolicy => _btnPrivacyPolicy;

        [Header("UI References")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private ToggleButton _btnMusic;
        [SerializeField] private ToggleButton _btnEffect;
        [SerializeField] private ToggleButton _btnHaptic;

        [SerializeField] private TMP_Dropdown _dropdownLanguage;
        [SerializeField] private TMP_Dropdown _dropdownFPS;


        [SerializeField] private Button _btnResetToDefault;

        [SerializeField] private Button _btnTermsOfService;
        [SerializeField] private Button _btnPrivacyPolicy;

        public void Setup()
        {
            _btnMusic.FetchData(GameSettingVariables.Music > 0);
            _btnEffect.FetchData(GameSettingVariables.Effect > 0);
            _btnHaptic.FetchData(GameSettingVariables.Haptic > 0);
            _dropdownLanguage.SetValueWithoutNotify(GetLanguageDropdownIndex(GameSettingVariables.Language));
            _dropdownFPS.SetValueWithoutNotify(GetFPSDropdownIndex(GameSettingVariables.FPS));

            _dropdownLanguage.RefreshShownValue();
            _dropdownFPS.RefreshShownValue();
        }


        private int GetLanguageDropdownIndex(string language)
        {
            int index = _dropdownLanguage.options.FindIndex(opt => opt.text == language);
            return index != -1 ? index : 0;
        }

        private int GetFPSDropdownIndex(int fps)
        {
            int index = _dropdownFPS.options.FindIndex(opt =>
                int.TryParse(opt.text, out int optionFPS) && optionFPS == fps);

            return index != -1 ? index : 0;
        }
    }
}

using LabDiner.Shared.UI;
using UnityEngine; 
using UnityEngine.UI;
namespace LabDiner.GameSetting.UI{
public class GameSettingPanel : BasePanel
{
    public Button CloseButton => _closeButton;

    [SerializeField] private Button _closeButton;
}
}

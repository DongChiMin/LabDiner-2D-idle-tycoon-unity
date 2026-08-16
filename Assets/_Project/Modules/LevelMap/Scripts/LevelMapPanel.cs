using LabDiner.Shared.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.LevelMap.UI
{
public class LevelMapPanel : BasePanel
{
    public Button CloseButton => _closeButton;

    [Header("UI")]
    [SerializeField] private Button _closeButton;
    [SerializeField] private TextMeshProUGUI _titleText;

    public void SetTitle(string title)
    {
        _titleText.text = title;
    }
}
}

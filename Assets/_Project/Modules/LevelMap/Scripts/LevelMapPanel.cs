using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.LevelMap.UI
{
public class LevelMapPanel : BasePanel
{
    public Button CloseButton => _closeButton;

    [Header("UI")]
    [SerializeField] private Button _closeButton;
}
}

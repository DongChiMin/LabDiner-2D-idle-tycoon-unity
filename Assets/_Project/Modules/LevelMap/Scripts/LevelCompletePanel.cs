using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.LevelMap.UI
{
    public class LevelCompletePanel : BasePanel
    {
        public Button ContinueButton => _continueButton;
        [SerializeField] private Button _continueButton;
    }
}

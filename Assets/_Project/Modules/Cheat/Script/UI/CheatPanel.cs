using LabDiner.Shared.UI;
using UnityEngine;
using UnityEngine.UI;

public class CheatPanel : BasePanel
{
    public Button CloseButton => _closeButton;
    [SerializeField] private Button _closeButton;

    public void Setup()
    {
        // Thiết lập các thành phần UI của CheatPanel nếu cần
    }
}

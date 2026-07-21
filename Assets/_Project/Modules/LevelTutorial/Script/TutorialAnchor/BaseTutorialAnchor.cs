using System;
using LabDiner.Restaurant.SO;
using UnityEditor;
using UnityEngine;

public class BaseTutorialAnchor : MonoBehaviour
{
    public TutorialSO TutorialSO => _tutorialSO;
    public Vector2 FocusSize => focusSize;
    public Vector2 FocusPosition => (Vector2) Camera.main.WorldToScreenPoint(_focusTarget.position) + focusPositionOffset;
    public float EffectDelay => effectDelay;
    public float EffectDuration => effectDuration;

    public Action OnTutorialStarted;      // 1. Kích hoạt hiển thị Tutorial
    public Action OnTutorialInteracted;   // 2. Người chơi tương tác thành công (để Controller chuyển bước/chuyển tutorial)
    public Action OnTutorialCompleted;    // 3. Đánh dấu hoàn thành thực sự (ghi file Save)

    [Header("Config")]
    [SerializeField] private TutorialSO _tutorialSO;
    [SerializeField] private Transform _focusTarget;
    [SerializeField] private Vector2 focusSize = new Vector2(150f, 150f);
    [SerializeField] private Vector2 focusPositionOffset = Vector2.zero;
    [SerializeField] private float effectDelay = 0f;
    [SerializeField] private float effectDuration = 0.5f;

    [Header("Debug")]
    [SerializeField] private bool debugDrawGizmos = true;

    void Awake()
    {
        LevelTutorialController.Instance.RegisterTutorialAnchor(this);
    }

    protected void StartTutorial()
    {
        Debug.Log("Vị trí: " + FocusPosition + ", Kích thước: " + FocusSize);
        OnTutorialStarted?.Invoke();
    }

    protected void InteractTutorial()
    {
        Debug.Log("[Tutorial] Tutorial được tương tác: " + _tutorialSO.ID);
        OnTutorialInteracted?.Invoke();
    }

    protected void CompleteTutorial()
    {
        OnTutorialCompleted?.Invoke();
    }


//Vẽ vùng focus
void OnDrawGizmosSelected()
    {
        if (!debugDrawGizmos) return;

#if UNITY_EDITOR
        // Lấy camera hiện tại đang dùng để render Scene view hoặc Game view
        Camera cam = Camera.main;
        if (cam == null) return;

        // 1. Chuyển vị trí Object từ World Space sang Screen Space (Pixel)
        Vector3 screenPos = cam.WorldToScreenPoint(transform.position);

        // Nếu Object nằm phía sau camera thì không vẽ
        if (screenPos.z < 0) return;

        // 2. Tính toán 4 góc của ô vuông dựa trên focusSize (Pixel)
        float halfX = focusSize.x / 2f;
        float halfY = focusSize.y / 2f;

        Vector3 bottomLeft = cam.ScreenToWorldPoint(new Vector3(screenPos.x - halfX, screenPos.y - halfY, screenPos.z));
        Vector3 topLeft = cam.ScreenToWorldPoint(new Vector3(screenPos.x - halfX, screenPos.y + halfY, screenPos.z));
        Vector3 topRight = cam.ScreenToWorldPoint(new Vector3(screenPos.x + halfX, screenPos.y + halfY, screenPos.z));
        Vector3 bottomRight = cam.ScreenToWorldPoint(new Vector3(screenPos.x + halfX, screenPos.y - halfY, screenPos.z));

        // 3. Vẽ các đường nối bằng Handles
        Handles.color = Color.yellow;
        Handles.DrawLine(bottomLeft, topLeft);
        Handles.DrawLine(topLeft, topRight);
        Handles.DrawLine(topRight, bottomRight);
        Handles.DrawLine(bottomRight, bottomLeft);
#endif
    }
}

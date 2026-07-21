using System;
using System.Collections;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using LabDiner.Restaurant.SO;
using LabDiner.Shared.UI;
using UnityEngine;

public class LevelTutorialPanel : MonoBehaviour
{
    [SerializeField] private RectTransform _unmask;
    [SerializeField] private VerticalSwingEffect _arrow;
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private RectTransform _unmaskParent;
    [SerializeField] private float arrowOffset = 50f;

    void Start()
    {
        // 1. Setup _unmaskParent: Full màn hình (Stretch - Stretch), Pivot (0.5, 0.5)
        _unmaskParent.anchorMin = Vector2.zero;
        _unmaskParent.anchorMax = Vector2.one;
        _unmaskParent.sizeDelta = Vector2.zero;
        _unmaskParent.anchoredPosition = Vector2.zero;
        _unmaskParent.pivot = new Vector2(0f, 0f);

        // 2. Setup _unmask: Anchor Bottom-Left (0,0), Pivot (0,0)
        _unmask.anchorMin = Vector2.zero;
        _unmask.anchorMax = Vector2.zero;
        _unmask.pivot = Vector2.zero;

        // 3. Setup _arrow: Anchor Bottom-Left (0,0), Pivot (0.5, 0) để căn giữa đáy mũi tên
        RectTransform swingRect = _arrow.transform as RectTransform;
        if (swingRect != null)
        {
            swingRect.anchorMin = Vector2.zero;
            swingRect.anchorMax = Vector2.zero;
            swingRect.pivot = swingRect.localScale.y == -1 ? new Vector2(0.5f, 1f) : new Vector2(0.5f, 0f);
        }
    }

    public void StartFocusTutorial(float timeDelay, float effectDuration, Vector2 focusPosition, Vector2 focusSize)
    {
        StartCoroutine(StartFocus(timeDelay, effectDuration, focusPosition, focusSize));
    }

    IEnumerator StartFocus(float timeDelay, float effectDuration, Vector2 focusPosition, Vector2 focusSize)
{
    Debug.Log("[Tutorial] StartFocusTutorial: Position = " + focusPosition + ", Size = " + focusSize);
    _arrow.gameObject.SetActive(false);
    _canvasGroup.alpha = 0f;
    _unmaskParent.gameObject.SetActive(true);

    yield return new WaitForSeconds(timeDelay);

    Vector2 localPoint;
    RectTransformUtility.ScreenPointToLocalPointInRectangle(
        _unmaskParent, 
        focusPosition, 
        null, 
        out localPoint
    );

    // 1. VẪN TÍNH TOÁN THEO PIVOT (0, 0) CỦA HỆ THỐNG BẠN
    // (Đảm bảo ban đầu _unmask đang có Pivot là 0, 0)
    _unmask.pivot = new Vector2(0f, 0f);
    _unmask.sizeDelta = focusSize;
    // Chuyển dịch tâm về góc dưới trái dựa theo công thức cũ của bạn
    _unmask.anchoredPosition = localPoint - new Vector2(focusSize.x / 2f, focusSize.y / 2f);

    // 2. CHUYỂN PIVOT SANG (0.5, 0.5) ĐỂ CHUẨN BỊ CHẠY ANIMATION SQUISHY
    // Gọi hàm phụ trợ để chuyển Pivot mà không làm lệch vị trí thực tế của Unmask trên màn hình
    SetPivotSmart(_unmask, new Vector2(0.5f, 0.5f));

    // 3. CHẠY ANIMATION SQUISHY TỪ TÂM (0.5, 0.5)
    _unmask.localScale = Vector3.zero; // Bắt đầu scale từ 0

    DOTween.To(() => _canvasGroup.alpha, x => _canvasGroup.alpha = x, 1f, effectDuration * 0.5f)
           .SetEase(Ease.OutQuad);

    bool isTweenComplete = false;
    Sequence squishySequence = DOTween.Sequence();

    squishySequence.Append(
        DOTween.To(() => _unmask.localScale, x => _unmask.localScale = x, new Vector3(1.25f, 1.15f, 1f), effectDuration * 0.4f)
               .SetEase(Ease.OutQuad)
    );

    squishySequence.Append(
        DOTween.To(() => _unmask.localScale, x => _unmask.localScale = x, new Vector3(0.9f, 1.08f, 1f), effectDuration * 0.3f)
               .SetEase(Ease.OutQuad)
    );

    squishySequence.Append(
        DOTween.To(() => _unmask.localScale, x => _unmask.localScale = x, Vector3.one, effectDuration * 0.3f)
               .SetEase(Ease.OutElastic, 1f, 0.5f)
    );

    squishySequence.OnComplete(() => isTweenComplete = true);

    yield return new WaitUntil(() => isTweenComplete);

    // 4. TRẢ PIVOT VỀ LẠI (0, 0) ĐỂ KHÔNG ẢNH HƯỞNG HỆ THỐNG CỦA BẠN VỀ SAU
    SetPivotSmart(_unmask, new Vector2(0f, 0f));

    // 5. HIỆU ỨNG MŨI TÊN (Tính toán dựa trên việc Pivot của _unmask đã trả về 0, 0)
    RectTransform swingRect = _arrow.transform as RectTransform;
    if (swingRect != null)
    {
        // Vì unmask có pivot là (0,0), góc dưới trái là anchoredPosition gốc
        // Toạ độ X ở giữa: anchoredPosition.x + một nửa chiều rộng
        // Toạ độ Y ở đỉnh: anchoredPosition.y + chiều cao + khoảng cách arrowOffset
        float targetX = _unmask.anchoredPosition.x + (focusSize.x / 2f);
        float targetY = _unmask.anchoredPosition.y + focusSize.y + arrowOffset;

        swingRect.anchoredPosition = new Vector2(targetX, targetY);
    }

    _arrow.gameObject.SetActive(true);
    _arrow.Show();
}

    public void StopFocusTutorial(float timeDelay)
    {
        StartCoroutine(StopFocus(timeDelay));
    }


    IEnumerator StopFocus(float timeDelay){
        float elapsedTime = timeDelay;
        while (elapsedTime > 0)
        {
            elapsedTime -= Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / timeDelay);
            _canvasGroup.alpha = alpha;
            yield return null;
        }
        _arrow.Hide();
        _unmaskParent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Hàm phụ trợ: Thay đổi Pivot của RectTransform nhưng giữ nguyên vị trí hiển thị trên màn hình.
    /// </summary>
    private void SetPivotSmart(RectTransform rectTransform, Vector2 newPivot)
    {
        Vector2 size = rectTransform.rect.size;
        Vector2 deltaPivot = rectTransform.pivot - newPivot;
        Vector3 deltaPosition = new Vector3(deltaPivot.x * size.x, deltaPivot.y * size.y) * rectTransform.localScale.x;
        
        rectTransform.pivot = newPivot;
        rectTransform.localPosition -= deltaPosition;
    }
}

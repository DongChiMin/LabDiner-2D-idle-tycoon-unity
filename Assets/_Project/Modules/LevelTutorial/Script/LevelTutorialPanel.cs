using System;
using System.Collections;
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

    public void StartFocusTutorial(float timeDelay, float duration, Vector2 focusPosition, Vector2 focusSize)
    {
        StartCoroutine(StartFocus(timeDelay, duration, focusPosition, focusSize));
    }

    IEnumerator StartFocus(float timeDelay, float duration, Vector2 focusPosition, Vector2 focusSize)
    {
        _arrow.gameObject.SetActive(false);
        _canvasGroup.alpha = 0f;
        _unmaskParent.gameObject.SetActive(true);

        yield return new WaitForSeconds(timeDelay);

        Debug.Log($"[LevelTutorialPanel] StartFocus: focusPosition={focusPosition}, focusSize={focusSize}");
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _unmaskParent, 
            focusPosition, 
            null, 
            out localPoint
        );
        Debug.Log($"[LevelTutorialPanel] LocalPoint: {localPoint}");

        _unmask.anchoredPosition = localPoint - new Vector2(focusSize.x / 2, focusSize.y / 2);
        Vector2 startSize = focusSize * 5f;
        _unmask.sizeDelta = startSize;
        
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            _canvasGroup.alpha = alpha;

            _unmask.sizeDelta = Vector2.Lerp(startSize, focusSize, alpha);
            yield return null;
        }
        
        _canvasGroup.alpha = 1f;

        //Hiệu ứng mũi tên
        RectTransform swingRect = _arrow.transform as RectTransform;
        if (swingRect != null)
        {
            // 1. Tính toán vị trí đỉnh dựa trên Pivot(0,0) của _unmask
            float targetX = _unmask.anchoredPosition.x + (focusSize.x / 2f);
            float targetY = _unmask.anchoredPosition.y + focusSize.y + arrowOffset;

            // 2. Gán vị trí cho mũi tên (Đảm bảo swingRect cũng chung cha với _unmask để đồng bộ tọa độ)
            swingRect.anchoredPosition = new Vector2(targetX, targetY);
        }

        _arrow.gameObject.SetActive(true);
        _arrow.Show();
    }
}

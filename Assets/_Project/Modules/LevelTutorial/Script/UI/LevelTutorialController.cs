using System;
using System.Collections;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;
using LabDiner.Shared;
using LabDiner.Shared.DesignPattern;
using UnityEngine;

public class LevelTutorialController : Singleton<LevelTutorialController>
{
    [Header("Data")]
    [SerializeField] private TutorialRepository _tutorialRepository;

    [Header("References")]
    [SerializeField] private LevelTutorialPanel _levelTutorialPanel;

    private TutorialSaveData _tutorialSaveData;
    
    //Danh sách các anchor có trên scene
    private List<BaseTutorialAnchor> tutorialAnchors = new List<BaseTutorialAnchor>();
    
    protected override void Awake()
    {
        base.Awake();
        _tutorialSaveData = PlayerSaveFile.LoadFromFile().tutorialData;
    }

    //Sẽ được gọi ở Awake
    public void RegisterTutorialAnchor(BaseTutorialAnchor anchor)
    {
        tutorialAnchors.Add(anchor);
        anchor.OnTutorialStarted += () => OnTutorialStarted(anchor);    //Đăng ký action start cho toàn bộ tutorialAnchor
        anchor.OnTutorialInteracted += () => OnTutorialInteracted(anchor);  //Đăng ký action interact cho toàn bộ tutorialAnchor
        anchor.OnTutorialCompleted += () => OnTutorialCompleted(anchor);    //Đăng ký action complete cho toàn bộ tutorialAnchor
    }

    //Hàm xử lý khi bất kì tutorial nào bắt đầu
    private void OnTutorialStarted(BaseTutorialAnchor anchor)
    {
        Debug.Log("[Tutorial] Tutorial started: " + anchor.TutorialSO.ID);
        //Nếu tutorial này đã hoàn thành thì không hiển thị nữa
        if (_tutorialSaveData.IsTutorialCompleted(anchor.TutorialSO.ID))
        {
            return;
        }

        //Hiển thị tutorial sau 1 frame
        StartCoroutine(WaitForNextFrame(() =>
        {
            _levelTutorialPanel.StartFocusTutorial(anchor.EffectDelay, anchor.EffectDuration, anchor.FocusPosition, anchor.FocusSize);
        }));
    }

    IEnumerator WaitForNextFrame(Action callback)
    {
        yield return null; // Wait for the next frame
        callback?.Invoke();
    }

    //Hàm xử lý khi bất kì tutorial nào được tương tác
    private void OnTutorialInteracted(BaseTutorialAnchor anchor)
    {
        // Tắt focus cũ đi để chờ Anchor tiếp theo tự gọi OnTutorialStarted
        _levelTutorialPanel.StopFocusTutorial(0); 
    }

    private void OnTutorialCompleted(BaseTutorialAnchor anchor)
    {
        //Đánh dấu tutorial đã hoàn thành
        _tutorialSaveData.MarkTutorialCompleted(anchor.TutorialSO.ID);
        _levelTutorialPanel.StopFocusTutorial(0);
    }
}

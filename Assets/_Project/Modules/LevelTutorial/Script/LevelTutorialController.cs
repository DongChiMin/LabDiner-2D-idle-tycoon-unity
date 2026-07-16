using System;
using LabDiner.Restaurant.SO;
using UnityEngine;

public class LevelTutorialController : MonoBehaviour
{
    public static string LEVEL1_TUTORIAL_1 = "Tutorial1";

    [Header("Runtimes")]
    [SerializeField] private TutorialRuntimeSO _tutorialRuntimeSO;
    [SerializeField] private ProgressSaveRuntimeSO _progressRuntimeSO;

    [Header("Item References")]
    [SerializeField] private LevelTutorialPanel _tutorialPanel;

    void OnEnable()
    {
        _tutorialRuntimeSO.OnCoreStationStartRun += HandleCoreStationStartRun;
    }

    void OnDisable()
    {
        _tutorialRuntimeSO.OnCoreStationStartRun -= HandleCoreStationStartRun;
    }

    //CoreStation bắt đầu chạy, chỉ tutorial ở level 1
    private void HandleCoreStationStartRun(Vector2 position)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(position);
        if(_progressRuntimeSO.PlayerSave.currentLevelIndex == 1 &&
        !_progressRuntimeSO.PlayerSave.tutorialData.IsTutorialCompleted(LEVEL1_TUTORIAL_1))
        {
            _tutorialPanel.StartFocusTutorial(
                timeDelay: 0f, 
                duration: 0.5f, 
                focusPosition: screenPoint, 
                focusSize: new Vector2(150f, 150f)
            );
        } 
    }
}

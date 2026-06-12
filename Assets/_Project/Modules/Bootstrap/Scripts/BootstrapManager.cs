using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using LabDiner.Shared.DesignPattern;
using System.Collections.Generic;

public class BootstrapManager : Singleton<BootstrapManager>
{

    [Header("UI References")]
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private CanvasGroup contentGroup;
    [SerializeField] private Slider progressBar;
    [SerializeField] private TextMeshProUGUI statusText;
    [SerializeField] private TextMeshProUGUI percentageText;

    [Header("Loading Settings")]
    [SerializeField] private float minDisplayDuration = 1.5f; // Thời gian loading hiển thị tối thiểu (VD: 1.5 giây)
    [SerializeField] private float fadeOutDuration = 0.5f;
    private float loadStartTime;    // Thời điểm bắt đầu load game, dùng để đảm bảo thời gian hiển thị tối thiểu

    //load thanh slider
    [SerializeField] private float maxProgressSpeed = 1.0f; // Tốc độ tối đa của progress bar (ít nhất mất 1 giây để đạt 100%)
    private Coroutine sliderCoroutine;
    
    // Danh sách lưu trữ trọng số của các phase
    private Dictionary<string, float> phaseWeights = new Dictionary<string, float>();
    private float totalWeight = 0f;
    private float accumulatedWeightPriorToCurrent = 0f;

    private bool isAllPhasesCompleted = false;

    private void Start()
    {
        // Khi game mở lên, kích hoạt load game ngay
        StartCoroutine(LoadGameRoutine());
    }

    #region API
    
    public void RegisterLoadPhases(string phaseName, float weight)
    {
        if (!phaseWeights.ContainsKey(phaseName))
        {
            phaseWeights.Add(phaseName, weight);
            totalWeight += weight;
        }

        //Phải đăng ký tất cả các phase trước khi bắt đầu load
        accumulatedWeightPriorToCurrent = 0f;
        isAllPhasesCompleted = false;
    }
    
    public void CompletePhase(string phaseName)
    {
        if (phaseWeights.ContainsKey(phaseName))
        {
            accumulatedWeightPriorToCurrent += phaseWeights[phaseName];
            float progress = accumulatedWeightPriorToCurrent / totalWeight;

            if (sliderCoroutine != null)
            {
                StopCoroutine(sliderCoroutine);
            }
            sliderCoroutine = StartCoroutine(UpdateProgressBar(progress));

            statusText.text = $"Đang tải: {phaseName}";
        }

        if(accumulatedWeightPriorToCurrent >= totalWeight)
        {
            isAllPhasesCompleted = true;
            HideLoading();
        }
    }

    public void HideLoading()
    {
        StartCoroutine(FadeOutRoutine());
    }
    #endregion

    #region Private Methods
    private IEnumerator LoadGameRoutine()
    {
        loadStartTime = Time.time;
        // 1. Hiện màn hình loading che full
        string scenePhase = "LoadScene";
        RegisterLoadPhases(scenePhase, 30f);

        // 2. Load scene Gameplay theo dạng Additive (Chồng lên)
        AsyncOperation op = SceneManager.LoadSceneAsync("Gameplay", LoadSceneMode.Additive);
        
        // Cập nhật progress của việc load scene cứng (thường chiếm 0% -> 90%)
        while (!op.isDone)
        {
            yield return null;
        }
        CompletePhase(scenePhase);

        // Kích hoạt scene mới load làm scene chính để các script lấy đúng MainCamera
        Scene gameplayScene = SceneManager.GetSceneByName("Gameplay");
        SceneManager.SetActiveScene(gameplayScene);

        // LƯU Ý: Đến đây scene Gameplay đã load xong, nhưng CHƯA TẮT LOADING. 
        // Chúng ta sẽ đợi LevelBuilder bên scene Gameplay tự chạy các phase của nó.
    }

    private IEnumerator FadeOutRoutine()
    {
        if (isAllPhasesCompleted)
        {
            while (progressBar.value < 1f)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.2f);
        }

        // Tính xem game đã load được bao lâu rồi
        float timeElapsed = Time.time - loadStartTime;
        
        // Nếu thời gian load thực tế nhỏ hơn thời gian tối thiểu mong muốn
        if (timeElapsed < minDisplayDuration)
        {
            // Bắt game "chờ thêm" cho đủ thời gian tối thiểu
            yield return new WaitForSeconds(minDisplayDuration - timeElapsed);
        }

        //Thực hiện fade out
        float duration = fadeOutDuration;
        contentGroup.alpha = 0;
        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1, 0, time / duration);
            yield return null;
        }
        canvasGroup.blocksRaycasts = false;
    }

    private IEnumerator UpdateProgressBar(float targetProgress)
    {
        float startProgress = progressBar.value;
        float timeElapsed = 0f;

        while (timeElapsed < maxProgressSpeed)
        {
            timeElapsed += Time.deltaTime;
            float currentProgress = Mathf.Lerp(startProgress, targetProgress, timeElapsed / maxProgressSpeed);
            
            progressBar.value = currentProgress;
            percentageText.text = $"{(int)(currentProgress * 100)}%";
            
            yield return null;
        }
        progressBar.value = targetProgress;
        percentageText.text = $"{(int)(targetProgress * 100)}%";
    }
    #endregion
}
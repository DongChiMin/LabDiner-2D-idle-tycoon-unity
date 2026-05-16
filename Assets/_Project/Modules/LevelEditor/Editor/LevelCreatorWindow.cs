using UnityEngine;
using UnityEditor;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Event;
using System.Collections.Generic;
using System.IO;

public class LevelCreatorWindow : EditorWindow
{
    const string LEVEL_FOLDER = "Assets/_Project/Modules/Restaurant/Data/Levels/Data";

    // --- Level Info ---
    private string levelFileName = "Level_1";
    private GameObject levelPrefab;
    private int levelIndex;
    private string levelDisplayName;
    private LevelUpgradeEvent guestQuantityEvent;

    // --- Lists ---
    private List<BaseUpgradeSO> upgrades = new List<BaseUpgradeSO>();
    private List<BaseGemMissionSO> missions = new List<BaseGemMissionSO>();
    private List<CoreStationSO> coreStations = new List<CoreStationSO>();
    private BaseGemMissionSO finalMission;

    // --- Settings ---
    private bool waitingLine;
    private int maxUniqueStations = 2;
    private int maxTotalQtyPerOrder = 5;
    private float minCamY, maxCamY;

    private Vector2 scrollPos;

    [MenuItem("LabDiner/Tools/Level Creator")]
    public static void ShowWindow()
    {
        GetWindow<LevelCreatorWindow>("Level Creator");
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // --- SECTION 1: IDENTITY ---
        DrawHeader("1. IDENTITY & PREFAB");
        EditorGUILayout.BeginVertical("box");
        levelFileName = EditorGUILayout.TextField("Asset Name", levelFileName);
        levelDisplayName = EditorGUILayout.TextField("Display Name", levelDisplayName);
        levelIndex = EditorGUILayout.IntField("Level Index", levelIndex);
        levelPrefab = (GameObject)EditorGUILayout.ObjectField("Level Prefab", levelPrefab, typeof(GameObject), false);
        guestQuantityEvent = (LevelUpgradeEvent)EditorGUILayout.ObjectField("Guest Qty Event", guestQuantityEvent, typeof(LevelUpgradeEvent), false);
        EditorGUILayout.EndVertical();

        // --- SECTION 2: CORE STATIONS ---
        EditorGUILayout.Space(10);
        DrawHeader("2. CORE STATIONS");
        EditorGUILayout.BeginVertical("box");
        DrawSOList(coreStations, "Station");
        
        GUI.backgroundColor = Color.cyan;
        if (GUILayout.Button("✨ Open Station Creator Tool"))
        {
            // Gọi Tool cũ của bạn ở đây
            StationCreatorWindow.ShowWindow();
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.EndVertical();

        // --- SECTION 3: UPGRADES & MISSIONS ---
        EditorGUILayout.Space(10);
        DrawHeader("3. UPGRADES & MISSIONS");
        EditorGUILayout.BeginVertical("box");
        DrawSOList(upgrades, "Upgrade");
        EditorGUILayout.Space(5);
        DrawSOList(missions, "Mission");
        finalMission = (BaseGemMissionSO)EditorGUILayout.ObjectField("Final Mission", finalMission, typeof(BaseGemMissionSO), false);
        EditorGUILayout.EndVertical();

        // --- SECTION 4: CONFIG & CAMERA ---
        EditorGUILayout.Space(10);
        DrawHeader("4. CONFIG & CAMERA");
        EditorGUILayout.BeginVertical("box");
        waitingLine = EditorGUILayout.Toggle("Has Waiting Line", waitingLine);
        maxUniqueStations = EditorGUILayout.IntField("Max Unique Stations", maxUniqueStations);
        maxTotalQtyPerOrder = EditorGUILayout.IntField("Max Qty Per Order", maxTotalQtyPerOrder);
        
        EditorGUILayout.LabelField("Camera Bounds", EditorStyles.miniBoldLabel);
        minCamY = EditorGUILayout.FloatField("Min Vertical (Y)", minCamY);
        maxCamY = EditorGUILayout.FloatField("Max Vertical (Y)", maxCamY);
        EditorGUILayout.EndVertical();

        // --- ACTION ---
        EditorGUILayout.Space(20);
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("CREATE LEVEL CONFIG", GUILayout.Height(50)))
        {
            CreateLevelConfig();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndScrollView();
    }

    private void DrawHeader(string title)
    {
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.normal.textColor = new Color(0.3f, 0.8f, 1f);
        EditorGUILayout.LabelField(title, style);
    }

    // Hàm vẽ List SO generic để tái sử dụng
    private void DrawSOList<T>(List<T> list, string label) where T : ScriptableObject
    {
        EditorGUILayout.LabelField($"{label} List", EditorStyles.miniBoldLabel);
        for (int i = 0; i < list.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            list[i] = (T)EditorGUILayout.ObjectField($"{label} {i}", list[i], typeof(T), false);
            if (GUILayout.Button("X", GUILayout.Width(20))) { list.RemoveAt(i); break; }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button($"+ Add {label} Slot")) list.Add(null);
    }

    private void CreateLevelConfig()
    {
        string path = $"{LEVEL_FOLDER}/{levelFileName}.asset";
        if (!Directory.Exists(LEVEL_FOLDER)) Directory.CreateDirectory(LEVEL_FOLDER);

        LevelConfigSO config = ScriptableObject.CreateInstance<LevelConfigSO>();
        
        // Gán dữ liệu
        config.LevelPrefab = levelPrefab;
        config.LevelIndex = levelIndex;
        config.LevelName = levelDisplayName;
        config.AvailableUpgrades = new List<BaseUpgradeSO>(upgrades);
        config.AvailableMissions = new List<BaseGemMissionSO>(missions);
        config.FinalMission = finalMission;
        config.CoreStations = new List<CoreStationSO>(coreStations);
        config.WaitingLine = waitingLine;
        config.MaxUniqueStations = maxUniqueStations;
        config.MaxTotalQtyPerOrder = maxTotalQtyPerOrder;
        config.minVerticalPos = minCamY;
        config.maxVerticalPos = maxCamY;

        // Vì _onUpgradeGuestQuantity là private, bạn có thể cần [SerializeField] 
        // hoặc dùng Reflection nếu không muốn sửa SO. 
        // Ở đây giả định bạn đã sửa field đó thành public hoặc có property.
        // config.SetGuestUpgradeEvent(guestQuantityEvent); // Ví dụ

        AssetDatabase.CreateAsset(config, AssetDatabase.GenerateUniqueAssetPath(path));
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = config;
        Debug.Log($"<color=lime>Level Creator:</color> Created {levelFileName} successfully!");
    }
}
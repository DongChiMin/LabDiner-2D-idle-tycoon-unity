using UnityEngine;
using UnityEditor;
using LabDiner.Restaurant.SO;
using System.IO;
using System.Collections.Generic;

public class StationCreatorWindow : EditorWindow
{
    // Folder Paths
    const string DISH_FOLDER = "Assets/_Project/Modules/Restaurant/Data/CoreStations/Dish";
    const string STATION_FOLDER = "Assets/_Project/Modules/Restaurant/Data/CoreStations/CoreStation";
    const string STAR_FOLDER = "Assets/_Project/Modules/Restaurant/Data/CoreStations/Star";

    [Header("Dish Information")]
    private string dishName = "New Dish";
    private Sprite dishIcon;
    private Sprite stationIcon;

    [Header("Level & Scaling")]
    private string stationFileName = "New Station";
    private int levelPerStar = 10;
    private float baseProcessTime = 5f;
    private double baseProfit = 10;
    private double baseUpgradeCost = 15;
    private float costMultiplier = 1.07f;
    private float profitMultiplier = 1.15f;
    
    [Header("Stars List")]
    private List<StationStarSO> stationStars = new List<StationStarSO>();
    private Vector2 scrollPos;

    [MenuItem("LabDiner/Tools/Integrated Station Creator")]
    public static void ShowWindow()
    {
        GetWindow<StationCreatorWindow>("Station Creator");
    }

    private void OnGUI()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);

        // --- PHẦN 1: DISH ---
        DrawHeader("1. DISH INFORMATION");
        EditorGUILayout.BeginVertical("box");
        dishName = EditorGUILayout.TextField("Dish Name", dishName);
        dishIcon = (Sprite)EditorGUILayout.ObjectField("Dish Icon (UI)", dishIcon, typeof(Sprite), false);
        stationIcon = (Sprite)EditorGUILayout.ObjectField("Station Icon (World)", stationIcon, typeof(Sprite), false);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        // --- PHẦN 2: CORE STATION BALANCING ---
        DrawHeader("2. STATION BALANCING (STATS)");
        EditorGUILayout.BeginVertical("box");
        stationFileName = EditorGUILayout.TextField("Asset Name", stationFileName);
        levelPerStar = EditorGUILayout.IntField("Levels Per Star", levelPerStar);
        baseProcessTime = EditorGUILayout.FloatField("Base Process Time (s)", baseProcessTime);
        
        EditorGUILayout.Space(5);
        baseProfit = EditorGUILayout.DoubleField("Base Profit", baseProfit);
        profitMultiplier = EditorGUILayout.FloatField("Profit Multiplier", profitMultiplier);
        
        EditorGUILayout.Space(5);
        baseUpgradeCost = EditorGUILayout.DoubleField("Base Upgrade Cost", baseUpgradeCost);
        costMultiplier = EditorGUILayout.FloatField("Cost Multiplier", costMultiplier);
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(10);

        // --- PHẦN 3: STARS ---
        DrawHeader("3. STATION STARS (MILESTONES)");
        EditorGUILayout.BeginVertical("box");
        
        for (int i = 0; i < stationStars.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            stationStars[i] = (StationStarSO)EditorGUILayout.ObjectField($"Star {i + 1}", stationStars[i], typeof(StationStarSO), false);
            
            GUI.color = Color.cyan;
            if (GUILayout.Button("New", GUILayout.Width(40)))
                stationStars[i] = CreateNewStarAsset(i + 1);
            
            GUI.color = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
                stationStars.RemoveAt(i);
            
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();
        }

        if (GUILayout.Button("+ Add Star Milestone", GUILayout.Height(25)))
            stationStars.Add(null);
        
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(25);

        // --- ACTION BUTTONS ---
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("GENERATE ALL ASSETS", GUILayout.Height(50)))
        {
            CreateEverything();
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        // --- QUICK NAV ---
        GUILayout.Label("Quick Access Folders", EditorStyles.miniBoldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("📁 Dishes")) FocusFolder(DISH_FOLDER);
        if (GUILayout.Button("📁 Stations")) FocusFolder(STATION_FOLDER);
        if (GUILayout.Button("📁 Stars")) FocusFolder(STAR_FOLDER);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.EndScrollView();
    }

    private void DrawHeader(string title)
    {
        GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
        style.normal.textColor = new Color(0.3f, 0.8f, 1f);
        EditorGUILayout.LabelField(title, style);
    }

    private StationStarSO CreateNewStarAsset(int index)
    {
        EnsureDirectory(STAR_FOLDER);
        StationStarSO newStar = ScriptableObject.CreateInstance<StationStarSO>();
        string fileName = $"Star_{index}_{stationFileName.Replace(" ", "")}.asset";
        string fullPath = AssetDatabase.GenerateUniqueAssetPath($"{STAR_FOLDER}/{fileName}");
        AssetDatabase.CreateAsset(newStar, fullPath);
        AssetDatabase.SaveAssets();
        return newStar;
    }

    private void CreateEverything()
    {
        if (dishIcon == null || stationIcon == null)
        {
            if (!EditorUtility.DisplayDialog("Warning", "Icons are missing. Still continue?", "Yes", "Cancel")) return;
        }

        EnsureDirectory(DISH_FOLDER);
        EnsureDirectory(STATION_FOLDER);

        // 1. DishSO
        DishSO newDish = ScriptableObject.CreateInstance<DishSO>();
        newDish.Name = dishName;
        newDish.Dishicon = dishIcon;
        newDish.StationIcon = stationIcon;
        string dishPath = AssetDatabase.GenerateUniqueAssetPath($"{DISH_FOLDER}/Dish_{dishName.Replace(" ", "")}.asset");
        AssetDatabase.CreateAsset(newDish, dishPath);

        // 2. CoreStationSO - Gán tất cả thuộc tính từ Editor
        CoreStationSO newStation = ScriptableObject.CreateInstance<CoreStationSO>();
        newStation.Dish = newDish;
        newStation.LevelPerStar = levelPerStar;
        newStation.BaseProcessTime = baseProcessTime;
        newStation.BaseProfit = baseProfit;
        newStation.ProfitMultiplier = profitMultiplier;
        newStation.BaseUpgradeCost = baseUpgradeCost;
        newStation.CostMultiplier = costMultiplier;
        newStation.StationStars = new List<StationStarSO>(stationStars);

        string stationPath = AssetDatabase.GenerateUniqueAssetPath($"{STATION_FOLDER}/{stationFileName}.asset");
        AssetDatabase.CreateAsset(newStation, stationPath);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newStation;

        Debug.Log($"<color=lime>LabDiner Editor:</color> Success created {stationFileName}");
    }

    private void EnsureDirectory(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }
    }

    private void FocusFolder(string path)
    {
        EnsureDirectory(path);
        Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
        if (obj != null)
        {
            EditorGUIUtility.PingObject(obj);
            Selection.activeObject = obj;
        }
    }
}
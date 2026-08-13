// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEditor.SceneManagement;
// using UnityEngine;

// namespace LabDiner.LevelEditor
// {
//     public class LevelVisualizerWindow : EditorWindow
//     {
//         private Vector2 _scrollPos;
//         private readonly Dictionary<LevelDesignStep, List<LevelElementMarker>> _groupedMarkers = new();
//         private readonly Dictionary<LevelDesignStep, bool> _stepFoldouts = new();
//         private bool _isInPrefabMode = false;

//         [MenuItem("Tools/LabDiner/Level Element Manager")]
//         public static void OpenWindow()
//         {
//             LevelVisualizerWindow window = GetWindow<LevelVisualizerWindow>("Level Manager");
//             window.minSize = new Vector2(380, 480);
//             window.ScanMarkers();
//         }

//         private void OnEnable()
//         {
//             PrefabStage.prefabStageOpened += OnStageChanged;
//             PrefabStage.prefabStageClosing += OnStageChanged;

//             foreach (LevelDesignStep step in System.Enum.GetValues(typeof(LevelDesignStep)))
//             {
//                 _stepFoldouts[step] = true;
//             }
//         }

//         private void OnDisable()
//         {
//             PrefabStage.prefabStageOpened -= OnStageChanged;
//             PrefabStage.prefabStageClosing -= OnStageChanged;
//         }

//         private void OnStageChanged(PrefabStage stage) => ScanMarkers();

//         private void ScanMarkers()
//         {
//             _groupedMarkers.Clear();

//             List<LevelElementMarker> allMarkers = new List<LevelElementMarker>();
//             PrefabStage currentStage = PrefabStageUtility.GetCurrentPrefabStage();
//             _isInPrefabMode = currentStage != null;

//             if (_isInPrefabMode && currentStage.prefabContentsRoot != null)
//             {
//                 // Quét trong Prefab đang mở
//                 allMarkers = currentStage.prefabContentsRoot
//                     .GetComponentsInChildren<LevelElementMarker>(true).ToList();
//             }
//             else
//             {
//                 // Quét trong Scene thông thường
//                 allMarkers = FindObjectsByType<LevelElementMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
//             }

//             // Group theo Step và Order in Step
//             foreach (LevelDesignStep step in System.Enum.GetValues(typeof(LevelDesignStep)))
//             {
//                 var listInStep = allMarkers
//                     .Where(m => m.Step == step)
//                     .OrderBy(m => m.OrderInStep)
//                     .ToList();

//                 _groupedMarkers[step] = listInStep;
//             }

//             Repaint();
//         }

//         private void OnGUI()
//         {
//             EditorGUILayout.Space(5);
//             EditorGUILayout.LabelField("Level Design Steps Inspector", EditorStyles.boldLabel);

//             string modeText = _isInPrefabMode ? "PREFAB MODE" : "SCENE MODE";
//             EditorGUILayout.HelpBox($"[{modeText}] Quét được tổng cộng {_groupedMarkers.Values.Sum(list => list.Count)} elements.", MessageType.Info);

//             EditorGUILayout.Space(5);

//             if (GUILayout.Button("Refresh / Scan", GUILayout.Height(25)))
//             {
//                 ScanMarkers();
//             }

//             EditorGUILayout.Space(10);
//             EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

//             _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

//             foreach (LevelDesignStep step in System.Enum.GetValues(typeof(LevelDesignStep)))
//             {
//                 List<LevelElementMarker> markers = _groupedMarkers.ContainsKey(step) ? _groupedMarkers[step] : new List<LevelElementMarker>();

//                 EditorGUILayout.BeginVertical(EditorStyles.helpBox);

//                 // Header của từng Step
//                 _stepFoldouts[step] = EditorGUILayout.Foldout(
//                     _stepFoldouts[step], 
//                     $"{step}  [{markers.Count} items]", 
//                     true, 
//                     EditorStyles.foldoutHeader
//                 );

//                 // Hiển thị danh sách Element
//                 if (_stepFoldouts[step])
//                 {
//                     EditorGUI.indentLevel++;
//                     if (markers.Count == 0)
//                     {
//                         EditorGUILayout.LabelField("Chưa có element nào...", EditorStyles.miniLabel);
//                     }

//                     foreach (var marker in markers)
//                     {
//                         if (marker == null) continue;

//                         EditorGUILayout.BeginHorizontal();

//                         // Nút Select / Highlight Object trong Editor
//                         if (GUILayout.Button("Select", GUILayout.Width(55)))
//                         {
//                             Selection.activeGameObject = marker.gameObject;
//                             EditorGUIUtility.PingObject(marker.gameObject);
//                         }

//                         // Nhãn hiển thị Order, Label và Tên GameObject
//                         string label = $"[#{marker.OrderInStep}] {marker.ElementLabel}  ({marker.gameObject.name})";
//                         EditorGUILayout.LabelField(label, EditorStyles.label);

//                         // Hiển thị badge UI Offset nếu được cấu hình
//                         if (marker.HasUIOffset)
//                         {
//                             GUILayout.Label($"[UI: {marker.UIPlacement}]", EditorStyles.miniBoldLabel, GUILayout.Width(75));
//                         }

//                         EditorGUILayout.EndHorizontal();
//                     }
//                     EditorGUI.indentLevel--;
//                 }

//                 EditorGUILayout.EndVertical();
//                 EditorGUILayout.Space(2);
//             }

//             EditorGUILayout.EndScrollView();
//         }
//     }
// }

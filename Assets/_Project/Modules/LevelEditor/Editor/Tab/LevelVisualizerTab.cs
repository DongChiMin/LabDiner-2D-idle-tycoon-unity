using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LabDiner.LevelEditor
{
    public static class LevelVisualizerTab
    {
        private static Vector2 _scrollPos;
        private static readonly Dictionary<LevelDesignStep, List<LevelElementMarker>> _groupedMarkers = new();
        private static readonly Dictionary<LevelDesignStep, bool> _stepFoldouts = new();
        private static bool _isInPrefabMode = false;

        public static void Initialize()
        {
            foreach (LevelDesignStep step in System.Enum.GetValues(typeof(LevelDesignStep)))
            {
                if (!_stepFoldouts.ContainsKey(step))
                {
                    _stepFoldouts[step] = true;
                }
            }
            ScanMarkers();
        }

        public static void ScanMarkers()
        {
            _groupedMarkers.Clear();

            List<LevelElementMarker> allMarkers = new List<LevelElementMarker>();
            PrefabStage currentStage = PrefabStageUtility.GetCurrentPrefabStage();
            _isInPrefabMode = currentStage != null;

            if (_isInPrefabMode && currentStage.prefabContentsRoot != null)
            {
                allMarkers = currentStage.prefabContentsRoot
                    .GetComponentsInChildren<LevelElementMarker>(true).ToList();
            }
            else
            {
                allMarkers = Object.FindObjectsByType<LevelElementMarker>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
            }

            foreach (LevelDesignStep step in System.Enum.GetValues(typeof(LevelDesignStep)))
            {
                var listInStep = allMarkers
                    .Where(m => m.Step == step)
                    .OrderBy(m => m.OrderInStep)
                    .ToList();

                _groupedMarkers[step] = listInStep;
            }
        }

        public static void DrawGUI()
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("Level Design Steps Inspector", EditorStyles.boldLabel);

            string modeText = _isInPrefabMode ? "PREFAB MODE" : "SCENE MODE";
            EditorGUILayout.HelpBox($"[{modeText}] Quét được tổng cộng {_groupedMarkers.Values.Sum(list => list.Count)} elements.", MessageType.Info);

            EditorGUILayout.Space(5);

            if (GUILayout.Button("Refresh / Scan", GUILayout.Height(25)))
            {
                ScanMarkers();
            }

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _scrollPos = EditorGUILayout.BeginScrollView(_scrollPos);

            foreach (LevelDesignStep step in System.Enum.GetValues(typeof(LevelDesignStep)))
            {
                List<LevelElementMarker> markers = _groupedMarkers.ContainsKey(step) ? _groupedMarkers[step] : new List<LevelElementMarker>();

                EditorGUILayout.BeginVertical(EditorStyles.helpBox);

                if (!_stepFoldouts.ContainsKey(step)) _stepFoldouts[step] = true;

                _stepFoldouts[step] = EditorGUILayout.Foldout(
                    _stepFoldouts[step], 
                    $"{step}  [{markers.Count} items]", 
                    true, 
                    EditorStyles.foldoutHeader
                );

                if (_stepFoldouts[step])
                {
                    EditorGUI.indentLevel++;
                    if (markers.Count == 0)
                    {
                        EditorGUILayout.LabelField("Chưa có element nào...", EditorStyles.miniLabel);
                    }

                    foreach (var marker in markers)
                    {
                        if (marker == null) continue;

                        EditorGUILayout.BeginHorizontal();

                        if (GUILayout.Button("Select", GUILayout.Width(55)))
                        {
                            Selection.activeGameObject = marker.gameObject;
                            EditorGUIUtility.PingObject(marker.gameObject);
                        }

                        string label = $"[#{marker.OrderInStep}] {marker.ElementLabel}  ({marker.gameObject.name})";
                        EditorGUILayout.LabelField(label, EditorStyles.label);

                        if (marker.HasUIOffset)
                        {
                            GUILayout.Label($"[UI: {marker.UIPlacement}]", EditorStyles.miniBoldLabel, GUILayout.Width(75));
                        }

                        EditorGUILayout.EndHorizontal();
                    }
                    EditorGUI.indentLevel--;
                }

                EditorGUILayout.EndVertical();
                EditorGUILayout.Space(2);
            }

            EditorGUILayout.EndScrollView();
        }
    }
}
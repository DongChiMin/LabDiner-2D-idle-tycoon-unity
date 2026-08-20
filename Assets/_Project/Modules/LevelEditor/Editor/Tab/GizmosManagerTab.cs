#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LabDiner.LevelEditor
{
    public static class GizmosManagerTab
    {
        private static Vector2 _scrollPosition;
        private static readonly List<IGizmosDrawable> _foundGizmos = new List<IGizmosDrawable>();
        private static bool _isInPrefabMode = false;

        // Gọi hàm này khi Tab được kích hoạt hoặc cần quét lại
        public static void ScanScene()
        {
            _foundGizmos.Clear();

            PrefabStage currentPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            _isInPrefabMode = currentPrefabStage != null;

            if (_isInPrefabMode)
            {
                GameObject prefabRoot = currentPrefabStage.prefabContentsRoot;
                if (prefabRoot != null)
                {
                    MonoBehaviour[] components = prefabRoot.GetComponentsInChildren<MonoBehaviour>(true);
                    foreach (var comp in components)
                    {
                        if (comp is IGizmosDrawable drawable)
                        {
                            _foundGizmos.Add(drawable);
                        }
                    }
                }
            }
            else
            {
                MonoBehaviour[] allComponents = Object.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var comp in allComponents)
                {
                    if (comp is IGizmosDrawable drawable)
                    {
                        _foundGizmos.Add(drawable);
                    }
                }
            }
        }

        // Hàm vẽ giao diện chính của Tab A (được gọi từ cửa sổ tổng)
        public static void DrawGUI()
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("IGizmoDrawable Controller", EditorStyles.boldLabel);

            string modeText = _isInPrefabMode ? "Mode: PREFAB STAGE" : "Mode: SCENE";
            EditorGUILayout.HelpBox($"[{modeText}] Tìm thấy {_foundGizmos.Count} script(s) dùng IGizmoDrawable.", MessageType.Info);

            EditorGUILayout.Space(5);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Refresh / Scan", GUILayout.Height(25)))
            {
                ScanScene();
            }
            if (GUILayout.Button("Show All", GUILayout.Height(25)))
            {
                SetAllGizmosState(true);
            }
            if (GUILayout.Button("Hide All", GUILayout.Height(25)))
            {
                SetAllGizmosState(false);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (int i = 0; i < _foundGizmos.Count; i++)
            {
                var drawable = _foundGizmos[i];
                if (drawable is not MonoBehaviour comp || comp == null) continue;

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                if (GUILayout.Button("Focus", GUILayout.Width(55)))
                {
                    Selection.activeGameObject = comp.gameObject;
                    EditorGUIUtility.PingObject(comp.gameObject);
                }

                string labelText = $"{comp.gameObject.name} ({comp.GetType().Name})";
                EditorGUILayout.LabelField(labelText, EditorStyles.label);

                EditorGUI.BeginChangeCheck();
                bool newState = EditorGUILayout.Toggle(drawable.ShowGizmos, GUILayout.Width(20));
                if (EditorGUI.EndChangeCheck())
                {
                    drawable.ShowGizmos = newState;
                    EditorUtility.SetDirty(comp);
                    
                    if (_isInPrefabMode)
                    {
                        EditorSceneManager.MarkSceneDirty(comp.gameObject.scene);
                    }
                    
                    SceneView.RepaintAll();
                }

                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndScrollView();
        }

        private static void SetAllGizmosState(bool state)
        {
            foreach (var drawable in _foundGizmos)
            {
                if (drawable is MonoBehaviour comp && comp != null)
                {
                    drawable.ShowGizmos = state;
                    EditorUtility.SetDirty(comp);
                    
                    if (_isInPrefabMode)
                    {
                        EditorSceneManager.MarkSceneDirty(comp.gameObject.scene);
                    }
                }
            }
            SceneView.RepaintAll();
        }
    }
}
#endif
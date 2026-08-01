using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LabDiner.LevelEditor
{
public class GizmosManagerWindow : EditorWindow
    {
        private Vector2 _scrollPosition;
        private readonly List<IGizmosDrawable> _foundGizmos = new List<IGizmosDrawable>();
        private bool _isInPrefabMode = false;

        [MenuItem("Tools/LabDiner/Gizmos Manager")]
        public static void OpenWindow()
        {
            GizmosManagerWindow window = GetWindow<GizmosManagerWindow>("Gizmos Manager");
            window.minSize = new Vector2(350, 400);
            window.ScanScene();
        }

        private void OnFocus()
        {
            ScanScene();
        }

        private void OnEnable()
        {
            // Lắng nghe sự kiện chuyển đổi giữa Scene và Prefab Mode để tự động Refresh
            PrefabStage.prefabStageOpened += OnPrefabStageChanged;
            PrefabStage.prefabStageClosing += OnPrefabStageChanged;
        }

        private void OnDisable()
        {
            PrefabStage.prefabStageOpened -= OnPrefabStageChanged;
            PrefabStage.prefabStageClosing -= OnPrefabStageChanged;
        }

        private void OnPrefabStageChanged(PrefabStage stage)
        {
            ScanScene();
            Repaint();
        }

        /// <summary>
        /// Quét thông minh: Tự phân biệt Scene Mode hay Prefab Mode
        /// </summary>
        private void ScanScene()
        {
            _foundGizmos.Clear();

            // 1. Kiểm tra xem người dùng có đang mở Prefab Mode hay không
            PrefabStage currentPrefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            _isInPrefabMode = currentPrefabStage != null;

            if (_isInPrefabMode)
            {
                // Nếu đang ở Prefab Mode, lấy Root GameObject của Prefab đang mở
                GameObject prefabRoot = currentPrefabStage.prefabContentsRoot;
                if (prefabRoot != null)
                {
                    // Tìm tất cả component IGizmoDrawable từ root trở xuống (bao gồm cả Inactive)
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
                // 2. Nếu ở Scene thường, quét bình thường
                MonoBehaviour[] allComponents = FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None);
                foreach (var comp in allComponents)
                {
                    if (comp is IGizmosDrawable drawable)
                    {
                        _foundGizmos.Add(drawable);
                    }
                }
            }
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField("IGizmoDrawable Controller", EditorStyles.boldLabel);

            // Báo trạng thái môi trường hiện tại
            string modeText = _isInPrefabMode ? "Mode: PREFAB STAGE" : "Mode: SCENE";
            EditorGUILayout.HelpBox($"[{modeText}] Tìm thấy {_foundGizmos.Count} script(s) dùng IGizmoDrawable.", MessageType.Info);

            EditorGUILayout.Space(5);

            // BỘ NÚT ĐIỀU KHIỂN HÀNG LOẠT
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

            // DANH SÁCH CÁC SCRIPT
            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (int i = 0; i < _foundGizmos.Count; i++)
            {
                var drawable = _foundGizmos[i];
                if (drawable is not MonoBehaviour comp || comp == null) continue;

                EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);

                // Nút Focus: Tìm và chọn GameObject
                if (GUILayout.Button("Focus", GUILayout.Width(55)))
                {
                    Selection.activeGameObject = comp.gameObject;
                    EditorGUIUtility.PingObject(comp.gameObject);
                }

                // Hiển thị Tên GameObject + Tên Class Script
                string labelText = $"{comp.gameObject.name} ({comp.GetType().Name})";
                EditorGUILayout.LabelField(labelText, EditorStyles.label);

                // Công tắc Toggle Show/Hide cho từng cái
                EditorGUI.BeginChangeCheck();
                bool newState = EditorGUILayout.Toggle(drawable.ShowGizmos, GUILayout.Width(20));
                if (EditorGUI.EndChangeCheck())
                {
                    drawable.ShowGizmos = newState;
                    EditorUtility.SetDirty(comp);
                    
                    // Nếu đang ở Prefab Mode, đánh dấu Prefab đã thay đổi để Unity cho phép Save
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

        private void SetAllGizmosState(bool state)
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

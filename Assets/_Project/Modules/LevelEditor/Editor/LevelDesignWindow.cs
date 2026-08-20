#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace LabDiner.LevelEditor
{
    public class LevelEditorHubWindow : EditorWindow
    {
        enum HubTab
        {
            GizmosManager,
            LevelManager
        }

        private HubTab _currentTab;

        [MenuItem("Tools/LabDiner/Level Editor Hub")]
        public static void OpenWindow()
        {
            LevelEditorHubWindow window = GetWindow<LevelEditorHubWindow>("Level Editor Hub");
            window.minSize = new Vector2(400, 500);
            
            // Khởi tạo dữ liệu ban đầu cho cả 2 tab
            GizmosManagerTab.ScanScene();
            LevelVisualizerTab.Initialize();
            
            window.Show();
        }

        private void OnEnable()
        {
            PrefabStage.prefabStageOpened += OnStageChanged;
            PrefabStage.prefabStageClosing += OnStageChanged;
            
            LevelVisualizerTab.Initialize();
        }

        private void OnDisable()
        {
            PrefabStage.prefabStageOpened -= OnStageChanged;
            PrefabStage.prefabStageClosing -= OnStageChanged;
        }

        private void OnStageChanged(PrefabStage stage)
        {
            GizmosManagerTab.ScanScene();
            LevelVisualizerTab.ScanMarkers();
            Repaint();
        }

        private void OnFocus()
        {
            // Tự động quét lại dữ liệu khi người dùng focus vào cửa sổ
            GizmosManagerTab.ScanScene();
            LevelVisualizerTab.ScanMarkers();
        }

        private void OnGUI()
        {
            EditorGUILayout.Space(5);

            // Tạo thanh Toolbar chuyển tab ngang
            string[] tabNames = { "Gizmos Manager", "Level Element Manager" };
            _currentTab = (HubTab)GUILayout.Toolbar((int)_currentTab, tabNames, GUILayout.Height(30));

            EditorGUILayout.Space(10);

            // Vẽ nội dung tương ứng theo Tab đang chọn
            switch (_currentTab)
            {
                case HubTab.GizmosManager:
                    GizmosManagerTab.DrawGUI();
                    break;
                case HubTab.LevelManager:
                    LevelVisualizerTab.DrawGUI();
                    break;
            }
        }
    }
}
#endif 
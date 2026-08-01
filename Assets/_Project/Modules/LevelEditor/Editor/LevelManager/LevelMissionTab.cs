#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using LabDiner.Restaurant.SO;

namespace LabDiner.LevelEditor
{
    public class LevelMissionTab
    {
        private LevelManagerWindow _parent;
        
        // Mảng chứa tên hiển thị và đường dẫn thực tế của các file mẫu
        private string[] _missionTypeNames = new string[0];
        private string[] _missionTemplatePaths = new string[0];
        private int _selectedMissionType = 0;

        public LevelMissionTab(LevelManagerWindow parent)
        {
            _parent = parent;
        }

        public void OnEnable()
        {
            RefreshTemplates();
        }

        /// <summary>
        /// Quét tự động thư mục mẫu để lấy danh sách các file Mission mẫu hiện có
        /// </summary>
        private void RefreshTemplates()
        {
            string templateFolder = $"{LevelCreatorTab.TemplateFolderPath}/Mission";
            
            if (!AssetDatabase.IsValidFolder(templateFolder))
            {
                _missionTypeNames = new string[0];
                _missionTemplatePaths = new string[0];
                return;
            }

            string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { templateFolder });
            List<string> names = new List<string>();
            List<string> paths = new List<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                names.Add(Path.GetFileNameWithoutExtension(path));
                paths.Add(path);
            }

            _missionTypeNames = names.ToArray();
            _missionTemplatePaths = paths.ToArray();

            if (_selectedMissionType >= _missionTypeNames.Length)
            {
                _selectedMissionType = Mathf.Max(0, _missionTypeNames.Length - 1);
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("🎯 QUẢN LÝ MISSION", EditorStyles.boldLabel);
            GUILayout.Space(5);

            if (_parent.LevelOptions.Length == 0)
            {
                EditorGUILayout.HelpBox("Chưa có Level nào trong Registry! Hãy sang Tab Creator để tạo Level trước.", MessageType.Warning);
                return;
            }

            // Chọn Level dạng Dropdown
            _parent.SelectedLevelDropdownIndex = EditorGUILayout.Popup("Chọn Level Cần Cấu Hình:", _parent.SelectedLevelDropdownIndex, _parent.LevelOptions);

            int currentLevelNumber = _parent.GetCurrentSelectedLevelNumber();
            string levelFolderPath = $"{LevelCreatorTab.RootDestinationPath}/level_{currentLevelNumber}";
            string missionFolderPath = $"{levelFolderPath}/Mission";

            if (!AssetDatabase.IsValidFolder(levelFolderPath))
            {
                EditorGUILayout.HelpBox($"Không tìm thấy thư mục của Level {currentLevelNumber}!", MessageType.Error);
                return;
            }

            GUILayout.Space(15);
            EditorGUILayout.LabelField("➕ Thêm Mission Mới:", EditorStyles.boldLabel);
            
            if (_missionTypeNames.Length == 0)
            {
                RefreshTemplates();
            }

            if (_missionTypeNames.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                _selectedMissionType = EditorGUILayout.Popup("Loại Mission Mẫu:", _selectedMissionType, _missionTypeNames);
                
                if (GUILayout.Button("Thêm", GUILayout.Width(80)))
                {
                    string selectedTemplatePath = _missionTemplatePaths[_selectedMissionType];
                    string selectedTypeName = _missionTypeNames[_selectedMissionType];
                    
                    AddNewMission(missionFolderPath, currentLevelNumber, selectedTemplatePath, selectedTypeName);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox($"Không tìm thấy file mẫu nào trong thư mục:\n{LevelCreatorTab.TemplateFolderPath}/Mission", MessageType.Warning);
                if (GUILayout.Button("🔄 Quét lại thư mục mẫu"))
                {
                    RefreshTemplates();
                }
            }

            GUILayout.Space(15);
            
            // Nút đồng bộ thủ công phòng trường hợp kéo/thả/xóa file trực tiếp bằng Project window
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("📂 Các Mission Hiện Có:", EditorStyles.boldLabel);
            if (GUILayout.Button("🔄 Đồng bộ lại SO", GUILayout.Width(130)))
            {
                SyncMissionsToLevelConfig(currentLevelNumber);
            }
            EditorGUILayout.EndHorizontal();

            DrawExistingMissions(missionFolderPath, currentLevelNumber);
        }

        private void AddNewMission(string targetFolder, int levelIndex, string sourceTemplatePath, string missionTypeName)
        {
            if (!File.Exists(sourceTemplatePath))
            {
                Debug.LogError($"[Mission] Không tìm thấy file mẫu tại đường dẫn: {sourceTemplatePath}");
                return;
            }

            if (!AssetDatabase.IsValidFolder(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
                AssetDatabase.Refresh();
            }

            string[] existingFiles = Directory.GetFiles(targetFolder, "*.asset");
            int count = existingFiles.Length + 1;
            
            string typeSuffix = missionTypeName.Equals("Final", System.StringComparison.OrdinalIgnoreCase) ? "final" : count.ToString();
            string newFileName = $"{levelIndex}_{typeSuffix}.asset";
            string destPath = $"{targetFolder}/{newFileName}";

            AssetDatabase.CopyAsset(sourceTemplatePath, destPath);
            AssetDatabase.Refresh();
            
            Object newAsset = AssetDatabase.LoadAssetAtPath<Object>(destPath);
            EditorGUIUtility.PingObject(newAsset);
            
            Debug.Log($"<color=lime>[Mission]</color> Đã tạo thành công: <b>{newFileName}</b>");

            // Tự động đồng bộ lại vào LevelConfigSO
            SyncMissionsToLevelConfig(levelIndex);
        }

        private void DrawExistingMissions(string folderPath, int levelIndex)
        {
            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
            EditorGUILayout.BeginVertical("box");
            GUI.backgroundColor = Color.white;

            if (AssetDatabase.IsValidFolder(folderPath))
            {
                string[] guids = AssetDatabase.FindAssets("t:BaseMissionSO", new[] { folderPath });
                if (guids.Length == 0)
                {
                    EditorGUILayout.LabelField("  (Thư mục trống)", EditorStyles.miniLabel);
                }
                
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    Object asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                    
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(asset, typeof(ScriptableObject), false);
                    
                    GUI.backgroundColor = new Color(1f, 0.4f, 0.4f);
                    if (GUILayout.Button("X", GUILayout.Width(25)))
                    {
                        if (EditorUtility.DisplayDialog("Xác nhận xóa", $"Bạn có muốn xóa file {asset.name} không?", "Xóa", "Hủy"))
                        {
                            AssetDatabase.DeleteAsset(path);
                            // Tự động đồng bộ lại sau khi xóa
                            SyncMissionsToLevelConfig(levelIndex);
                        }
                    }
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("  (Chưa tạo thư mục Mission)", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// Đồng bộ danh sách các file Mission hiện có trong folder /Mission vào LevelConfigSO
        /// </summary>
        private void SyncMissionsToLevelConfig(int levelIndex)
        {
            string levelFolderPath = $"{LevelCreatorTab.RootDestinationPath}/level_{levelIndex}";
            string missionFolderPath = $"{levelFolderPath}/Mission";

            // 1. Tìm file LevelConfigSO (ví dụ: Level 1.asset)
            string expectedLevelPath = $"{levelFolderPath}/Level {levelIndex}.asset";
            LevelConfigSO levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfigSO>(expectedLevelPath);

            // Nếu không tìm thấy bằng đường dẫn chính xác, tìm bất kỳ LevelConfigSO nào trong folder level_X
            if (levelConfig == null && AssetDatabase.IsValidFolder(levelFolderPath))
            {
                string[] levelGuids = AssetDatabase.FindAssets("t:LevelConfigSO", new[] { levelFolderPath });
                if (levelGuids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(levelGuids[0]);
                    levelConfig = AssetDatabase.LoadAssetAtPath<LevelConfigSO>(path);
                }
            }

            if (levelConfig == null)
            {
                Debug.LogWarning($"[Mission Sync] Không tìm thấy file LevelConfigSO cho Level {levelIndex} tại {levelFolderPath}");
                return;
            }

            // 2. Quét tất cả BaseMissionSO trong folder Mission
            List<BaseMissionSO> missionList = new List<BaseMissionSO>();
            if (AssetDatabase.IsValidFolder(missionFolderPath))
            {
                string[] guids = AssetDatabase.FindAssets("t:BaseMissionSO", new[] { missionFolderPath });
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    BaseMissionSO missionAsset = AssetDatabase.LoadAssetAtPath<BaseMissionSO>(path);
                    if (missionAsset != null)
                    {
                        missionList.Add(missionAsset);
                    }
                }
            }

            // Sắp xếp danh sách theo tên file để thứ tự mảng ổn định
            missionList = missionList.OrderBy(m => m.name).ToList();

            // 3. Ghi nhận Undo và cập nhật danh sách AvailableMissions
            Undo.RecordObject(levelConfig, "Sync Missions to Level Config");
            levelConfig.AvailableMissions = missionList;

            // 4. Lưu thay đổi
            EditorUtility.SetDirty(levelConfig);
            AssetDatabase.SaveAssets();

            Debug.Log($"<color=cyan>[Mission Sync]</color> Đã đồng bộ <b>{missionList.Count}</b> mission(s) vào <b>{levelConfig.name}</b>");
        }
    }
}
#endif
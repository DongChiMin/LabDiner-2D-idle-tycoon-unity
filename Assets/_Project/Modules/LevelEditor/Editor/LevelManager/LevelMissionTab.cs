#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;

namespace LabDiner.LevelEditor
{
    public class LevelMissionTab
    {
        private LevelManagerWindow _parent;
        
        // Template scanning
        private string[] _missionTypeNames = new string[0];
        private string[] _missionTemplatePaths = new string[0];
        private int _selectedMissionType = 0;

        // Reorderable UI & Sync state
        private ReorderableList _reorderableList;
        private LevelConfigSO _currentLevelConfig;
        private int _lastLevelIndex = -1;

        public LevelMissionTab(LevelManagerWindow parent)
        {
            _parent = parent;
        }

        public void OnEnable()
        {
            RefreshTemplates();
            _lastLevelIndex = -1; // Reset cache
        }

        /// <summary>
        /// Quét tự động danh sách các file template mẫu
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

            // Load LevelConfigSO và khởi tạo ReorderableList khi chuyển Level
            if (_lastLevelIndex != currentLevelNumber || _currentLevelConfig == null)
            {
                _lastLevelIndex = currentLevelNumber;
                _currentLevelConfig = GetLevelConfig(currentLevelNumber, levelFolderPath);
                InitReorderableList(missionFolderPath);
            }

            if (_currentLevelConfig == null)
            {
                EditorGUILayout.HelpBox($"Không tìm thấy file LevelConfigSO cho Level {currentLevelNumber}!", MessageType.Error);
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
                    
                    AddNewMission(missionFolderPath, selectedTemplatePath, selectedTypeName);
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

            // Header & Nút đồng bộ bổ sung
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("📂 Danh Sách Mission:", EditorStyles.boldLabel);
            if (GUILayout.Button("🔄 Cập nhật LevelConfigSO", GUILayout.Width(170)))
            {
                SyncFolderToConfig(missionFolderPath);
            }
            EditorGUILayout.EndHorizontal();

            // Làm sạch các reference null trước khi vẽ
            CleanNullReferences();

            // Hiển thị danh sách kéo thả
            if (_reorderableList != null)
            {
                _reorderableList.DoLayoutList();
            }
        }

        /// <summary>
        /// Khởi tạo ReorderableList để kéo thả trực tiếp list AvailableMissions
        /// </summary>
        private void InitReorderableList(string missionFolderPath)
        {
            if (_currentLevelConfig == null) return;

            if (_currentLevelConfig.AvailableMissions == null)
            {
                _currentLevelConfig.AvailableMissions = new List<BaseMissionSO>();
            }

            // Quét tự động để đưa các file có sẵn trong folder vào config nếu chưa có
            SyncFolderToConfig(missionFolderPath, silent: true);

            _reorderableList = new ReorderableList(_currentLevelConfig.AvailableMissions, typeof(BaseMissionSO), true, true, false, true);
            _reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Danh Sách Mission");
            };

            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index >= _currentLevelConfig.AvailableMissions.Count) return;

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                var mission = _currentLevelConfig.AvailableMissions[index];

                // Field hiển thị Object
                Rect objectRect = new Rect(rect.x, rect.y, rect.width, rect.height);
                EditorGUI.ObjectField(objectRect, mission, typeof(BaseMissionSO), false);
            };

            // Sự kiện khi người dùng kéo thả thay đổi thứ tự
            _reorderableList.onReorderCallback = (ReorderableList list) =>
            {
                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();
        };

            // Sự kiện khi nhấn nút xóa (-) trên danh sách
            _reorderableList.onRemoveCallback = (ReorderableList list) =>
            {
                int index = list.index;
                if (index < 0 || index >= _currentLevelConfig.AvailableMissions.Count) return;

                BaseMissionSO mission = _currentLevelConfig.AvailableMissions[index];
                if (mission != null)
                {
                    string path = AssetDatabase.GetAssetPath(mission);
                    
                    int option = EditorUtility.DisplayDialogComplex(
                        "Xác nhận xóa Mission",
                        $"Bạn muốn làm gì với file '{mission.name}'?",
                        "Xóa vĩnh viễn File trên đĩa", // 0
                        "Hủy",                           // 1
                        "Chỉ xóa khỏi Danh Sách"         // 2
                    );

                    if (option == 0) // Xóa hẳn file
                    {
                        Undo.RecordObject(_currentLevelConfig, "Remove Mission");
                        _currentLevelConfig.AvailableMissions.RemoveAt(index);
                        AssetDatabase.DeleteAsset(path);
                    }
                    else if (option == 2) // Chỉ remove khỏi SO
                    {
                        Undo.RecordObject(_currentLevelConfig, "Remove Mission From List");
                        _currentLevelConfig.AvailableMissions.RemoveAt(index);
                    }
                    else
                    {
                        return; // Hủy
                    }
                }
                else
                {
                    _currentLevelConfig.AvailableMissions.RemoveAt(index);
                }

                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();
            };
        }

        /// <summary>
        /// Tạo file Mission mới, tự chống trùng tên (GenerateUniqueAssetPath) và thêm ngay vào SO
        /// </summary>
        private void AddNewMission(string targetFolder, string sourceTemplatePath, string missionTypeName)
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

            // Tự động sinh đường dẫn duy nhất (Ví dụ: Station.asset -> Station 1.asset -> Station 2.asset)
            string defaultPath = $"{targetFolder}/{missionTypeName}.asset";
            string uniqueDestPath = AssetDatabase.GenerateUniqueAssetPath(defaultPath);

            AssetDatabase.CopyAsset(sourceTemplatePath, uniqueDestPath);
            AssetDatabase.Refresh();

            BaseMissionSO newMission = AssetDatabase.LoadAssetAtPath<BaseMissionSO>(uniqueDestPath);

            if (newMission != null && _currentLevelConfig != null)
            {
                Undo.RecordObject(_currentLevelConfig, "Add New Mission");
                if (_currentLevelConfig.AvailableMissions == null)
                {
                    _currentLevelConfig.AvailableMissions = new List<BaseMissionSO>();
                }

                _currentLevelConfig.AvailableMissions.Add(newMission);
                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();

                EditorGUIUtility.PingObject(newMission);
                Debug.Log($"<color=lime>[Mission]</color> Đã tạo thành công: <b>{Path.GetFileName(uniqueDestPath)}</b> và thêm vào LevelConfigSO!");
            }
        }

        private LevelConfigSO GetLevelConfig(int levelIndex, string levelFolderPath)
        {
            string expectedLevelPath = $"{levelFolderPath}/Level {levelIndex}.asset";
            LevelConfigSO config = AssetDatabase.LoadAssetAtPath<LevelConfigSO>(expectedLevelPath);

            if (config == null && AssetDatabase.IsValidFolder(levelFolderPath))
            {
                string[] levelGuids = AssetDatabase.FindAssets("t:LevelConfigSO", new[] { levelFolderPath });
                if (levelGuids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(levelGuids[0]);
                    config = AssetDatabase.LoadAssetAtPath<LevelConfigSO>(path);
                }
            }

            return config;
        }

        private void CleanNullReferences()
        {
            if (_currentLevelConfig == null || _currentLevelConfig.AvailableMissions == null) return;

            bool isDirty = false;
            for (int i = _currentLevelConfig.AvailableMissions.Count - 1; i >= 0; i--)
            {
                if (_currentLevelConfig.AvailableMissions[i] == null)
                {
                    _currentLevelConfig.AvailableMissions.RemoveAt(i);
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                EditorUtility.SetDirty(_currentLevelConfig);
            }
        }

        private void SyncFolderToConfig(string missionFolderPath, bool silent = false)
        {
            if (_currentLevelConfig == null) return;

            if (_currentLevelConfig.AvailableMissions == null)
            {
                _currentLevelConfig.AvailableMissions = new List<BaseMissionSO>();
            }

            CleanNullReferences();

            int addedCount = 0;
            if (AssetDatabase.IsValidFolder(missionFolderPath))
            {
                string[] guids = AssetDatabase.FindAssets("t:BaseMissionSO", new[] { missionFolderPath });
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    BaseMissionSO missionAsset = AssetDatabase.LoadAssetAtPath<BaseMissionSO>(path);

                    if (missionAsset != null && !_currentLevelConfig.AvailableMissions.Contains(missionAsset))
                    {
                        _currentLevelConfig.AvailableMissions.Add(missionAsset);
                        addedCount++;
                    }
                }
            }

            if (addedCount > 0)
            {
                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();
                if (!silent)
                {
                    Debug.Log($"<color=cyan>[Mission Sync]</color> Đã bổ sung <b>{addedCount}</b> mission mới tìm thấy từ folder vào <b>{_currentLevelConfig.name}</b>");
                }
            }
            else if (!silent)
            {
                Debug.Log($"<color=cyan>[Mission Sync]</color> Danh sách trong <b>{_currentLevelConfig.name}</b> đã đầy đủ.");
            }
        }
    }
}
#endif
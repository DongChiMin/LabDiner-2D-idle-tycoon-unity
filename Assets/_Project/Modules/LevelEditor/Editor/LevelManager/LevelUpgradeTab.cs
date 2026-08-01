#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;

namespace LabDiner.LevelEditor
{
    public class LevelUpgradeTab
    {
        private LevelManagerWindow _parent;

        // Template scanning
        private string[] _upgradeTypeNames = new string[0];
        private string[] _upgradeTemplatePaths = new string[0];
        private int _selectedUpgradeType = 0;

        // Reorderable UI & Sync state
        private ReorderableList _reorderableList;
        private LevelConfigSO _currentLevelConfig;
        private int _lastLevelIndex = -1;

        public LevelUpgradeTab(LevelManagerWindow parent)
        {
            _parent = parent;
        }

        public void OnEnable()
        {
            RefreshTemplates();
            _lastLevelIndex = -1; // Reset cache
        }

        /// <summary>
        /// Quét tự động danh sách các file Upgrade mẫu trong folder _Example/Upgrade
        /// </summary>
        private void RefreshTemplates()
        {
            string templateFolder = $"{LevelCreatorTab.TemplateFolderPath}/Upgrade";

            if (!AssetDatabase.IsValidFolder(templateFolder))
            {
                _upgradeTypeNames = new string[0];
                _upgradeTemplatePaths = new string[0];
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

            _upgradeTypeNames = names.ToArray();
            _upgradeTemplatePaths = paths.ToArray();

            if (_selectedUpgradeType >= _upgradeTypeNames.Length)
            {
                _selectedUpgradeType = Mathf.Max(0, _upgradeTypeNames.Length - 1);
            }
        }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("⬆️ QUẢN LÝ UPGRADE", EditorStyles.boldLabel);
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
            string upgradeFolderPath = $"{levelFolderPath}/Upgrade";

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
                InitReorderableList(upgradeFolderPath);
            }

            if (_currentLevelConfig == null)
            {
                EditorGUILayout.HelpBox($"Không tìm thấy file LevelConfigSO cho Level {currentLevelNumber}!", MessageType.Error);
                return;
            }

            GUILayout.Space(15);
            EditorGUILayout.LabelField("➕ Thêm Upgrade Mới:", EditorStyles.boldLabel);

            if (_upgradeTypeNames.Length == 0)
            {
                RefreshTemplates();
            }

            if (_upgradeTypeNames.Length > 0)
            {
                EditorGUILayout.BeginHorizontal();
                _selectedUpgradeType = EditorGUILayout.Popup("Loại Upgrade Mẫu:", _selectedUpgradeType, _upgradeTypeNames);

                if (GUILayout.Button("Thêm", GUILayout.Width(80)))
                {
                    string selectedTemplatePath = _upgradeTemplatePaths[_selectedUpgradeType];
                    string selectedTypeName = _upgradeTypeNames[_selectedUpgradeType];

                    AddNewUpgrade(upgradeFolderPath, selectedTemplatePath, selectedTypeName);
                }
                EditorGUILayout.EndHorizontal();
            }
            else
            {
                EditorGUILayout.HelpBox($"Không tìm thấy file mẫu nào trong thư mục:\n{LevelCreatorTab.TemplateFolderPath}/Upgrade", MessageType.Warning);
                if (GUILayout.Button("🔄 Quét lại thư mục mẫu"))
                {
                    RefreshTemplates();
                }
            }

            GUILayout.Space(15);

            // Header & Nút đồng bộ bổ sung
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("📂 Danh Sách Upgrade:", EditorStyles.boldLabel);
            if (GUILayout.Button("🔄 Cập nhật LevelConfigSO", GUILayout.Width(170)))
            {
                SyncFolderToConfig(upgradeFolderPath);
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
        /// Khởi tạo ReorderableList để kéo thả trực tiếp list AvailableUpgrades (hoặc loại SO tương đương)
        /// </summary>
        private void InitReorderableList(string upgradeFolderPath)
        {
            if (_currentLevelConfig == null) return;

            if (_currentLevelConfig.AvailableUpgrades == null)
            {
                _currentLevelConfig.AvailableUpgrades = new List<BaseUpgradeSO>();
            }

            // Quét tự động để đưa các file có sẵn trong folder vào config nếu chưa có
            SyncFolderToConfig(upgradeFolderPath, silent: true);

            _reorderableList = new ReorderableList(_currentLevelConfig.AvailableUpgrades, typeof(BaseUpgradeSO), true, true, false, true);

            _reorderableList.drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, $"Upgrades trong {_currentLevelConfig.name}");
            };

            _reorderableList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                if (index >= _currentLevelConfig.AvailableUpgrades.Count) return;

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                var upgrade = _currentLevelConfig.AvailableUpgrades[index];

                // Field hiển thị Object
                Rect objectRect = new Rect(rect.x, rect.y, rect.width, rect.height);
                EditorGUI.ObjectField(objectRect, upgrade, typeof(BaseUpgradeSO), false);
            };

            // Sự kiện khi người dùng kéo thả thay đổi thứ tự
            _reorderableList.onReorderCallback = (ReorderableList list) =>
            {
                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();
                Debug.Log($"<color=cyan>[Upgrade]</color> Đã cập nhật lại thứ tự Upgrade trong <b>{_currentLevelConfig.name}</b>");
            };

            // Sự kiện khi nhấn nút xóa (-) trên danh sách
            _reorderableList.onRemoveCallback = (ReorderableList list) =>
            {
                int index = list.index;
                if (index < 0 || index >= _currentLevelConfig.AvailableUpgrades.Count) return;

                BaseUpgradeSO upgrade = _currentLevelConfig.AvailableUpgrades[index];
                if (upgrade != null)
                {
                    string path = AssetDatabase.GetAssetPath(upgrade);

                    int option = EditorUtility.DisplayDialogComplex(
                        "Xác nhận xóa Upgrade",
                        $"Bạn muốn làm gì với file '{upgrade.name}'?",
                        "Xóa vĩnh viễn File trên đĩa", // 0
                        "Hủy",                           // 1
                        "Chỉ xóa khỏi Danh Sách"         // 2
                    );

                    if (option == 0) // Xóa hẳn file
                    {
                        Undo.RecordObject(_currentLevelConfig, "Remove Upgrade");
                        _currentLevelConfig.AvailableUpgrades.RemoveAt(index);
                        AssetDatabase.DeleteAsset(path);
                    }
                    else if (option == 2) // Chỉ remove khỏi SO
                    {
                        Undo.RecordObject(_currentLevelConfig, "Remove Upgrade From List");
                        _currentLevelConfig.AvailableUpgrades.RemoveAt(index);
                    }
                    else
                    {
                        return; // Hủy
                    }
                }
                else
                {
                    _currentLevelConfig.AvailableUpgrades.RemoveAt(index);
                }

                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();
            };
        }

        /// <summary>
        /// Tạo file Upgrade mới, chống trùng tên (GenerateUniqueAssetPath) và thêm vào LevelConfigSO
        /// </summary>
        private void AddNewUpgrade(string targetFolder, string sourceTemplatePath, string upgradeTypeName)
        {
            if (!File.Exists(sourceTemplatePath))
            {
                Debug.LogError($"[Upgrade] Không tìm thấy file mẫu tại đường dẫn: {sourceTemplatePath}");
                return;
            }

            if (!AssetDatabase.IsValidFolder(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
                AssetDatabase.Refresh();
            }

            // Sinh đường dẫn duy nhất (Ví dụ: Guest.asset -> Guest 1.asset)
            string defaultPath = $"{targetFolder}/{upgradeTypeName}.asset";
            string uniqueDestPath = AssetDatabase.GenerateUniqueAssetPath(defaultPath);

            AssetDatabase.CopyAsset(sourceTemplatePath, uniqueDestPath);
            AssetDatabase.Refresh();

            BaseUpgradeSO newUpgrade = AssetDatabase.LoadAssetAtPath<BaseUpgradeSO>(uniqueDestPath);

            if (newUpgrade != null && _currentLevelConfig != null)
            {
                Undo.RecordObject(_currentLevelConfig, "Add New Upgrade");
                if (_currentLevelConfig.AvailableUpgrades == null)
                {
                    _currentLevelConfig.AvailableUpgrades = new List<BaseUpgradeSO>();
                }

                _currentLevelConfig.AvailableUpgrades.Add(newUpgrade);
                EditorUtility.SetDirty(_currentLevelConfig);
                AssetDatabase.SaveAssets();

                EditorGUIUtility.PingObject(newUpgrade);
                Debug.Log($"<color=cyan>[Upgrade]</color> Đã tạo thành công: <b>{Path.GetFileName(uniqueDestPath)}</b> và thêm vào LevelConfigSO!");
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
            if (_currentLevelConfig == null || _currentLevelConfig.AvailableUpgrades == null) return;

            bool isDirty = false;
            for (int i = _currentLevelConfig.AvailableUpgrades.Count - 1; i >= 0; i--)
            {
                if (_currentLevelConfig.AvailableUpgrades[i] == null)
                {
                    _currentLevelConfig.AvailableUpgrades.RemoveAt(i);
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                EditorUtility.SetDirty(_currentLevelConfig);
            }
        }

        private void SyncFolderToConfig(string upgradeFolderPath, bool silent = false)
        {
            if (_currentLevelConfig == null) return;

            if (_currentLevelConfig.AvailableUpgrades == null)
            {
                _currentLevelConfig.AvailableUpgrades = new List<BaseUpgradeSO>();
            }

            CleanNullReferences();

            int addedCount = 0;
            if (AssetDatabase.IsValidFolder(upgradeFolderPath))
            {
                string[] guids = AssetDatabase.FindAssets("t:BaseUpgradeSO", new[] { upgradeFolderPath });
                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);
                    BaseUpgradeSO upgradeAsset = AssetDatabase.LoadAssetAtPath<BaseUpgradeSO>(path);

                    if (upgradeAsset != null && !_currentLevelConfig.AvailableUpgrades.Contains(upgradeAsset))
                    {
                        _currentLevelConfig.AvailableUpgrades.Add(upgradeAsset);
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
                    Debug.Log($"<color=cyan>[Upgrade Sync]</color> Đã bổ sung <b>{addedCount}</b> upgrade mới tìm thấy từ folder vào <b>{_currentLevelConfig.name}</b>");
                }
            }
            else if (!silent)
            {
                Debug.Log($"<color=cyan>[Upgrade Sync]</color> Danh sách trong <b>{_currentLevelConfig.name}</b> đã đầy đủ.");
            }
        }
    }
}
#endif
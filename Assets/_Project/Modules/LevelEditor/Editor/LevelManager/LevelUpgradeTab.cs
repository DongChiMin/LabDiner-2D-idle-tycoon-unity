#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace LabDiner.LevelEditor
{
    public class LevelUpgradeTab
    {
        private LevelManagerWindow _parent;
        private string[] _upgradeTypes = { "Guest", "Staff", "Station" };
        private int _selectedUpgradeType = 0;

        public LevelUpgradeTab(LevelManagerWindow parent)
        {
            _parent = parent;
        }

        public void OnEnable() { }

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

            GUILayout.Space(15);
            EditorGUILayout.LabelField("➕ Thêm Upgrade Mới:", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            _selectedUpgradeType = EditorGUILayout.Popup("Loại Upgrade:", _selectedUpgradeType, _upgradeTypes);
            
            if (GUILayout.Button("Thêm", GUILayout.Width(80)))
            {
                AddNewUpgrade(upgradeFolderPath, currentLevelNumber, _upgradeTypes[_selectedUpgradeType]);
            }
            EditorGUILayout.EndHorizontal();

            GUILayout.Space(15);
            DrawExistingUpgrades(upgradeFolderPath);
        }

        private void AddNewUpgrade(string targetFolder, int levelIndex, string upgradeType)
        {
            string sourcePath = $"{LevelCreatorTab.TemplateFolderPath}/Upgrade/{upgradeType}.asset";
            
            if (!File.Exists(sourcePath))
            {
                Debug.LogError($"[Upgrade] Không tìm thấy file mẫu tại đường dẫn: {sourcePath}");
                return;
            }

            if (!AssetDatabase.IsValidFolder(targetFolder))
            {
                Directory.CreateDirectory(targetFolder);
                AssetDatabase.Refresh();
            }

            string[] existingFiles = Directory.GetFiles(targetFolder, "*.asset");
            int count = existingFiles.Length + 1;
            
            string newFileName = $"{levelIndex}_{count}.asset";
            string destPath = $"{targetFolder}/{newFileName}";

            AssetDatabase.CopyAsset(sourcePath, destPath);
            AssetDatabase.Refresh();
            
            Object newAsset = AssetDatabase.LoadAssetAtPath<Object>(destPath);
            EditorGUIUtility.PingObject(newAsset);
            
            Debug.Log($"<color=cyan>[Upgrade]</color> Đã tạo thành công: {newFileName}");
        }

        private void DrawExistingUpgrades(string folderPath)
        {
            EditorGUILayout.LabelField("📂 Các Upgrade Hiện Có:", EditorStyles.boldLabel);
            GUI.backgroundColor = new Color(0.9f, 0.9f, 0.9f);
            EditorGUILayout.BeginVertical("box");
            GUI.backgroundColor = Color.white;

            if (AssetDatabase.IsValidFolder(folderPath))
            {
                string[] guids = AssetDatabase.FindAssets("t:ScriptableObject", new[] { folderPath });
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
                        }
                    }
                    GUI.backgroundColor = Color.white;

                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.LabelField("  (Chưa tạo thư mục Upgrade)", EditorStyles.miniLabel);
            }

            EditorGUILayout.EndVertical();
        }
    }
}
#endif
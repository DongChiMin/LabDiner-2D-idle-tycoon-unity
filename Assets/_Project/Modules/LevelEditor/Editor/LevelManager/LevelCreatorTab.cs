#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;
using LabDiner.Restaurant.SO; 
using LabDiner.LevelSystem.Domain;
using LabDiner.Shared;

namespace LabDiner.LevelEditor
{
    public class LevelCreatorTab
    {
        private LevelManagerWindow _parent;
        private int _newLevelIndex = 1;

        public const string TemplateFolderPath = "Assets/_Project/Modules/Restaurant/Data/Levels/_Example";
        public const string RootDestinationPath = "Assets/_Project/Modules/Restaurant/Data/Levels";

        public LevelCreatorTab(LevelManagerWindow parent)
        {
            _parent = parent;
        }

        public void OnEnable() { }

        public void OnGUI()
        {
            EditorGUILayout.LabelField("🏭 KHỞI TẠO TIẾN TRÌNH LEVEL", EditorStyles.boldLabel);
            GUILayout.Space(10);

            _newLevelIndex = EditorGUILayout.IntField("Số Thứ Tự Level Mới:", _newLevelIndex);

            if (_newLevelIndex < 1)
            {
                EditorGUILayout.HelpBox("Level Index phải lớn hơn hoặc bằng 1!", MessageType.Error);
                GUI.enabled = false;
            }

            GUI.backgroundColor = new Color(0.2f, 0.6f, 1f);
            if (GUILayout.Button("✨ KHỞI TẠO LEVEL NEW", GUILayout.Height(35)))
            {
                ExecuteScaffolding();
                _parent.RefreshLevelList(); // Tự động làm mới Dropdown ở tất cả các Tab
            }
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;

            GUILayout.Space(15);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);

            EditorGUILayout.LabelField("🎮 KHU VỰC CHẠY THỬ (PLAYTEST)", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (_parent.LevelOptions.Length > 0)
            {
                // Sử dụng Dropdown chung
                _parent.SelectedLevelDropdownIndex = EditorGUILayout.Popup("Chọn Level Muốn Test:", _parent.SelectedLevelDropdownIndex, _parent.LevelOptions);

                GUILayout.Space(10);

                GUI.backgroundColor = new Color(0.9f, 0.3f, 0.3f);
                if (GUILayout.Button("🔥 XÓA DỮ LIỆU LƯU TRỮ (CLEAR SAVE)", GUILayout.Height(25)))
                {
                    if (EditorUtility.DisplayDialog("Xác nhận xóa", "Bạn có chắc chắn muốn xóa toàn bộ file save hiện tại không?", "Xóa ngay", "Hủy"))
                    {
                        ClearSaveData();
                    }
                }
                GUI.backgroundColor = Color.white;
                GUILayout.Space(5);

                GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f);
                if (GUILayout.Button("▶️ XÓA DỮ LIỆU & CHẠY LEVEL CHỈ ĐỊNH", GUILayout.Height(40)))
                {
                    if (EditorUtility.DisplayDialog("Xác nhận xóa", "Bạn có chắc chắn muốn xóa toàn bộ file save hiện tại không?", "Xóa ngay", "Hủy"))
                    {
                        ClearSaveData();
                        PlayTestLevel(_parent.GetCurrentSelectedLevelConfig());
                    }
                }
                GUI.backgroundColor = Color.white;
            }
            else
            {
                EditorGUILayout.HelpBox("Không tìm thấy dữ liệu level nào trong Registry để chạy thử. Hãy tạo level trước!", MessageType.Info);
                if (GUILayout.Button("🔄 Tải lại Registry"))
                {
                    _parent.RefreshLevelList();
                }
            }
        }

        private void PlayTestLevel(LevelConfigSO levelConfig)
        {
            if (levelConfig == null) return;

            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                Debug.LogWarning("[Playtest] Đang trong PlayMode, tool đã tự ngắt. Vui lòng nhấn lại lần nữa để áp dụng level mới.");
                return;
            }

            try 
            {
                PlayerSave progress = PlayerSaveFile.LoadFromFile();
                
                // Thay vì dùng levelConfig.ID, hãy thử truyền levelConfig.LevelIndex 
                // (hoặc nếu hàm StartNewLevel nhận int, hãy truyền trực tiếp LevelIndex vào)
                progress.StartNewLevel(levelConfig.ID); // Hoặc levelConfig.ID tùy thuộc vào kiểu dữ liệu hàm StartNewLevel yêu cầu
                
                PlayerSaveFile.SaveToFile(progress);
                Debug.Log($"<color=cyan>[Playtest]</color> Đã nạp thành công dữ liệu Level [<b>{levelConfig.ID}</b>] vào hệ thống lưu trữ!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Playtest] Lỗi khi lưu save: {e.Message}");
                return;
            }

            EditorApplication.isPlaying = true;
        }

        private void ExecuteScaffolding()
        {
            if (!AssetDatabase.IsValidFolder(TemplateFolderPath))
            {
                EditorUtility.DisplayDialog("Lỗi Hệ Thống", $"Không tìm thấy thư mục mẫu tại đường dẫn:\n{TemplateFolderPath}", "Kiểm tra lại");
                return;
            }

            string newFolderName = $"level_{_newLevelIndex}";
            string destinationPath = $"{RootDestinationPath}/{newFolderName}";

            if (AssetDatabase.IsValidFolder(destinationPath))
            {
                bool overwrite = EditorUtility.DisplayDialog("Cảnh báo", $"Thư mục [{newFolderName}] đã tồn tại! Bạn có chắc muốn ghi đè?", "Có, ghi đè", "Không");
                if (!overwrite) return;
                AssetDatabase.DeleteAsset(destinationPath);
            }

            if (!AssetDatabase.CopyAsset(TemplateFolderPath, destinationPath))
            {
                Debug.LogError($"[Scaffolder] Thất bại khi copy từ {TemplateFolderPath} sang {destinationPath}");
                return;
            }

            AssetDatabase.Refresh();

            string prefabPath = "";
            string configPath = "";

            string[] allAssetPaths = AssetDatabase.FindAssets("", new string[] { destinationPath });
            List<(string oldPath, string newName)> assetsToRename = new List<(string, string)>();

            foreach (string guid in allAssetPaths)
            {
                string currentPath = AssetDatabase.GUIDToAssetPath(guid);
                string extension = Path.GetExtension(currentPath);

                if (extension == ".prefab" && currentPath.Contains(destinationPath) && !currentPath.Contains("/Mission") && !currentPath.Contains("/Upgrade"))
                {
                    assetsToRename.Add((currentPath, $"Level {_newLevelIndex}"));
                    prefabPath = $"{Path.GetDirectoryName(currentPath)}/Level {_newLevelIndex}{extension}";
                }
                else if (extension == ".asset" && currentPath.Contains(destinationPath) && !currentPath.Contains("/Mission") && !currentPath.Contains("/Upgrade"))
                {
                    assetsToRename.Add((currentPath, $"Level {_newLevelIndex}"));
                    configPath = $"{Path.GetDirectoryName(currentPath)}/Level {_newLevelIndex}{extension}";
                }
            }

            foreach (var asset in assetsToRename)
            {
                AssetDatabase.RenameAsset(asset.oldPath, asset.newName);
            }
            AssetDatabase.Refresh();

            CreatePrefabVariant(prefabPath);
            
            LevelConfigSO configSO = AssetDatabase.LoadAssetAtPath<LevelConfigSO>(configPath);
            LinkPrefabToConfig(configSO, prefabPath);
            RegisterToLevelRegistry(configSO);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Object createdFolder = AssetDatabase.LoadAssetAtPath<Object>(destinationPath);
            EditorGUIUtility.PingObject(createdFolder);
            
            EditorUtility.DisplayDialog("Thành công!", $"Đã cấu hình xong cây thư mục và đăng ký Level {_newLevelIndex}!", "Tuyệt vời");
        }

        private void CreatePrefabVariant(string pathOfCopiedPrefab)
        {
            string[] sourcePrefabGUIDs = AssetDatabase.FindAssets("t:Prefab", new string[] { TemplateFolderPath });
            if (sourcePrefabGUIDs.Length == 0) return;

            string sourcePrefabPath = AssetDatabase.GUIDToAssetPath(sourcePrefabGUIDs[0]);
            GameObject sourcePrefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePrefabPath);

            if (sourcePrefabObj != null)
            {
                GameObject tempInstance = (GameObject)PrefabUtility.InstantiatePrefab(sourcePrefabObj);
                if (tempInstance != null)
                {
                    PrefabUtility.SaveAsPrefabAsset(tempInstance, pathOfCopiedPrefab);
                    Object.DestroyImmediate(tempInstance);
                }
            }
        }

        private void LinkPrefabToConfig(LevelConfigSO configSO, string prefabPath)
        {
            GameObject prefabObj = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (configSO != null)
            {
                SerializedObject serializedConfig = new SerializedObject(configSO);
                
                SerializedProperty prefabProperty = serializedConfig.FindProperty("LevelPrefab");
                if (prefabProperty != null && prefabObj != null)
                {
                    prefabProperty.objectReferenceValue = prefabObj;
                }

                SerializedProperty indexProperty = serializedConfig.FindProperty("LevelIndex");
                if (indexProperty != null)
                {
                    indexProperty.intValue = _newLevelIndex;
                }

                serializedConfig.ApplyModifiedProperties();
                EditorUtility.SetDirty(configSO);
            }
        }

        private void RegisterToLevelRegistry(LevelConfigSO newConfigSO)
        {
            if (newConfigSO == null) return;

            LevelRegistrySO registrySO = AssetDatabase.LoadAssetAtPath<LevelRegistrySO>(LevelManagerWindow.RegistryAssetPath);

            if (registrySO == null)
            {
                Debug.LogError($"[Scaffolder] Không tìm thấy file LevelRegistry tại đường dẫn: {LevelManagerWindow.RegistryAssetPath}");
                return;
            }

            SerializedObject serializedRegistry = new SerializedObject(registrySO);
            SerializedProperty registryProperty = serializedRegistry.FindProperty("registry");

            if (registryProperty != null && registryProperty.isArray)
            {
                int existingIndexInList = -1;

                for (int i = registryProperty.arraySize - 1; i >= 0; i--)
                {
                    SerializedProperty elementProp = registryProperty.GetArrayElementAtIndex(i);
                    LevelConfigSO elementAsset = elementProp.objectReferenceValue as LevelConfigSO;

                    if (elementAsset == null)
                    {
                        registryProperty.DeleteArrayElementAtIndex(i);
                        continue;
                    }

                    if (elementAsset.LevelIndex == _newLevelIndex)
                    {
                        existingIndexInList = i;
                    }
                }

                if (existingIndexInList != -1)
                {
                    SerializedProperty targetSlot = registryProperty.GetArrayElementAtIndex(existingIndexInList);
                    targetSlot.objectReferenceValue = newConfigSO;
                }
                else
                {
                    int newSlotIndex = registryProperty.arraySize;
                    registryProperty.InsertArrayElementAtIndex(newSlotIndex);
                    registryProperty.GetArrayElementAtIndex(newSlotIndex).objectReferenceValue = newConfigSO;
                }

                serializedRegistry.ApplyModifiedProperties();
                EditorUtility.SetDirty(registrySO);
            }
        }

        private void ClearSaveData()
        {
            try
            {
                PlayerSave emptyPlayerSave = new PlayerSave();
                PlayerSaveFile.SaveToFile(emptyPlayerSave);

                LevelProgressSave emptyLevelSave = new LevelProgressSave();
                LevelProgressSaveFile.SaveToFile(emptyLevelSave);
                
                Debug.Log("<color=green>[Save]</color> Đã xoá sạch dữ liệu save thành công!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Playtest] Lỗi khi xóa dữ liệu save: {e.Message}");
            }
        }
    }
}
#endif
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using LabDiner.Restaurant.SO; 
using LabDiner.LevelSystem.Domain;
using LabDiner.Shared;
// UI/Data namespace của bạn nếu cần (Ví dụ giả định nơi chứa ProgressRepository)
// using LabDiner.LevelSystem.Data; 

namespace LabDiner.Restaurant.Editor
{
    public class LevelCreatorTool : EditorWindow
    {
        private int _levelIndex = 1;
        
        // Khai báo thêm biến phục vụ tính năng Test Level
        private int _selectedTestIndex = 0;
        private string[] _levelOptions = new string[0];
        private List<int> _levelIndices = new List<int>();

        private const string TemplateFolderPath = "Assets/_Project/Modules/Restaurant/Data/Levels/_Example";
        private const string RootDestinationPath = "Assets/_Project/Modules/Restaurant/Data/Levels";
        private const string RegistryAssetPath = "Assets/_Project/Modules/Restaurant/Data/Levels/_Registry/LevelRegistry.asset";

        [MenuItem("LabDiner/🚀 Level Creator Tool")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelCreatorTool>("Level Creator Tool");
            window.minSize = new Vector2(380, 350); // Tăng chiều cao để đủ chỗ hiển thị
            window.maxSize = new Vector2(380, 350);
        }

        private void OnEnable()
        {
            // Cập nhật lại danh sách level mỗi khi mở hoặc focus vào tool
            UpdateLevelDropdown();
        }

        private void OnGUI()
        {
            GUILayout.Space(15);
            EditorGUILayout.LabelField("🏭 KHỞI TẠO TIẾN TRÌNH LEVEL", EditorStyles.boldLabel);
            GUILayout.Space(10);

            _levelIndex = EditorGUILayout.IntField("Số Thứ Tự Level Index:", _levelIndex);

            if (_levelIndex < 1)
            {
                EditorGUILayout.HelpBox("Level Index phải lớn hơn hoặc bằng 1!", MessageType.Error);
                GUI.enabled = false;
            }

            GUI.backgroundColor = new Color(0.2f, 0.6f, 1f);
            if (GUILayout.Button("✨ KHỞI TẠO LEVEL NEW", GUILayout.Height(35)))
            {
                ExecuteScaffolding();
                UpdateLevelDropdown(); // Cập nhật lại list sau khi tạo xong
            }
            GUI.backgroundColor = Color.white;
            GUI.enabled = true;

            // --- PHÂN TÁCH VÙNG CHỨC NĂNG VỚI ĐƯỜNG KẺ NGANG ---
            GUILayout.Space(15);
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Space(10);

            // --- KHU VỰC CHẠY THỬ LEVEL ---
            EditorGUILayout.LabelField("🎮 KHU VỰC CHẠY THỬ (PLAYTEST)", EditorStyles.boldLabel);
            GUILayout.Space(10);

            if (_levelOptions.Length > 0)
            {
                _selectedTestIndex = EditorGUILayout.Popup("Chọn Level Muốn Test:", _selectedTestIndex, _levelOptions);

                GUILayout.Space(10);

                // --- BẮT ĐẦU THÊM NÚT XÓA FILE SAVE TẠI ĐÂY ---
                GUI.backgroundColor = new Color(0.9f, 0.3f, 0.3f); // Màu đỏ cảnh báo
                if (GUILayout.Button("🔥 XÓA DỮ LIỆU LƯU TRỮ (CLEAR SAVE)", GUILayout.Height(25)))
                {
                    // Hiển thị hộp thoại xác nhận để tránh bấm nhầm
                    if (EditorUtility.DisplayDialog("Xác nhận xóa", "Bạn có chắc chắn muốn xóa toàn bộ file save hiện tại không?", "Xóa ngay", "Hủy"))
                    {
                        ClearSaveData();
                    }
                }
                GUI.backgroundColor = Color.white;
                GUILayout.Space(5);
                // --- KẾT THÚC THÊM NÚT ---

                GUI.backgroundColor = new Color(0.4f, 0.8f, 0.4f); // Màu xanh lá cây nổi bật cho nút Play
                if (GUILayout.Button("▶️ LƯU PROGRESS & CHẠY THỬ", GUILayout.Height(40)))
                {
                    int levelToTest = _levelIndices[_selectedTestIndex];
                    PlayTestLevel(levelToTest);
                }
                GUI.backgroundColor = Color.white;
            }
            else
            {
                EditorGUILayout.HelpBox("Không tìm thấy dữ liệu level nào trong Registry để chạy thử. Hãy tạo level trước!", MessageType.Info);
                if (GUILayout.Button("🔄 Tải lại Registry"))
                {
                    UpdateLevelDropdown();
                }
            }

            GUILayout.Space(15);
        }

        /// <summary>
        /// Quét file Registry tổng để cập nhật dữ liệu hiển thị lên Dropdown Popup
        /// </summary>
        private void UpdateLevelDropdown()
        {
            LevelRegistrySO registrySO = AssetDatabase.LoadAssetAtPath<LevelRegistrySO>(RegistryAssetPath);
            if (registrySO == null) return;

            // Sử dụng SerializedObject để đọc data đảm bảo đồng bộ, loại bỏ các phần tử bị null (missing)
            SerializedObject serializedRegistry = new SerializedObject(registrySO);
            SerializedProperty registryProperty = serializedRegistry.FindProperty("registry");

            List<string> options = new List<string>();
            _levelIndices.Clear();

            if (registryProperty != null && registryProperty.isArray)
            {
                for (int i = 0; i < registryProperty.arraySize; i++)
                {
                    SerializedProperty elementProp = registryProperty.GetArrayElementAtIndex(i);
                    LevelConfigSO elementAsset = elementProp.objectReferenceValue as LevelConfigSO;

                    if (elementAsset != null)
                    {
                        options.Add($"Level {elementAsset.LevelIndex} ({elementAsset.name})");
                        _levelIndices.Add(elementAsset.LevelIndex);
                    }
                }
            }

            _levelOptions = options.ToArray();

            // Giới hạn index đã chọn không bị vượt quá độ dài mảng mới
            if (_selectedTestIndex >= _levelOptions.Length)
            {
                _selectedTestIndex = Mathf.Max(0, _levelOptions.Length - 1);
            }
        }

        /// <summary>
        /// Xử lý logic lưu dữ liệu và đưa Unity vào PlayMode
        /// </summary>
        private void PlayTestLevel(int levelIndex)
        {
            // Đảm bảo Editor đang không ở Playmode sẵn
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                Debug.LogWarning("[Playtest] Đang trong PlayMode, tool đã tự ngắt. Vui lòng nhấn lại lần nữa để áp dụng level mới.");
                return;
            }

            // Gọi đến Repository lưu trữ của bạn để lưu thông tin màn chơi cần test trước khi bắt đầu
            // LƯU Ý: Hàm SaveProgress này bắt buộc phải là một static method của class ProgressRepository
            try 
            {
                // Thay thế đúng đường dẫn namespace class của bạn tại đây nếu bị báo đỏ lỗi biên dịch
                PlayerSave progress = PlayerSaveFile.LoadFromFile();
                progress.SetCurrentLevelIndex(levelIndex);
                PlayerSaveFile.SaveToFile(progress);
                Debug.Log($"<color=cyan>[Playtest]</color> Đã nạp thành công dữ liệu Level Index [<b>{levelIndex}</b>] vào hệ thống lưu trữ!");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Playtest] Lỗi khi gọi ProgressRepository.SaveProgress: {e.Message}\nHãy chắc chắn method này là static và gọi được trong Editor.");
                return;
            }

            // Kích hoạt Playmode tự động
            EditorApplication.isPlaying = true;
        }

        // --- GIỮ NGUYÊN CÁC HÀM CŨ CỦA BẠN BÊN DƯỚI KHÔNG THAY ĐỔI ---
        private void ExecuteScaffolding()
        {
            if (!AssetDatabase.IsValidFolder(TemplateFolderPath))
            {
                EditorUtility.DisplayDialog("Lỗi Hệ Thống", $"Không tìm thấy thư mục mẫu tại đường dẫn:\n{TemplateFolderPath}", "Kiểm tra lại");
                return;
            }

            string newFolderName = $"level_{_levelIndex}";
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
            System.Collections.Generic.List<(string oldPath, string newName)> assetsToRename = new System.Collections.Generic.List<(string, string)>();

            foreach (string guid in allAssetPaths)
            {
                string currentPath = AssetDatabase.GUIDToAssetPath(guid);
                string fileName = Path.GetFileNameWithoutExtension(currentPath);
                string extension = Path.GetExtension(currentPath);

                if (extension == ".prefab" && currentPath.Contains(destinationPath) && !currentPath.Contains("/Mission") && !currentPath.Contains("/Upgrade"))
                {
                    assetsToRename.Add((currentPath, $"Level {_levelIndex}"));
                    prefabPath = $"{Path.GetDirectoryName(currentPath)}/Level {_levelIndex}{extension}";
                }
                else if (extension == ".asset" && currentPath.Contains(destinationPath) && !currentPath.Contains("/Mission") && !currentPath.Contains("/Upgrade"))
                {
                    assetsToRename.Add((currentPath, $"Level {_levelIndex}"));
                    configPath = $"{Path.GetDirectoryName(currentPath)}/Level {_levelIndex}{extension}";
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
            
            EditorUtility.DisplayDialog("Thành công!", $"Đã cấu hình xong cây thư mục và đăng ký Level {_levelIndex}!", "Tuyệt vời");
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
                    DestroyImmediate(tempInstance);
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
                    indexProperty.intValue = _levelIndex;
                }

                serializedConfig.ApplyModifiedProperties();
                EditorUtility.SetDirty(configSO);
            }
        }

        private void RegisterToLevelRegistry(LevelConfigSO newConfigSO)
        {
            if (newConfigSO == null) return;

            LevelRegistrySO registrySO = AssetDatabase.LoadAssetAtPath<LevelRegistrySO>(RegistryAssetPath);

            if (registrySO == null)
            {
                Debug.LogError($"[Scaffolder] Không tìm thấy file LevelRegistry tại đường dẫn: {RegistryAssetPath}. Vui lòng tạo file trước!");
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

                    if (elementAsset.LevelIndex == _levelIndex)
                    {
                        existingIndexInList = i;
                    }
                }

                if (existingIndexInList != -1)
                {
                    SerializedProperty targetSlot = registryProperty.GetArrayElementAtIndex(existingIndexInList);
                    targetSlot.objectReferenceValue = newConfigSO;
                    Debug.Log($"<color=orange>[Registry]</color> Đã phát hiện trùng ID! Tiến hành thay thế file Config mới vào vị trí Level {_levelIndex} trong Registry.");
                }
                else
                {
                    int newSlotIndex = registryProperty.arraySize;
                    registryProperty.InsertArrayElementAtIndex(newSlotIndex);
                    registryProperty.GetArrayElementAtIndex(newSlotIndex).objectReferenceValue = newConfigSO;
                    Debug.Log($"<color=lime>[Registry]</color> Đã tự động đăng ký file Config Level {_levelIndex} vào danh sách Registry.");
                }

                serializedRegistry.ApplyModifiedProperties();
                EditorUtility.SetDirty(registrySO);
            }
            else
            {
                Debug.LogError("[Scaffolder] Không tìm thấy biến List tên là 'registry' trong file LevelRegistrySO!");
            }
        }

        /// <summary>
        /// Xóa sạch dữ liệu tiến trình cũ, đưa về trạng thái ban đầu
        /// </summary>
        private void ClearSaveData()
        {
            try
            {
                // 1. Reset file Save của Player về mặc định (Level 1)
                PlayerSave emptyPlayerSave = new PlayerSave();
                emptyPlayerSave.SetCurrentLevelIndex(1);
                PlayerSaveFile.SaveToFile(emptyPlayerSave);

                // 2. Reset file Save Tiến trình Level (level_progress.dat) về mặc định tinh khôi
                LevelProgressSave emptyLevelSave = new LevelProgressSave();
                LevelProgressSaveFile.SaveToFile(emptyLevelSave);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Playtest] Lỗi khi xóa dữ liệu save: {e.Message}");
            }
        }
    }
}
#endif
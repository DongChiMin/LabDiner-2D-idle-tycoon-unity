#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using LabDiner.Restaurant.SO;
using LabDiner.LevelSystem.Domain; // Giữ lại SO của bạn

namespace LabDiner.LevelEditor
{
    public class LevelManagerWindow : EditorWindow
    {
        private int _selectedTab = 0;
        private string[] _tabs = { "✨ Creator", "🎯 Missions", "⬆️ Upgrades" };

        private LevelCreatorTab _creatorTab;
        private LevelMissionTab _missionTab;
        private LevelUpgradeTab _upgradeTab;

        // Dữ liệu dùng chung cho Dropdown ở cả 3 Tab
        public int SelectedLevelDropdownIndex = 0;
        public string[] LevelOptions = new string[0];
        public List<LevelConfigSO> LevelConfigs = new List<LevelConfigSO>();

        public const string RegistryAssetPath = "Assets/_Project/Modules/Restaurant/Data/Levels/_Registry/LevelRegistry.asset";

        [MenuItem("LabDiner/🚀 Level Manager Tool")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelManagerWindow>("Level Manager");
            window.minSize = new Vector2(420, 480);
        }

        private void OnEnable()
        {
            RefreshLevelList();

            _creatorTab = new LevelCreatorTab(this);
            _missionTab = new LevelMissionTab(this);
            _upgradeTab = new LevelUpgradeTab(this);

            _creatorTab.OnEnable();
            _missionTab.OnEnable();
            _upgradeTab.OnEnable();
        }

        private void OnGUI()
        {
            GUILayout.Space(10);
            _selectedTab = GUILayout.Toolbar(_selectedTab, _tabs, GUILayout.Height(30));
            GUILayout.Space(15);

            switch (_selectedTab)
            {
                case 0:
                    _creatorTab.OnGUI();
                    break;
                case 1:
                    _missionTab.OnGUI();
                    break;
                case 2:
                    _upgradeTab.OnGUI();
                    break;
            }
        }

        /// <summary>
        /// Quét file LevelRegistrySO để cập nhật danh sách Dropdown dùng chung
        /// </summary>
        public void RefreshLevelList()
        {
            LevelRegistrySO registrySO = AssetDatabase.LoadAssetAtPath<LevelRegistrySO>(RegistryAssetPath);
            List<string> options = new List<string>();
            LevelConfigs.Clear();

            if (registrySO != null && registrySO.Levels != null)
            {
                foreach (var elementAsset in registrySO.Levels)
                {
                    if (elementAsset != null)
                    {
                        options.Add($"Level {elementAsset.LevelIndex} ({elementAsset.name})");
                        LevelConfigs.Add(elementAsset);
                    }
                }
            }

            LevelOptions = options.ToArray();
            
            if (SelectedLevelDropdownIndex >= LevelOptions.Length)
            {
                SelectedLevelDropdownIndex = Mathf.Max(0, LevelOptions.Length - 1);
            }
        }

        /// <summary>
        /// Lấy LevelIndex (số thứ tự) từ Dropdown đang được chọn
        /// </summary>
        public int GetCurrentSelectedLevelNumber()
        {
            if (LevelConfigs != null && SelectedLevelDropdownIndex < LevelConfigs.Count && LevelConfigs[SelectedLevelDropdownIndex] != null)
            {
                return LevelConfigs[SelectedLevelDropdownIndex].LevelIndex;
            }
            return 1;
        }

        /// <summary>
        /// Lấy file ConfigSO đang được chọn trong Dropdown
        /// </summary>
        public LevelConfigSO GetCurrentSelectedLevelConfig()
        {
            if (LevelConfigs != null && SelectedLevelDropdownIndex < LevelConfigs.Count)
            {
                return LevelConfigs[SelectedLevelDropdownIndex];
            }
            return null;
        }
    }
}
#endif
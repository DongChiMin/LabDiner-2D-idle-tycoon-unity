
using UnityEngine;

namespace LabDiner.GameSetting.UI
{
    public static class GameSettingVariables
    {
        // Tạo Keys tĩnh riêng tư ngay trong class (hoặc xài SettingKeys ở Cách 1)
        private const string KEY_MUSIC = "gamesetting.music";
        private const string KEY_EFFECT = "gamesetting.effect";
        private const string KEY_HAPTIC = "gamesetting.haptic";
        private const string KEY_LANGUAGE = "gamesetting.language";
        private const string KEY_FPS = "gamesetting.fps";

        // Property đóng gói việc Get / Set PlayerPrefs
        public static float Music
        {
            get => PlayerPrefs.GetFloat(KEY_MUSIC, 1.0f); // Mặc định 1.0
            set
            {
                PlayerPrefs.SetFloat(KEY_MUSIC, value);
                // Có thể thêm logic apply trực tiếp vào AudioMixer ở đây luôn!
            }
        }

        public static float Effect
        {
            get => PlayerPrefs.GetFloat(KEY_EFFECT, 1.0f); // Mặc định 1.0
            set => PlayerPrefs.SetFloat(KEY_EFFECT, value);
        }

        public static float Haptic
        {
            get => PlayerPrefs.GetFloat(KEY_HAPTIC, 1.0f); // Mặc định On (1)
            set => PlayerPrefs.SetFloat(KEY_HAPTIC, value);
        }

        public static string Language
        {
            get => PlayerPrefs.GetString(KEY_LANGUAGE, "English"); // Mặc định tiếng Anh
            set => PlayerPrefs.SetString(KEY_LANGUAGE, value);
        }

        public static int FPS
        {
            get => PlayerPrefs.GetInt(KEY_FPS, 60); // Mặc định 60 FPS
            set => PlayerPrefs.SetInt(KEY_FPS, value);
        }

        // Hàm gọi lưu tất cả khi đóng Menu Settings
        public static void SaveAll()
        {
            PlayerPrefs.Save();
        }

        public static void ResetToDefault()
        {
            Music = 1.0f;
            Effect = 1.0f;
            Haptic = 1.0f;
            Language = "English";
            FPS = 60;
            SaveAll();
        }
    }
}
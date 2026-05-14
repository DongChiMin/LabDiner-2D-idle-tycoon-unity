using System;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

namespace LabDiner.Shared
{
    public static class JSONExecutor
    {
        public static string ToJson(this object obj, bool prettyPrint = false)
        {
            return JsonConvert.SerializeObject(obj, prettyPrint ? Formatting.Indented : Formatting.None);
        }

        // Biến chuỗi JSON ngược lại thành Object
        public static T FromJson<T>(this string json)
        {
            if (string.IsNullOrEmpty(json)) return default;
            return JsonConvert.DeserializeObject<T>(json);
        }

        // Ghi chuỗi xuống file (có mã hóa Base64)
        public static void WriteToFile(this string content, string fileName, bool encrypt = true)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            
            if (encrypt)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                content = Convert.ToBase64String(bytes);
            }

            File.WriteAllText(path, content);
        }

        // Đọc chuỗi từ file
        public static string ReadFromFile(string fileName, bool decrypt = true)
        {
            string path = Path.Combine(Application.persistentDataPath, fileName);
            if (!File.Exists(path)) return null;

            string content = File.ReadAllText(path);

            if (decrypt)
            {
                try {
                    byte[] bytes = Convert.FromBase64String(content);
                    content = Encoding.UTF8.GetString(bytes);
                } catch {
                    Debug.LogWarning("File không phải dạng mã hóa hoặc bị lỗi!");
                    return null;
                }
            }

            return content;
        }
    
    }
}

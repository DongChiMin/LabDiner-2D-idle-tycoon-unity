using System;
using System.IO;
using LabDiner.Shared.Event;
using UnityEditor;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    // public enum MissionType
    // {
    //     UpgradeDish,      // Nâng cấp món a đến level b
    //     CollectTips,    // Nhặt a lần tiền tip
    //     HireChefs,      // Tổng số lượng đầu bếp đạt a
    //     SetupStation,  // Mở khóa trạm chính a
    //     CompleteOrder,     // Phục vụ món cho khách a lần
    //     BuyUpgrades,       // Mua a lần nâng cấp (có thể là bất kỳ nâng cấp nào, miễn là mua thành công)
    // }

    public abstract class BaseMissionSO : ScriptableObject
    {
        public string Id => name; // Mặc định ID của mission sẽ là tên của ScriptableObject, đảm bảo tính duy nhất
        public Action OnValueChanged;
        public Action OnMissionStart;

        [Header("Mission Info")]
        public string Title;
        public Sprite MissionIcon;
        public float TargetValue;

        [Header("Reward")]
        public BaseRewardSO Reward;
        public double RewardValue;

        // Hàm abstract để mỗi loại mission tự định nghĩa cách lấy giá trị hiện tại
        public abstract float GetCurrentValue();

        // Kiểm tra xem đã hoàn thành chưa
        public virtual bool IsCompleted() => GetCurrentValue() >= TargetValue;

        /// <summary>
        /// Gọi phương thức này khi người chơi hoàn thành nhiệm vụ và muốn nhận phần thưởng. Nó sẽ kích hoạt hiệu ứng bay gem từ vị trí startPos đến HUD.
        /// </summary>
        /// <param name="startPos"></param>
        public void ApplyReward(Vector3 startPos)
        {
            // Gửi chính Asset này đi qua Event
            if (Reward != null)
                Reward.ApplyReward(startPos, RewardValue);
        }

        #region Editor Only

protected virtual void OnValidate()
    {
#if UNITY_EDITOR
        if (Application.isPlaying) return;

        // Đăng ký delayCall để chạy sau khi GUI/Validate kết thúc
        EditorApplication.delayCall -= RenameAssetToMatchFormat;
        EditorApplication.delayCall += RenameAssetToMatchFormat;
#endif
    }

#if UNITY_EDITOR
    private void RenameAssetToMatchFormat()
    {
        if (this == null) return;

        string assetPath = AssetDatabase.GetAssetPath(this);
        if (string.IsNullOrEmpty(assetPath) || !assetPath.EndsWith(".asset")) return;

        // 1. Làm sạch Title
        string safeTitle = Title;
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            safeTitle = safeTitle.Replace(c, '_');
        }
        if (string.IsNullOrWhiteSpace(safeTitle)) safeTitle = "Mission";

        // 2. Tên mong muốn chuẩn (Tên gốc)
        string targetFileName = $"{safeTitle}_{TargetValue}";
        string currentFileName = Path.GetFileNameWithoutExtension(assetPath);

        // Nếu tên hiện tại ĐÃ BẮT ĐẦU bằng targetFileName (ví dụ: "Serve_10" hoặc "Serve_10 1", "Serve_10 2")
        // Kiểm tra xem file gốc (Serve_10.asset) có phải do MÌNH sở hữu không!
        string directory = Path.GetDirectoryName(assetPath);
        string originalPath = Path.Combine(directory, $"{targetFileName}.asset").Replace("\\", "/");

        // Lấy asset đang nắm giữ tên gốc (nếu có)
        UnityEngine.Object existingAsset = AssetDatabase.LoadMainAssetAtPath(originalPath);

        // Trường hợp 1: Tên gốc đã tồn tại VÀ đó là một Object KHÁC (file A gốc)
        if (existingAsset != null && existingAsset != this)
        {
            // Nếu file B hiện tại đã có dạng "Serve_10 X" rồi thì DỪNG LẠI, không đổi nữa để tránh loop!
            if (currentFileName.StartsWith(targetFileName))
            {
                return; 
            }
        }

        // Trường hợp 2: Nếu tên hiện tại đã chính xác là tên gốc rồi thì không làm gì cả
        if (currentFileName.Equals(targetFileName, System.StringComparison.Ordinal))
        {
            return;
        }

        // 3. Tự sinh tên duy nhất (chỉ chạy khi thực sự cần đổi tên)
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath(originalPath);
        string finalFileName = Path.GetFileNameWithoutExtension(uniquePath);

        // Nếu tên duy nhất sinh ra trùng với tên hiện tại thì ngưng
        if (currentFileName.Equals(finalFileName, System.StringComparison.Ordinal))
        {
            return;
        }

        // 4. Tiến hành đổi tên
        string result = AssetDatabase.RenameAsset(assetPath, finalFileName);
        if (string.IsNullOrEmpty(result))
        {
            AssetDatabase.SaveAssets();
        }
    }
#endif
        #endregion
    }
}
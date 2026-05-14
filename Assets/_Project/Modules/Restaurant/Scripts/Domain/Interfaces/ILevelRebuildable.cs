using LabDiner.Restaurant.SO;

namespace LabDiner.Restaurant.Interface
{
    /// <summary>
    /// Dùng cho các class cần dọn dẹp dữ liệu trong list, đọc object trên scene và đưa lại vào list
    /// </summary>
    public interface ILevelRebuildable
    {
        public void Rebuild();
    }
}
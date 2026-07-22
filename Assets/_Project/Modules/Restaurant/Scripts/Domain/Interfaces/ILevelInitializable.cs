using LabDiner.Restaurant.SO;

namespace LabDiner.Restaurant.Interface
{
    /// <summary>
    /// Interface cho các class được gọi ở phase 2 trong LevelLoader, khởi tạo các dữ liệu và thành phần prefab
    /// </summary>
    public interface ILevelInitializable
    {
        public void Init(LevelConfigSO config);
    }
}
using LabDiner.Restaurant.SO;
using LabDiner.Shared;

namespace LabDiner.Restaurant.Interface
{
    /// <summary>
    /// Interface cho các class được gọi ở phase 3 trong LevelLoader, load dữ liệu từ save game
    /// </summary>
    public interface ILevelProgress
    {
        public void LoadProgress();
        public void UpdateProgress();
    }
}
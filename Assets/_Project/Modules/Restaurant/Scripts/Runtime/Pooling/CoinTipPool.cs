using LabDiner.Restaurant.Environment;
using LabDiner.Shared.DesignPattern;

namespace LabDiner.Restaurant.Pooling
{
    public class CoinTipPool : SceneObjectPooling<CoinTip>
    {
        // Nếu cần thêm logic đặc biệt cho CoinTip khi trả về pool, có thể override ở đây
    }
}

using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Model;

namespace LabDiner.Restaurant.Interface
{
    public interface IOrderComposer
    {
        Order Compose(GuestContext guest, int maxUniqueStations, int maxTotalQty);
    }
}
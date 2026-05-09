using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;

namespace LabDiner.Restaurant.Runtime
{
    /// <summary>
    /// Ngẫu nhiên lượng món khác nhau và số lượng từng món
    /// </summary>
    public class DefaultOrderComposer : IOrderComposer
    {
        public Order Compose(GuestContext guest, int maxUniqueStations, int maxTotalQty)
        {
            Dictionary<CoreStation, int> orderDict =
                LevelManagerContext.Instance.CoreStationManager.GenerateRandomOrder(maxUniqueStations, maxTotalQty);

            return new Order(orderDict, guest, 0, false);
        }
    }
}
using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Manager;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant.Runtime
{
    /// <summary>
    /// Cố định 1 món và số lượng khác nhau
    /// </summary>
    public class FixedOrderComposer : IOrderComposer
    {
        readonly CoreStation _fixedStation;
        
        //Hàm khởi tạo
        public FixedOrderComposer(CoreStation fixedStation)
        {
            _fixedStation = fixedStation;
        }

        // Tạo order với 1 món cố định và số lượng ngẫu nhiên từ 1 đến maxTotalQty
        public Order Compose(GuestContext guest, int maxUniqueStations, int maxTotalQty)
        {
            int quantity = Random.Range(1, maxTotalQty + 1);
            var dict = new Dictionary<CoreStation, int> { { _fixedStation, quantity } };
            return new Order(dict, guest, 0, false);
        }
    }
}
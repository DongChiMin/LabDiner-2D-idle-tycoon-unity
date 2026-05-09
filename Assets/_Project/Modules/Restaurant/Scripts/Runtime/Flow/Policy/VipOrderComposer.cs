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
    /// Gọi nhiều món với số lượng lớn
    /// </summary>
    public class VipOrderComposer : IOrderComposer
    {
        readonly int _quantityMultiplier;
        
        //Hàm khởi tạo
        public VipOrderComposer(int quantityMultiplier)
        {
            _quantityMultiplier = quantityMultiplier;
        }

        // Tạo order với nhiều món và số lượng lớn
        public Order Compose(GuestContext guest, int maxUniqueStations, int maxTotalQty)
        {
            int quantity = Random.Range(1, maxTotalQty + 1) * _quantityMultiplier;
            Dictionary<CoreStation, int> orderDict =
                LevelManagerContext.Instance.CoreStationManager.GenerateRandomOrder(maxUniqueStations, quantity);

            return new Order(orderDict, guest, 0, false);
        }
    }
}
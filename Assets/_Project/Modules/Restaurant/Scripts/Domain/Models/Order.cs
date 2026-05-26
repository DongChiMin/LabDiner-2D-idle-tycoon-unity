
using System.Collections.Generic;
using LabDiner.Restaurant.Environment;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;

namespace LabDiner.Restaurant.Model
{
    [System.Serializable]
    public class Order
    {
        public Dictionary<CoreStation, int> OrderDict; // CoreStation là món ăn, int là số lượng
        public GuestContext OrderBy;
        public double Profit = 0; // Lợi nhuận thu được từ order này, sẽ được câph nhật lại sau
        public bool IsServed = false; // Đánh dấu xem order đã được phục vụ hay chưa

        public Order(Dictionary<CoreStation, int> orderDict, GuestContext orderBy, double profit, bool isServed)
        {
            OrderDict = orderDict;
            OrderBy = orderBy;
            Profit = profit;
            IsServed = isServed;
        }   
    }
}
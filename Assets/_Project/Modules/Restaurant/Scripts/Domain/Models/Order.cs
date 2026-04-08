
using System.Collections.Generic;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class Order : IStaffTask
    {
        public Dictionary<CoreStation, int> OrderDict; // CoreStation là món ăn, int là số lượng
        public GuestContext OrderBy;
        public float Profit = 0; // Lợi nhuận thu được từ order này, sẽ được câph nhật lại sau
        public bool IsServed = false; // Đánh dấu xem order đã được phục vụ hay chưa

        public Order(Dictionary<CoreStation, int> orderDict, GuestContext orderBy, float profit, bool isServed)
        {
            OrderDict = orderDict;
            OrderBy = orderBy;
            Profit = profit;
            IsServed = isServed;
        }   
    }
}
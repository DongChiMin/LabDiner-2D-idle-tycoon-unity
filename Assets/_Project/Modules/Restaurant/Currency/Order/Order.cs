
using System.Collections.Generic;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class Order : IStaffTask
    {
        public Dictionary<CoreStation, int> _orderDict; // CoreStation là món ăn, int là số lượng
        public GuestContext _orderBy;
        public float profit = 0; // Lợi nhuận thu được từ order này, sẽ được câph nhật lại sau
        public bool isServed = false; // Đánh dấu xem order đã được phục vụ hay chưa

        public Order(Dictionary<CoreStation, int> orderDict, GuestContext orderBy, float profit, bool isServed)
        {
            _orderDict = orderDict;
            _orderBy = orderBy;
            this.profit = profit;
            this.isServed = isServed;
        }   
    }
}
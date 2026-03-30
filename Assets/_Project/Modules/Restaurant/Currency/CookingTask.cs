
namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class CookingTask : IStaffTask
    {
        public Order Order;
        public Station StationTarget;
        public CoreStation CoreStation;
        public PassTable PassTableTarget;
        // Bạn có thể thêm các thông tin như thời gian nấu, prefab món ăn...

        public CookingTask(Order parentOrder, CoreStation coreStation)
        {
            Order = parentOrder;
            CoreStation = coreStation;
        }

        public CookingTask(Station station, PassTable passTable)
        {
            StationTarget = station;
            PassTableTarget = passTable;
        }
    }
}
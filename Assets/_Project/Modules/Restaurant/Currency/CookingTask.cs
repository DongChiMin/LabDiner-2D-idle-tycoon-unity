
namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class CookingTask : IStaffTask
    {
        public Order ParentOrder { get; private set; }
        public CoreStation StationType { get; private set; }
        // Bạn có thể thêm các thông tin như thời gian nấu, prefab món ăn...

        public CookingTask(Order parentOrder, CoreStation stationType)
        {
            ParentOrder = parentOrder;
            StationType = stationType;
        }
    }
}
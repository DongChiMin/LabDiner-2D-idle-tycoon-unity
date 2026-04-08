
namespace LabDiner.Restaurant
{
    public interface IStaff
    {
        bool IsAvailable { get; set; }
        void DoTask(IStaffTask task);

        void OnTaskCompleted(IStaffTask task);
    }
}
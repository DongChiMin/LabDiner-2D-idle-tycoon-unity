
namespace LabDiner.Restaurant
{
    public interface IStaff
    {
        void DoTask(IStaffTask task);

        void OnTaskCompleted(IStaffTask task);
    }
}
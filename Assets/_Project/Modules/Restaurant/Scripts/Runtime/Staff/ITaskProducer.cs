using LabDiner.Restaurant.Humanoid;

namespace LabDiner.Restaurant.Interface
{
    public interface ITaskProducer
    {
        // Trình đăng ký task (thường là tham chiếu đến TaskRegistrySO)
        void PublishTask();
        
        // Callback khi có nhân viên bắt đầu nhận task này
        // void OnTaskAccepted(Staff staff);
        
        // Callback khi nhân viên hoàn thành xong việc
        void OnTaskCompleted();
        
        // Thu hồi task nếu không còn cần thiết nữa
        // void RevokeTask();
    }
}
using UnityEngine;

namespace LabDiner.Shared.Input
{
    public interface IInteractable
    {
        // Hàm này sẽ tự định nghĩa hành động khi được click
        void OnInteract(); 
        
        // (Tùy chọn) Có thể thêm để check xem lúc này có cho click không
        bool CanInteract(); 
    }
}

using UnityEngine;

namespace LabDiner.Shared
{
    public interface IPoolable
    {
        void ReturnToPool();
    }
    public interface IClickable
    {
        void OnPointerUp(Vector3 worldPosition);
        IDraggable OnPointerDown(Vector3 worldPosition);
    }
    public interface IDraggable
    {
        void OnDragStart(Vector3 worldPosition);
        void OnDragContinue(Vector3 worldPosition);
        void OnDragEnd();
        Transform Transform { get; }
    }

    public interface IIngredient
    {
        IngredientSO IngredientData { get; }
        // Có thể thêm các thuộc tính nấu ăn như:
        // float Freshness { get; } 
    }

    public interface IDropZone
    {
        bool CanAccept(IDraggable draggable);
        void OnDrop(IDraggable draggable);
    }
}

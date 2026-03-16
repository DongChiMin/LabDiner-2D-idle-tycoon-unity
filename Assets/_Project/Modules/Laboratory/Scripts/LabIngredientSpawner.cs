using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Laboratory
{
    public class LabIngredientSpawner : MonoBehaviour, IClickable
    {
        [Header("Spawner Settings")]
        [SerializeField] bool _isDisableWhenClick;
        [SerializeField] Collider2D _collider;
        [SerializeField] GameObject _visual;
        [SerializeField] private IngredientSO _ingredientData;
        public void OnPointerUp(Vector3 worldPosition)
        {

        }

        public IDraggable OnPointerDown(Vector3 worldPosition)
        {
            if (_isDisableWhenClick)
            {
                ToggleSpawner(false);
            }

            // 1. Spawn item từ pool
            var go = PoolManager.Instance.IngredientPool.Get(worldPosition + Vector3.down * 0.5f, Quaternion.identity);
            LabIngredientContext labIngredientContext = go.GetComponent<LabIngredientContext>();

            //2. Set dữ liệu cho item mới spawn
            labIngredientContext.Init(this, _ingredientData);
            

            // 3. Lấy component Draggable từ item mới
            if (go.TryGetComponent(out IDraggable newDraggable))
            {
                return newDraggable;
            }

            return null;
        }

        public void ToggleSpawner(bool isEnabled)
        {
            Debug.Log("TODO: làm hiệu ứng xuất hiện + biến mất cho spawner");
            if (_collider != null)
                _collider.enabled = isEnabled;

            if (_visual != null)
                _visual.SetActive(isEnabled);
        }
    }
}

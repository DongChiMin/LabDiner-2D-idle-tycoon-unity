using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Laboratory
{
    public class RecipeDraft : MonoBehaviour
    {
        [Header("Broadcasting Events")]
        [SerializeField] private IngredientEvent onIngredientDroppedIn;
        [SerializeField] private GameObjectEvent onIngredientDroppedInGO;

        private List<IngredientSO> currentIngredients = new List<IngredientSO>();

        void OnEnable()
        {
            onIngredientDroppedIn.Register(HandleIngredientDroppedIn);
        }

        void OnDisable()
        {
            onIngredientDroppedIn.Unregister(HandleIngredientDroppedIn);
        }

        private void HandleIngredientDroppedIn(IngredientSO ingredient)
        {
            
        }
    }
}
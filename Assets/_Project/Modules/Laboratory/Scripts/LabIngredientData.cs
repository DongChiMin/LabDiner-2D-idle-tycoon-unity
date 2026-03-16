using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Laboratory
{
    [RequireComponent(typeof(Rigidbody2D), typeof(TargetJoint2D))]
    public class LabIngredientData : MonoBehaviour, IIngredient
    {
        private IngredientSO data; 
        public IngredientSO IngredientData => data;
        public object GetData() => data;

        public void Init(IngredientSO ingredientData)
        {
            data = ingredientData;
        }
    }
}
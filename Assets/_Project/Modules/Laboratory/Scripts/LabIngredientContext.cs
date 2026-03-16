using LabDiner.Shared;
using UnityEngine;

namespace LabDiner.Laboratory
{
    public class LabIngredientContext : MonoBehaviour
    {
        [SerializeField] private LabIngredientData data;
        [SerializeField] private LabIngredientDrag drag;
        [SerializeField] private SpawnerMember spawnerMember;
        [SerializeField] private PoolMember poolMember;
        public LabIngredientData CtxData => data;
        public LabIngredientDrag CtxDrag => drag;
        public SpawnerMember CtxSpawnerMember => spawnerMember;
        public PoolMember CtxPoolMember => poolMember;

        public void Init(LabIngredientSpawner spawner, IngredientSO ingredientData)
        {
            data.Init(ingredientData);
            spawnerMember.SetOrigin(spawner);
        }
    }
}
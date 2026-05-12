using UnityEngine;

namespace LabDiner.Shared.SO
{
    [CreateAssetMenu(menuName = "SO/Runtime/Int")]
    public class IntRuntimeSO : BaseVariableRuntimeSO<int>
    {
        public void Add(int amount) => SetValue(Value + amount);
    }
}
using UnityEngine;

namespace LabDiner.Shared.SO
{
    [CreateAssetMenu(menuName = "SO/Runtime/Double")]
    public class DoubleRuntimeSO : BaseVariableRuntimeSO<double>
    {
        public void Add(double amount) => SetValue(Value + amount);
    }
}
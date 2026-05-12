using System;
using UnityEngine;
namespace LabDiner.Shared.SO
{
    public abstract class BaseVariableRuntimeSO<T> : ScriptableObject
    {
        public T Value;
        public Action<T> OnValueChanged;

        public void SetValue(T newValue)
        {
            Value = newValue;
            OnValueChanged?.Invoke(Value);
        }
    }
}
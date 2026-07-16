using System;
using System.Collections.Generic;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant.SO
{
    [CreateAssetMenu(fileName = "TutorialRuntimeSet", menuName = "SO/Runtime/Tutorial")]
    public partial class TutorialRuntimeSO : ScriptableObject
    {
        public Action OnLevelStarted;
        public Action<Vector2> OnCoreStationStartRun;
    }
}
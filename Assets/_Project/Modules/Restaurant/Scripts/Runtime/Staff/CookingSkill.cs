using System;
using System.Collections;
using LabDiner.Restaurant.Enum;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Interface;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class CookingSkill : StaffSkill
    {
        public override TaskType SkillType => TaskType.Cooking;
        public override IEnumerator PerformTask(BaseTask task, Action onComplete)
        {
            throw new NotImplementedException();
        }
    }
}
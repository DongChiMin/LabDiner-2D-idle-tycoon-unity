using System.Collections.Generic;
using LabDiner.Shared.Events;
using UnityEngine;

namespace LabDiner.Shared
{
    [CreateAssetMenu(fileName = "OnUIPanelOpened", menuName = "Events/UI/UI Panel Event")]
    public class UIPanelEvent : GameEvent<BasePanel> { }
}
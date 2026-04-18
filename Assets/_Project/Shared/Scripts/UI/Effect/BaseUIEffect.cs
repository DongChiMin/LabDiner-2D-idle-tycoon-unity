using UnityEngine;
using DG.Tweening;
using System;

namespace LabDiner.Shared.UI
{
    public abstract class BaseUIEffect : MonoBehaviour
    {
        public float Duration => _duration;
        [Header("Settings")]
        [SerializeField] protected float _duration = 0.3f;
        public abstract void Show(Action onComplete = null);
        public abstract void Hide(Action onComplete = null);
    }
}
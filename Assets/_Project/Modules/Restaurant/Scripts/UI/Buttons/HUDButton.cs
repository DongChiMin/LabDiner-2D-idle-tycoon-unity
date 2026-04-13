using System;
using System.Collections.Generic;
using LabDiner.Shared;
using LabDiner.Shared.SO;
using UnityEngine;
using UnityEngine.UI;

namespace LabDiner.Restaurant
{
    public class HUDButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private UIPopupEvent _onPopupShow;

        void Awake()
        {
            _button.onClick.AddListener(() => {
                _onPopupShow.Raise();
            });
        }
    }
}

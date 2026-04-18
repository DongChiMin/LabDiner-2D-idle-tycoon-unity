
using System;
using System.Collections.Generic;
using LabDiner.Shared.Input;
using UnityEngine;

namespace LabDiner.Restaurant
{
    [System.Serializable]
    public class Station : MonoBehaviour, IInteractable
    {
        public Action OnClickStation;
        public Transform WorkPos => _workPos;
        public bool IsAvailable => _isAvailable;
        [SerializeField] private StationEvent onStationAvailable;
        
        [Header("[DEBUG]")]
        [SerializeField] private Transform _workPos;
        [SerializeField] private bool _isAvailable = true;
        
        #region API
        
        /// <summary>
        /// Cập nhật trạng thái của trạm (bận/rảnh) và kích hoạt sự kiện khi trạm trở nên sẵn sàng.
        /// </summary>
        /// <param name="isAvailable"></param>
        public void SetStatus(bool isAvailable)
        {
            _isAvailable = isAvailable;
            if(_isAvailable)
            {
                onStationAvailable.Raise(this);
            }
        }

        public void OnInteract()
        {
            OnClickStation?.Invoke();
        }

        public bool CanInteract()
        {
            return true;
        }

        #endregion
    }
}
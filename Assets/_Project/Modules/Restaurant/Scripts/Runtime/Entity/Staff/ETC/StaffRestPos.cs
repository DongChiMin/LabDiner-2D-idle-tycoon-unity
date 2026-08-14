using System.Collections.Generic;
using LabDiner.Restaurant.Humanoid;
using LabDiner.Restaurant.Model;
using LabDiner.Restaurant.SO;
using LabDiner.Restaurant.Workflow;
using UnityEngine;

namespace LabDiner.Restaurant
{
    public class StaffRestPos : MonoBehaviour
    {
        public bool IsOccupied => _isOccupied;
        public StaffType StaffType => _staffType;


        [SerializeField] private StaffType _staffType;
        private bool _isOccupied;

        public void SetOccupied(bool occupied)
        {
            _isOccupied = occupied;
        }
    }
}